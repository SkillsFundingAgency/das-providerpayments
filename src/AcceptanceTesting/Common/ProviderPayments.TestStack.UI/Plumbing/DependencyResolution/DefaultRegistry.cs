// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Azure;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Infrastructure.Data;
using ProviderPayments.TestStack.UI.Plumbing.Mapping;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureStorageQueue;
using StructureMap;

namespace ProviderPayments.TestStack.UI.Plumbing.DependencyResolution {
    public class DefaultRegistry : Registry {
        public DefaultRegistry() {
            Scan(
               scan =>
               {
                   scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("ProviderPayments.TestStack"));

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<AutoMapper.MapperConfiguration>().Use(MapperConfiguration.Configure());

            For<IMessagePublisher>().Use<AzureStorageQueueService>()
                .Ctor<string>("connectionString").Is(CloudConfigurationManager.GetSetting("RequestQueueConnectionString"))
                .Ctor<string>("queueName").Is("teststackprocessrequests");

            For<ICommitmentRepository>().Use<SqlServerCommitmentRepository>();
        }
    }
}