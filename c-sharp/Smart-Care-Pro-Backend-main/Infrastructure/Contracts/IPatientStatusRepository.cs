using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Infrastructure.Contracts
{
    public interface IPatientStatusRepository : IRepository<PatientStatus>
    {
        /// <summary>
        /// The method is used to get a patientStatus by key.
        /// </summary>
        /// <param name="key">Primary key of the table patientStatuses.</param>
        /// <returns>Returns a patientStatus if the key is matched.</returns>
        public Task<PatientStatus> GetPatientStatusByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of patientStatuses.
        /// </summary>
        /// <returns>Returns a list of all patientStatuses.</returns>
        public Task<IEnumerable<PatientStatus>> PatientStatuses();
        public Task<IEnumerable<PatientStatus>> PatientStatuses(int page, int pageSize);
        public int PatientStatusesTotalCount();

        /// <summary>
        /// The method is used to get a Patient Status by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Patient status if the ClientID is matched.</returns>
        public Task<IEnumerable<PatientStatus>> GetPatientStatusbyArtRegisterId(Guid artRegisterId);
        public Task<IEnumerable<PatientStatus>> GetPatientStatusbyArtRegisterId(Guid artRegisterId, int page, int pageSize);
        public int GetPatientStatusbyArtRegisterIdTotalCount(Guid artRegisterId);
    }
}
