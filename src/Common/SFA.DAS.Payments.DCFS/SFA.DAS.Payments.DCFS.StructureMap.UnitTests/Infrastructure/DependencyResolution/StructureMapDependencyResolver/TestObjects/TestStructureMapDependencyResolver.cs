using System;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;

namespace SFA.DAS.Payments.DCFS.StructureMap.UnitTests.Infrastructure.DependencyResolution.StructureMapDependencyResolver.TestObjects
{
    public class TestStructureMapDependencyResolver : StructureMap.Infrastructure.DependencyResolution.StructureMapDependencyResolver
    {
        protected override Registry CreateRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            return new TestRegistry();
        }
    }
}
