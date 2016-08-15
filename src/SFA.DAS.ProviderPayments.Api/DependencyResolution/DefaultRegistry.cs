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

using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using MediatR;
using StructureMap;
using StructureMap.Graph;

namespace SFA.DAS.ProviderPayments.Api.DependencyResolution
{

    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.ProviderPayments";

        public DefaultRegistry()
        {
            Scan(
               scan =>
               {
                   scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            RegisterAutomapper();
            RegisterMediatr();
            RegisterHttpPipeline();
        }

        private void RegisterAutomapper()
        {
            For<AutoMapper.MapperConfiguration>().Use(Infrastructure.Mapping.AutoMapperConfiguration.Configure());
        }

        private void RegisterHttpPipeline()
        {
            For<HttpContext>()
                .AlwaysUnique().Use(ctx => HttpContext.Current);

            For<HttpRequestMessage>()
                .AlwaysUnique()
                .Use(ctx => ctx.GetInstance<HttpContext>().Items["MS_HttpRequestMessage"] as HttpRequestMessage);

            For<UrlHelper>()
                .AlwaysUnique()
                .Use(ctx => new UrlHelper(ctx.GetInstance<HttpRequestMessage>()));
        }

        private void RegisterMediatr()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
    }
}