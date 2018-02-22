using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.ExecutionProxy;
using TestStackContext = ProviderPayments.TestStack.Core.Context.TestStackContext;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal abstract class RunExternalTask : WorkflowTask
    {
        private readonly ComponentType _componentType;
        private readonly string _assemblyName;
        private readonly string _typeName;
        private readonly ILogger _logger;

        protected RunExternalTask(ComponentType componentType, string assemblyName, string typeName, ILogger logger)
        {
            _componentType = componentType;
            _assemblyName = assemblyName;
            _typeName = typeName;
            _logger = logger;
        }

        internal override void Prepare(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(context);
            PrepareDatabase(componentDirectory, context);
            PrepareExecutables(componentDirectory,context.OpaRulebaseYear);
        }
        internal override void PreRun(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(context);
            PrepareForExecution(componentDirectory, context);
        }
        internal override void Execute(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(context);
            var executablesDirectory = Path.Combine(componentDirectory, "component");
            if (!Directory.Exists(executablesDirectory))
            {
                executablesDirectory = Path.Combine(componentDirectory, context.OpaRulebaseYear, "component");
            }
            if (!Directory.Exists(executablesDirectory))
            {
                return;
            }

            var appDomain = AppDomainProvider.GetAppDomain(executablesDirectory);

            var proxyTask = GetExecutionProxy(appDomain);
            var proxyContext = GetProxyContext(context);
            proxyContext.Properties.Add("ExecutablesDirectory", executablesDirectory);

            proxyTask.Execute(proxyContext);
            
        }

        private string GetComponentWorkingDirectory(TestStackContext context)
        {
            var componentsDirectory = new DirectoryInfo(Path.Combine(context.WorkingDirectory, "components"));
            if (!componentsDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Cannot find components directory {componentsDirectory.FullName}");
            }

            foreach (var componentDirectory in componentsDirectory.GetDirectories())
            {
                if (componentDirectory.Name.StartsWith(_componentType.ToString()))
                {
                    return componentDirectory.FullName;
                }
            }

            throw new DirectoryNotFoundException($"Cannot find component directory for {_componentType} in {componentsDirectory.FullName}");
        }

        private bool ShouldScriptBeRan(string sqlFile)
        {
            return DdlManager.IsScriptRequiredOnEveryRun(sqlFile);
        }

        private void PrepareDatabase(string componentDirectory, TestStackContext context)
        {
            _logger.Debug($"Preparing database for component {_componentType}");

            var sqlDirectory = Path.Combine(componentDirectory, "sql", "ddl");
            if (!Directory.Exists(sqlDirectory))
            {
                sqlDirectory = Path.Combine(componentDirectory,context.OpaRulebaseYear, "sql", "ddl");
            }
            if (!Directory.Exists(sqlDirectory))
            {
                return;
            }

            using (var transientConnection = GetOpenTransientConnection(context))
            //using (var dedsConnection = GetOpenDedsConnection(context))
            {
                try
                {
                    var runAllDdl = !DdlManager.HasRan(componentDirectory);

                    foreach (var sqlFile in GetOrderedSqlFiles(sqlDirectory))
                    {
                        _logger.Debug($"Found script {sqlFile}");

                        if (!runAllDdl && !ShouldScriptBeRan(sqlFile))
                        {
                            _logger.Debug($"Skipping DDL file on subsequent runs - {sqlFile}");
                            continue;
                        }

                        var isDedsScript = Regex.IsMatch(sqlFile, @".*\.deds\..*\.sql$", RegexOptions.IgnoreCase);
                        if (!isDedsScript)
                        {
                            var sql = File.ReadAllText(sqlFile);

                            _logger.Debug($"Executing {sqlFile}");
                            ExecuteSqlScript(sql, transientConnection, context);
                        }
                        else
                        {
                            _logger.Info($"Skipping {sqlFile} as it for DEDS");
                        }
                    }
                }
                finally
                {
                    //dedsConnection.Close();
                    transientConnection.Close();
                }
            }
        }

     
        private void PrepareExecutables(string componentDirectory,string opaRulebaseYear)
        {
            var executablesDirectory = Path.Combine(componentDirectory, "component");

            if (!Directory.Exists(executablesDirectory))
            {
                executablesDirectory = Path.Combine(componentDirectory, opaRulebaseYear, "component");
            }

            if (!Directory.Exists(executablesDirectory))
            {
                return;
            }

            var sourcePath = typeof(LateBoundTaskProxy).Assembly.Location ?? string.Empty;
            var destinationPath = Path.Combine(executablesDirectory, Path.GetFileName(sourcePath));
            if (File.Exists(destinationPath))
                return;
            
            File.Copy(sourcePath, destinationPath, true);
        }
        private void PrepareForExecution(string componentDirectory, TestStackContext context)
        {
            _logger.Debug($"Preparing database for component {_componentType} execution (PreRun)");

            var sqlDirectory = Path.Combine(componentDirectory, "sql", "dml");

            if (!Directory.Exists(sqlDirectory))
            {
                sqlDirectory = Path.Combine(componentDirectory,context.OpaRulebaseYear, "sql", "dml");
            }

            if (!Directory.Exists(sqlDirectory))
            {
                return;
            }

            using (var transientConnection = GetOpenTransientConnection(context))
            {
                try
                {
                    var sqlFiles = GetOrderedSqlFiles(sqlDirectory)
                        .Where(x => Regex.IsMatch(x, @"(?i)^.*PreRun.*\.sql$"));
                    foreach (var sqlFile in sqlFiles)
                    {
                        _logger.Debug($"Found script {sqlFile}");

                        var sql = File.ReadAllText(sqlFile);

                        _logger.Debug($"Executing {sqlFile}");
                        ExecuteSqlScript(sql, transientConnection, context);
                    }
                }
                finally
                {
                    transientConnection.Close();
                }
            }
        }

        protected virtual string[] GetOrderedSqlFiles(string sqlDirectory)
        {
            return Directory.GetFiles(sqlDirectory)
                .Select(x => new { Path = x, Weight = GetSqlFileWeight(x) })
                .OrderBy(x => x.Weight)
                .ThenBy(x => x.Path)
                .Select(x => x.Path)
                .ToArray();
        }
        private int GetSqlFileWeight(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            if (Regex.IsMatch(fileName, @".*tables.*", RegexOptions.IgnoreCase))
            {
                return 100;
            }
            if (Regex.IsMatch(fileName, @".*functions.*", RegexOptions.IgnoreCase))
            {
                return 200;
            }
            if (Regex.IsMatch(fileName, @".*views.*", RegexOptions.IgnoreCase))
            {
                return 300;
            }
            if (Regex.IsMatch(fileName, @".*sprocs.*", RegexOptions.IgnoreCase))
            {
                return 400;
            }
            return int.MaxValue;
        }

        private LateBoundTaskProxy GetExecutionProxy(AppDomain executionDomain)
        {
            var proxyType = typeof(LateBoundTaskProxy);
            var proxyInstance = (LateBoundTaskProxy)executionDomain.CreateInstanceAndUnwrap(proxyType.Assembly.FullName, proxyType.FullName);

            proxyInstance.LoadExternalTask(_assemblyName, _typeName);

            return proxyInstance;
        }
        private ProxyContext GetProxyContext(TestStackContext context)
        {
            var properties = new Dictionary<string, string>();
            foreach (var key in context.Properties.Keys)
            {
                properties.Add(key, context.Properties[key]);
            }
            return new ProxyContext
            {
                Properties = properties
            };
        }
    }
}
