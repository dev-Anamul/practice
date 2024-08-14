using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IMedicineBrandRepository : IRepository<MedicineBrand>
    {
        public Task<MedicineBrand> GetMedicineBrandByKey(int key);

        public Task<IEnumerable<MedicineBrand>> GetMedicineBrand();

        public Task<MedicineBrand> GetMedicineBrandByName(string medicineBrand);

    }
}
