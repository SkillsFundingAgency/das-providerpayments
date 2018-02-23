using System;
using System.Collections.Generic;
using System.Linq;

namespace ProviderPayments.TestStack.Core
{
    public static class AppDomainProvider
    {
        private static readonly Dictionary<string, AppDomain> _appDomains = new Dictionary<string, AppDomain>();

        public static AppDomain GetAppDomain(string executablesDirectory)
        {
            if (_appDomains.Keys.All(x => x != executablesDirectory))
            {

                var info = new AppDomainSetup
                {
                    ApplicationBase = executablesDirectory
                };
                var evidence = AppDomain.CurrentDomain.Evidence;
                var name = $"{DateTime.Now.Ticks}";
                _appDomains[executablesDirectory] = AppDomain.CreateDomain(name, evidence, info);
            }

            return _appDomains[executablesDirectory];
        }
    }
}
