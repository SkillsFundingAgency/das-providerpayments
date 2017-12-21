using ProviderPayments.TestStack.Core.Context;
using System;
using System.IO;

namespace ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase.Tasks
{
    internal abstract class BaseRebuildDedsDatabaseTask :WorkflowTask
    {

        protected string GetComponentWorkingDirectory(TestStackContext context)
        {
            var componentType = (ComponentType)int.Parse(context.RequestContent);

            var componentsDirectory = new DirectoryInfo(Path.Combine(context.WorkingDirectory, "components"));
            if (!componentsDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Cannot find components directory {componentsDirectory.FullName}");
            }

            foreach (var componentDirectory in componentsDirectory.GetDirectories())
            {
                if (componentDirectory.Name.StartsWith(componentType.ToString()))
                {
                    return componentDirectory.FullName;
                }
            }

            throw new DirectoryNotFoundException($"Cannot find component directory for {componentType} in {componentsDirectory.FullName}");
        }

        internal override void Execute(TestStackContext context)
        {
            throw new NotImplementedException();
        }
    }
}
