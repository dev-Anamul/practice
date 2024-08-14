using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LeepsTreatmentMethodRepository : Repository<LeepsTreatmentMethod>, ILeepsTreatmentMethodRepository
    {
        public LeepsTreatmentMethodRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LeepsTreatmentMethod>> GetLeepsTreatmentMethod()
        {
            try
            {
                return await QueryAsync(x => x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LeepsTreatmentMethod> GetLeepsTreatmentMethodByKey(int key)
        {
            return await FirstOrDefaultAsync(x => x.Oid == key && x.IsDeleted == false);
        }
    }
}
