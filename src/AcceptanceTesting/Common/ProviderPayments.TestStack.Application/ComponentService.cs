using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ionic.Zip;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface IComponentService
    {
        Task<Component> GetComponent(ComponentType type);
        Task<IEnumerable<Component>> GetComponents();
        Task UpdateComponent(byte[] zipBuffer);
    }

    public class ComponentService : IComponentService
    {
        private const string ComponentNamePattern = @"^([A-Za-z]{1,})-([0-9\.]{1,})$";

        private readonly IComponentRepository _componentRepository;
        private readonly IMapper _mapper;

        public ComponentService(IComponentRepository componentRepository, IMapper mapper)
        {
            _componentRepository = componentRepository;
            _mapper = mapper;
        }


        public async Task<Component> GetComponent(ComponentType type)
        {
            return (await GetComponents()).SingleOrDefault(c => c.Type == type);
        }

        public async Task<IEnumerable<Component>> GetComponents()
        {
            var entities = await _componentRepository.All();
            var components = _mapper.Map<IEnumerable<Component>>(entities);
            return components.OrderBy(c => c.Type.ToString());
        }

        public async Task UpdateComponent(byte[] zipBuffer)
        {
            var componentIdentification = IdentifyComponent(zipBuffer);
            if (componentIdentification == null)
            {
                throw new System.Exception("Unknown component");
            }

            await _componentRepository.UpdateComponent((int)componentIdentification.Type, componentIdentification.Version, zipBuffer);
        }


        private ComponentIdentification IdentifyComponent(byte[] zipBuffer)
        {
            string topLevelDirectoryName;
            using (var ms = new MemoryStream(zipBuffer))
            {
                var zipFile = ZipFile.Read(ms);
                topLevelDirectoryName = zipFile.Entries.First(e => e.IsDirectory).FileName.Split('/')[0];
            }

            var match = Regex.Match(topLevelDirectoryName, ComponentNamePattern);
            if (!match.Success)
            {
                return null;
            }

            var componentName = match.Groups[1].Value;
            var version = match.Groups[2].Value;
            switch (componentName.ToLower())
            {
                case "datalocksubmission":
                    return new ComponentIdentification(ComponentType.DataLockSubmission, version);
                case "levycalculator":
                    return new ComponentIdentification(ComponentType.LevyCalculator, version);
                case "earningscalculator":
                    return new ComponentIdentification(ComponentType.EarningsCalculator, version);
                case "paymentsdue":
                    return new ComponentIdentification(ComponentType.PaymentsDue, version);
                case "coinvestedpaymentscalculator":
                    return new ComponentIdentification(ComponentType.CoInvestedPaymentsCalculator, version);
                case "referenceaccounts":
                    return new ComponentIdentification(ComponentType.ReferenceAccounts, version);
                case "referencecommitments":
                    return new ComponentIdentification(ComponentType.ReferenceCommitments, version);
                case "datalockperiodend":
                    return new ComponentIdentification(ComponentType.DataLockPeriodEnd, version);
                case "periodendscripts":
                    return new ComponentIdentification(ComponentType.PeriodEndScripts, version);
            }

            return null;
        }


        private class ComponentIdentification
        {
            public ComponentIdentification(ComponentType type, string version)
            {
                Type = type;
                Version = version;
            }
            public ComponentType Type { get; set; }
            public string Version { get; set; }
        }
    }
}
