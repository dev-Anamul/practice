using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IGeneralDrugDefinationRepository : IRepository<GeneralDrugDefinition>
    {
        public Task<GeneralDrugDefinition> GetGeneralDrugDefinitionByKey(int key);

        public Task<IEnumerable<GeneralDrugDefinition>> GetGeneralDrugDefinition();

        public Task<GeneralDrugDefinition> GetGeneralDrugDefinitionByName(string generalDrugDefinition);
    }
}
