using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IStoppingReasonRepository : IRepository<StoppingReason>
    {
        public Task<StoppingReason> GetStoppingReasonByKey(int key);

        public Task<IEnumerable<StoppingReason>> GetStoppingReason();

        public Task<StoppingReason> GetStoppingReasonByName(string stoppingReason);
    }
}
