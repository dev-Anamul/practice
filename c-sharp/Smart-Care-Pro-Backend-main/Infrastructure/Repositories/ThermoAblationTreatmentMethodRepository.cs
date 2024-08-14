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
    public class ThermoAblationTreatmentMethodRepository : Repository<ThermoAblationTreatmentMethod>, IThermoAblationTreatmentMethodRepository
    {
        public ThermoAblationTreatmentMethodRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ThermoAblationTreatmentMethod>> GetThermoAblationTreatmentMethod()
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

        public async Task<ThermoAblationTreatmentMethod> GetThermoAblationTreatmentMethodByKey(int key)
        {
            return await FirstOrDefaultAsync(x => x.Oid == key && x.IsDeleted == false);
        }
    }
}
