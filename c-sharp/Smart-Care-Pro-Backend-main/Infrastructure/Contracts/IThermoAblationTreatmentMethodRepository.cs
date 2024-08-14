using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts 
{
    public interface IThermoAblationTreatmentMethodRepository : IRepository<ThermoAblationTreatmentMethod>
    {
        public Task<IEnumerable<ThermoAblationTreatmentMethod>> GetThermoAblationTreatmentMethod();
        /// <summary>
        /// The method is used to get a ThermoAblationTreatmentMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table ThermoAblationTreatmentMethod.</param>
        /// <returns>Returns a ThermoAblationTreatmentMethod if the key is matched.</returns>
        public Task<ThermoAblationTreatmentMethod> GetThermoAblationTreatmentMethodByKey(int key);
    }
}
