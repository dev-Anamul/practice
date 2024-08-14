using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ILeepsTreatmentMethodRepository : IRepository<LeepsTreatmentMethod>
    {
        public Task<IEnumerable<LeepsTreatmentMethod>> GetLeepsTreatmentMethod();
        /// <summary>
        /// The method is used to get a LeepsTreatmentMethod by key.
        /// </summary>
        /// <param name="key">Primary key of the table LeepsTreatmentMethod.</param>
        /// <returns>Returns a LeepsTreatmentMethod if the key is matched.</returns>
        public Task<LeepsTreatmentMethod> GetLeepsTreatmentMethodByKey(int key);
    }
}
