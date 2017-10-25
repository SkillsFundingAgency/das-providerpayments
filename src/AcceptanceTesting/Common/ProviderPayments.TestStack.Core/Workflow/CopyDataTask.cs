using System.IO;
using CS.Common.SqlBulkCopyCat;
using CS.Common.SqlBulkCopyCat.Model.Config.Builder;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal abstract class CopyDataTask : WorkflowTask
    {
        private readonly ComponentType[] _componentTypes;
        private readonly ILogger _logger;
        private readonly DataCopyDirection _copyDirection = DataCopyDirection.TransientToDeds;

        protected CopyDataTask(ComponentType[] componentTypes, ILogger logger)
        {
            _componentTypes = componentTypes;
            _logger = logger;
        }
        protected CopyDataTask(ComponentType[] componentTypes, ILogger logger,DataCopyDirection copyDirection)
        {
            _componentTypes = componentTypes;
            _logger = logger;
            _copyDirection = copyDirection;
        }

        internal override void Execute(TestStackContext context)
        {
            foreach (var component in _componentTypes)
            {
                RunForComponent(component, context);
            }
        }

        private void RunForComponent(ComponentType componentType, TestStackContext context)
        {
            _logger.Debug($"Starting running copy to deds mappings for {componentType}");

            var componentDirectory = GetComponentWorkingDirectory(componentType, context);
            var mappings = GetMappings(componentDirectory,context.OpaRulebaseYear);
            if (mappings.Length == 0)
            {
                _logger.Debug($"No mappings found for {componentType}");
                return;
            }

            foreach (var mapping in mappings)
            {
                var config = new CopyCatConfigBuilder().FromXmlFile(mapping);
                config.SourceConnectionString = _copyDirection == DataCopyDirection.TransientToDeds ? context.TransientConnectionString : context.DedsDatabaseConnectionString;
                config.DestinationConnectionString = _copyDirection == DataCopyDirection.TransientToDeds ? context.DedsDatabaseConnectionString : context.TransientConnectionString;

                var copyCat = new CopyCat(config);
                copyCat.Copy();
            }
        }

        protected string GetComponentWorkingDirectory(ComponentType componentType, TestStackContext context)
        {
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

        protected string[] GetMappings(string componentDirectory,string collectionYear)
        {
            var mappingsDirectory = Path.Combine(componentDirectory, "copy mappings");
            if (!Directory.Exists(mappingsDirectory))
            {
                mappingsDirectory = Path.Combine(componentDirectory,collectionYear, "copy mappings");
            }
            if (!Directory.Exists(mappingsDirectory))
            {
                return new string[0];
            }

            var searchPattern = _copyDirection == DataCopyDirection.TransientToDeds ? "*CopyToDeds*.xml" : "*CopyToTransient*.xml";

            return Directory.GetFiles(mappingsDirectory, searchPattern);
        }
    }
}