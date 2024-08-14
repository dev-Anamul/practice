using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedConstitutionalSymptomRepository : IRepository<IdentifiedConstitutionalSymptom>
   {
      /// <summary>
      /// The method is used to get a identifiedConstitutionalSymptom by key.
      /// </summary>
      /// <param name="key">Primary key of the table identifiedConstitutionalSymptom.</param>
      /// <returns>Returns a identifiedConstitutionalSymptom if the key is matched.</returns>
      public Task<IdentifiedConstitutionalSymptom> GetIdentifiedConstitutionalSymptomByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of identifiedConstitutionalSymptoms.
      /// </summary>
      /// <returns>Returns a list of all identifiedConstitutionalSymptoms.</returns>
      public Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptoms();

      /// <summary>
      /// The method is used to get a Immunization Records by encounterId.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a Immunization Records if the encounterId is matched.</returns>
      public Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByClientID(Guid clientId);
      public Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetIdentifiedConstitutionalSymptomsByClientIDTotalCount(Guid clientID, EncounterType? encounterType);

        /// <summary>
        /// The method is used to get a IdentifiedConstitutionalSymptom by encounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a IdentifiedConstitutionalSymptom if the encounterId is matched.</returns>
        public Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByEncounterId(Guid encounterId);
   }
}