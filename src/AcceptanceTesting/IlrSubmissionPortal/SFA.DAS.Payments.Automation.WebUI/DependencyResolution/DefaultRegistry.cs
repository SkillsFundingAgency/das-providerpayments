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

namespace SFA.DAS.Payments.Automation.WebUI.DependencyResolution {
    using Automation.Infrastructure.Data;
    using Lars;
    using MediatR;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry {

        private const string ServiceName = "SFA.DAS.Payments.Automation";
        private string _connectionString = string.Empty;


        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            _connectionString = Microsoft.Azure.CloudConfigurationManager.GetSetting("ConnectionString");
            var apiBaseUrl = System.Configuration.ConfigurationManager.AppSettings["PaymentsApiUrl"];

            For<IReferenceDataRepository>().Use<ReferenceDataRepository>().Ctor<string>("connectionString").Is(_connectionString);
            For<ILarsRepository>().Use<AutomationLarsSqlRepository>().Ctor<string>("connectionString").Is(_connectionString);
            For<ILearnersRepository>().Use<LearnersRepository>().Ctor<string>("connectionString").Is(_connectionString);
           For<IPaymentsClient>().Use<PaymentsApiClient>().Ctor<string>("apiBaseUrl").Is(apiBaseUrl)
                                                            .Ctor<string>("clientToken").Is("");


            RegisterMediator();


        }

        private void RegisterMediator()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }


    }
}
