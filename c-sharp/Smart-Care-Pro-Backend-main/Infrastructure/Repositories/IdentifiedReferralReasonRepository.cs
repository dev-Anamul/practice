using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedReferralReasonRepository : Repository<IdentifiedReferralReason>, IIdentifiedReferralReasonRepository
    {
        public IdentifiedReferralReasonRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get referral module by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a referral module if the key is matched.</returns>
        public async Task<IdentifiedReferralReason> GetIdentifiedReferralReasonByKey(Guid key)
        {
            try
            {
                var identifiedReferralReason = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedReferralReason != null)
                {
                    identifiedReferralReason.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedReferralReason.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedReferralReason.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedReferralReason.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedReferralReason.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedReferralReason.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedReferralReason;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of referral modules.
        /// </summary>
        /// <returns>Returns a list of all referral modules.</returns>
        public async Task<IEnumerable<IdentifiedReferralReason>> GetIdentifiedReferralReasons()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of referral modules.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a referral modules by encounterId</returns>
        public async Task<IEnumerable<IdentifiedReferralReason>> GetIdentifiedReferralReasonByEncounter(Guid encounterId)
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false && b.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}