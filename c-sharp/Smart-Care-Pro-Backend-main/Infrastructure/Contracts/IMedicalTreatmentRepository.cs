using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMedicalTreatmentRepository : IRepository<MedicalTreatment>
    {
        /// <summary>
        /// The method is used to get a MedicalTreatment by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalTreatments.</param>
        /// <returns>Returns a MedicalTreatment if the key is matched.</returns>
        public Task<MedicalTreatment> GetMedicalTreatmentByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of MedicalTreatments.
        /// </summary>
        /// <returns>Returns a list of all MedicalTreatments.</returns>
        public Task<IEnumerable<MedicalTreatment>> GetMedicalTreatments();

        /// <summary>
        /// The method is used to get the list of MedicalTreatment by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all MedicalTreatment by DeliveryId.</returns>
        public Task<IEnumerable<MedicalTreatment>> GetMedicalTreatmentByDelivery(Guid deliveryId);

        /// <summary>
        /// The method is used to get the list of MedicalTreatment by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all MedicalTreatment by EncounterId.</returns>
        public Task<IEnumerable<MedicalTreatment>> GetMedicalTreatmentByEncounter(Guid encounterId);
    }
}