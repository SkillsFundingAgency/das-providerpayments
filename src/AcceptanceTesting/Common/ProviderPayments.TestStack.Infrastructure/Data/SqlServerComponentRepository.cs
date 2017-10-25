using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerComponentRepository : SqlServerRepository, IComponentRepository
    {
        public SqlServerComponentRepository()
            : base("TransientConnectionString")
        {
        }


        public async Task<IEnumerable<ComponentEntity>> All()
        {
            return await Query<ComponentEntity>(@"SELECT ComponentType [Type]
                                                        ,VersionNumber [Version]
                                                        ,ArchiveData [Data]
                                                  FROM TestStack.Component");
        }

        public async Task UpdateComponent(int componentType, string version, byte[] componentBuffer)
        {
            await Execute("DELETE FROM TestStack.Component WHERE ComponentType = @componentType",
                new { componentType });

            await Execute(@"INSERT INTO TestStack.Component
                            (ComponentType, VersionNumber, ArchiveData)
                            VALUES
                            (@componentType, @version, @componentBuffer)",
                            new
                            {
                                componentType,
                                version,
                                componentBuffer
                            });
        }
    }
}
