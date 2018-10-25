using System;
using System.Reflection;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.ReadOnlyClassManipulation
{
    class WriteToDatalockOutput 
    {
        /// <summary>
        /// Really dodgy, but it's much cleaner than the alternatives
        /// </summary>
        public static void SetTransactionTypeFlag(DatalockOutput obj, int value)
        {
            var type = typeof(DatalockOutput);
            var fieldName = $"<{nameof(DatalockOutput.TransactionTypeGroup)}>k__BackingField";
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new ApplicationException("The backing field naming strategy may have changed, please investigate the code but be mindful of other compilers");
            }
            field.SetValue(obj, value);
        }
    }
}
