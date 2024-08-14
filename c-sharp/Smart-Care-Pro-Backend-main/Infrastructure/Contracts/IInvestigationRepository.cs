using Domain.Dto;
using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IInvestigationRepository : IRepository<Investigation>
   {
      /// <summary>
      /// The method is used to get a Investigation by key.
      /// </summary>
      /// <param name="key">Primary key of the table Investigations.</param>
      /// <returns>Returns a Investigation if the key is matched.</returns>
      public Task<Investigation> GetInvestigationByKey(Guid key);

      /// <summary>
      /// The method is used to get a Investigation by key
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      public Task<IEnumerable<Investigation>> GetInvestigationByEncounterId(Guid key);

      /// <summary>
      /// The method is used to get the list of Investigations.
      /// </summary>
      /// <returns>Returns a list of all Investigations.</returns>
      public Task<IEnumerable<Investigation>> GetInvestigations();

      /// <summary>
      /// The method is used to get a Investigation by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a Investigation if the ClientID is matched.</returns>
      public Task<IEnumerable<Investigation>> GetInvestigationByClient(Guid clientId);

      /// <summary>
      /// The method is used to get a InvestigationDto by ClientID.
      /// </summary>
      /// <param name="clientId"></param>
      /// <returns>Returns a InvestigationDto if the ClientID is matched.</returns>
      public Task<IEnumerable<InvestigationDto>> GetInvestigationDtoByClient(Guid clientId);
      public Task<IEnumerable<InvestigationDto>> GetInvestigationDtoByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType);
        public int GetInvestigationByClientTotalCount(Guid clientID, EncounterType? encounterType);
        /// <summary>
        /// The method is used to get a Investigation by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a investigation if the EncounterId is matched.</returns>
        public Task<IEnumerable<Investigation>> GetInvestigationByEncounter(Guid encounterId);

        /// <summary>
        /// The method is used to get a Investigation.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="PatientName"></param>
        /// <param name="InvestigationDateSearch"></param>
        /// <returns>Returns a investigation.</returns>
        public Task<IEnumerable<Investigation>> GetInvestigationDashBoard(int facilityId, int skip, int take, string PatientName, string InvestigationDateSearch);
   }
}