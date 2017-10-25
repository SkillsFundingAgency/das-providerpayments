using System;
using System.Reflection;
using CS.Common.External.Interfaces;

namespace ProviderPayments.TestStack.Engine.ExecutionProxy
{
    public class LateBoundTaskProxy : MarshalByRefObject, IExternalTask
    {
        private IExternalTask _innerTask;

        public void LoadExternalTask(string assemblyName, string typeName)
        {
            var assembly = Assembly.Load(assemblyName);
            var type = assembly.GetType(typeName);
            _innerTask = (IExternalTask)Activator.CreateInstance(type);
        }

        public void Execute(IExternalContext context)
        {
            _innerTask.Execute(context);
        }
    }
}
