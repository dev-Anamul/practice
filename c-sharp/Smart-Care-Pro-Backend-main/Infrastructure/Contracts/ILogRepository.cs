using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ILogRepository
    {
        public Task<Tuple<List<Log>, int>> GetAll(int pageSize, int pageNumber);
    }
}
