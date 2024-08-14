using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPatientTypeRepository : IRepository<DFZPatientType>
    {
        /// <summary>
        /// The method is used to get a PatientType by PatientType name.
        /// </summary>
        /// <param name="patientTypeName">Name of a PatientType.</param>
        /// <returns>Returns a facility if the facility name is matched.</returns>
        public Task<DFZPatientType> GetPatientTypeByName(string patientTypeName);

        /// <summary>
        /// The method is used to get a PatientType by key.
        /// </summary>
        /// <param name="key">Primary key of the table PatientTypes.</param>
        /// <returns>Returns a PatientType if the key is matched.</returns>
        public Task<DFZPatientType> GetPatientTypeByKey(int key);

        /// <summary>
        /// The method is used to get the PatientTypes by armedForceId.
        /// </summary>
        /// <param name="armedForceId">armedForceId of the table PatientTypes.</param>
        /// <returns>Returns a PatientType if the armedForceId is matched.</returns>
        public Task<IEnumerable<DFZPatientType>> GetPatientTypeByArmedForce(int armedForceId);

        /// <summary>
        /// The method is used to get the list of PatientTypes.
        /// </summary>
        /// <returns>Returns a list of all PatientTypes.</returns>
        public Task<IEnumerable<DFZPatientType>> GetPatientTypes();
    }
}