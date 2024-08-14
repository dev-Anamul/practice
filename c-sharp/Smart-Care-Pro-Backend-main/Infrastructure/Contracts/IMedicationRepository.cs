using Domain.Dto;
using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMedicationRepository : IRepository<Medication>
    {
        /// <summary>
        /// The method is used to get a GeneralMedication by key.
        /// </summary>
        /// <param name="key">Primary key of the table GeneralMedication.</param>
        /// <returns>Returns a GeneralMedication if the key is matched.</returns>
        public Task<Medication> GetGeneralMedicationByKey(Guid key);

        public Task<MedicationListDto> GetGeneralDispenseDetailByKey(Guid key);

        public Task<IEnumerable<MedicationDto>> GetGeneralMedicationByPrescription(Guid prescriptionId);

        /// <summary>
        /// The method is used to get the list of GeneralMedication  .
        /// </summary>
        /// <returns>Returns a list of all GeneralMedication  .</returns>
        public Task<IEnumerable<Medication>> GetGeneralMedication(Guid clientId);

        public Task<IEnumerable<Medication>> GetGeneralMedication();

        public Task<IEnumerable<MedicationDto>> GetGeneralMedicationByClient(Guid clientId);
        public Task<IEnumerable<MedicationDto>> GetGeneralMedicationByClient(Guid clientId, int page, int pageSize);
        public int GetGeneralMedicationByClientTotalCount(Guid clientID);

        public Task<MedicationDto> GetLatestGeneralMedicationByClient(Guid clientId);

        /// <summary>
        /// The method is used to get a GeneralMedication by PrescriptionId.
        /// </summary>
        /// <param name="prescriptionId">Primary key of the table GeneralMedication.</param>
        /// <returns>Returns a GeneralMedication if the PrescriptionId is matched.</returns>
        public Task<IEnumerable<Medication>> GetGeneralMedicationByPrescriptionId(Guid prescriptionId);

        public Task<Medication> GetGeneralMedicationByInteractionId(Guid interactionId);
    }
}