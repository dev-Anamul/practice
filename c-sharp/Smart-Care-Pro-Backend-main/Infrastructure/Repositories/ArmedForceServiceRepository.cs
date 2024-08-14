using Domain.Entities;
using Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ArmedForceServiceRepository : Repository<ArmedForceService>,IArmedForceServiceRepository
    {
        private readonly DataContext context;

        public ArmedForceServiceRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<ArmedForceService> GetArmedForceServiceByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Oid == key && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  async Task<ArmedForceService> GetArmedForceServiceByName(string description)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == description.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ArmedForceService>> GetArmedForceServiceses()
        {
            try
            {
                return await QueryAsync(a => a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
