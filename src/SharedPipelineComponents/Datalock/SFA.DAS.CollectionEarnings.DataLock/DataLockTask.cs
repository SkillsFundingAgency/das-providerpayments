﻿using System;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : DcfsTask
    {
        private readonly IDependencyResolver _dependencyResolver;
        private const string DataLockSchema = "DataLock";

        public static int CommandTimeout = 180;

        public DataLockTask()
            : base(DataLockSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public DataLockTask(IDependencyResolver dependencyResolver)
            : base(DataLockSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            SetCommandTimeout(context);
            _dependencyResolver.Init(typeof(DataLockProcessor), context);

            var processor = _dependencyResolver.GetInstance<DataLockProcessor>();

            processor.Process();
        }

        void SetCommandTimeout(ContextWrapper context)
        {
            try
            {
                var timeout = context.GetPropertyValue(DataLockContextPropertyKeys.CommandTimeout);
                if (timeout != null)
                {
                    int configuredTimeout;
                    if (int.TryParse(timeout, out configuredTimeout))
                    {
                        CommandTimeout = configuredTimeout;
                    }
                }
            }
            catch (Exception)
            {
                // Always continue using the default value
            }
        }

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            base.IsValidContext(contextWrapper);

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(DataLockContextPropertyKeys.YearOfCollection)))
            {
                throw new InvalidContextException(DataLockException.ContextPropertiesNoYearOfCollectionMessage);
            }

            return ValidateYearOfCollection(contextWrapper.GetPropertyValue(DataLockContextPropertyKeys.YearOfCollection));
        }

        private bool ValidateYearOfCollection(string yearOfCollection)
        {
            int year1;
            int year2;

            if (yearOfCollection.Length != 4 ||
                !int.TryParse(yearOfCollection.Substring(0, 2), out year1) ||
                !int.TryParse(yearOfCollection.Substring(2, 2), out year2) ||
                (year2 != year1 + 1))
            {
                throw new InvalidContextException(DataLockException.ContextPropertiesInvalidYearOfCollectionMessage);
            }

            return true;
        }
    }
}
