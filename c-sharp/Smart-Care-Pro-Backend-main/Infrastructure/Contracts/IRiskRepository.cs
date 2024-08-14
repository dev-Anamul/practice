using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IRiskRepository : IRepository<Risks>
    {
        public Task<Risks> GetRiskByKey(int key);

        public Task<IEnumerable<Risks>> GetRisk();

        public Task<Risks> GetRiskByName(string risk);
    }
}
