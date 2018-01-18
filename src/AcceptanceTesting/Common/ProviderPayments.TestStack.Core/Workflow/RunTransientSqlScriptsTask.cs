using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal abstract class RunTransientSqlScriptsTask : WorkflowTask
    {
        private readonly ComponentType[] _componentTypes;
        private readonly string _scriptsRegex;
        private readonly bool _withScriptOrder;
        private readonly ILogger _logger;

        protected RunTransientSqlScriptsTask(ComponentType[] componentTypes, string scriptsRegex, bool withScriptOrder, ILogger logger)
        {
            _componentTypes = componentTypes;
            _scriptsRegex = scriptsRegex;
            _withScriptOrder = withScriptOrder;
            _logger = logger;
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
            _logger.Debug($"Starting running transient sql scripts for {componentType}");

            var componentDirectory = GetComponentWorkingDirectory(componentType, context);
            var scripts = GetScripts(componentDirectory,context.OpaRulebaseYear);
            if (scripts.Length == 0)
            {
                _logger.Debug($"No scripts found for {componentType}");
                return;
            }

            using (var transientConnection = GetOpenTransientConnection(context))
            {
                foreach (var script in scripts)
                {
                    if (script.Contains("15 PeriodEnd"))
                    {
                        var breakpoint = "";
                    }
                    _logger.Debug($"Running script {script} for {componentType}");

                    var sql = File.ReadAllText(script);

                    ExecuteSqlScript(sql, transientConnection, context);
                }
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

        protected string[] GetScripts(string componentDirectory,string opaRulebaseYear)
        {
            var sqlDirectory = Path.Combine(componentDirectory, "sql", "dml");
            if (!Directory.Exists(sqlDirectory))
            {
                sqlDirectory = Path.Combine(componentDirectory, opaRulebaseYear, "sql", "dml");
            }
            if (!Directory.Exists(sqlDirectory))
            {
                return new string[0];
            }

            return Directory.GetFiles(sqlDirectory)
                            .Select(path =>
                            {
                                var match = Regex.Match(Path.GetFileName(path), _scriptsRegex);
                                return new
                                {
                                    Path = path,
                                    IsRequired = match.Success,
                                    Order = _withScriptOrder && match.Success ? int.Parse(match.Groups[1].Value) : 0
                                };
                            })
                            .Where(x => x.IsRequired)
                            .OrderBy(x => x.Order)
                            .Select(x => x.Path)
                            .ToArray();
        }
    }
}