using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IIdentifiedReferralReasonRepository : IRepository<IdentifiedReferralReason>
   {
      /// <summary>
      /// The method is used to get a referral module by key.
      /// </summary>
      /// <param name="key">Primary key of the table IdentifiedReferralReasons.</param>
      /// <returns>Returns a referral module if the key is matched.</returns>
      public Task<IdentifiedReferralReason> GetIdentifiedReferralReasonByKey(Guid key);

      /// <summary>
      /// The method is used to get the list of referrals.
      /// </summary>
      /// <returns>Returns a list of all referrals.</returns>
      public Task<IEnumerable<IdentifiedReferralReason>> GetIdentifiedReferralReasons();

      /// <summary>
      /// The method is used to get a referral module by OPD visit.
      /// </summary>
      /// <param name="encounterId"></param>
      /// <returns>Returns a referral module if the Encounter is matched.</returns>
      public Task<IEnumerable<IdentifiedReferralReason>> GetIdentifiedReferralReasonByEncounter(Guid encounterId);
   }
}