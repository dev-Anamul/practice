using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IConstitutionalSymptomTypeRepository : IRepository<ConstitutionalSymptomType>
    {
        /// <summary>
        /// The method is used to get the list of constitutional symptom type.
        /// </summary>
        /// <returns>Returns a list of all constitutional symptom types.</returns>
        public Task<IEnumerable<ConstitutionalSymptomType>> GetConstitutionalSymptomTypes();

        /// <summary>
        /// The method is used to get a ConstitutionalSymtom by ConstitutionalSymtomID.
        /// </summary>
        /// <param name="constitutionalSymtomId"></param>
        /// <returns>Returns a ConstitutionalSymtom if the ConstitutionalSymtomID is matched.</returns>
        public Task<IEnumerable<ConstitutionalSymptomType>> GetConstitutionalSymptomTypesByConstitutionalSymtom(int constitutionalSymtomId);

        /// <summary>
        /// The method is used to get a ConstitutionalSymptomType by key.
        /// </summary>
        /// <param name="key">Primary key of the table ConstitutionalSymptomType.</param>
        /// <returns>Returns a ConstitutionalSymptomType if the key is matched.</returns>
        public Task<ConstitutionalSymptomType> GetConstitutionalSymptomTypeByKey(int key);

        /// <summary>
        /// The method is used to get an ConstitutionalSymptomType by ConstitutionalSymptomType Description.
        /// </summary>
        /// <param name="constitutionalSymptomType">Description of an ConstitutionalSymptomType.</param>
        /// <returns>Returns an ConstitutionalSymptomType if the ConstitutionalSymptomType name is matched.</returns>
        public Task<ConstitutionalSymptomType> GetConstitutionalSymptomTypeByName(string constitutionalSymptomType);
    }
}