using Domain.Entities;

/*
 * Created by   : Tariqul Islam
 * Date created : 12-03-2023
 * Modified by  : Sayem
 * Last modified: 14-03-2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="key">Primary key of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public Task<Prescription> GetPrescriptionByKey(Guid key);

        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="ClientId">ClientId of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public Task<IEnumerable<Prescription>> GetPrescriptionsByClientId(Guid clientId);

        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="ClientId">ClientId of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public Task<IEnumerable<Prescription>> GetPrescriptionsForDispenseByClientId(Guid clientId);

        /// <summary>
        /// The method is used to get a Prescription by today's date.
        /// </summary>
        /// <param>All of today of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public Task<IEnumerable<Prescription>> GetPrescriptionsByToday();
        public Task<IEnumerable<Prescription>> GetPharmacyDashBoard(int FacilityId, int skip, int take, string PatientName, string PreCriptionDateSearch);
        public int GetPharmacyDashBoardTotalCount(int FacilityId, string PatientName, string PreCriptionDateSearch);
        public int GetPharmacyDashBoardDispensationTotalCount(int FacilityId, string PatientName, string PreCriptionDateSearch);
        /// <summary>
        /// The method is used to get the list of Prescription  .
        /// </summary>
        /// <returns>Returns a list of all Prescription  .</returns>
        public Task<IEnumerable<Prescription>> GetPrescription();
    }
}
