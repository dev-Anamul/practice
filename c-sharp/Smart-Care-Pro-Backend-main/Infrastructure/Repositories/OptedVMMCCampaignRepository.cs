using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class OptedVMMCCampaignRepository : Repository<OptedVMMCCampaign>, IOptedVMMCCampaignRepository
    {
        public OptedVMMCCampaignRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get OptedVMMCCampaign by InteractionId.
        /// </summary>
        /// <param name="interactionId">Primary key of the table OptedVMMCCampaign.</param>
        /// <returns>Returns a OptedVMMCCampaign if the InteractionId is matched.</returns>
        public async Task<OptedVMMCCampaign> GetOptedVMMCCampaignByKey(Guid interactionId)
        {
            try
            {
                var optedVMMCCampaign = await FirstOrDefaultAsync(p => p.InteractionId == interactionId && p.IsDeleted == false);

                if (optedVMMCCampaign != null)
                    optedVMMCCampaign.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return optedVMMCCampaign;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a get OptedVMMCCampaign by VMMCCampaignId.
        /// </summary>
        /// <param name="vmmcCampaignId">Foreign key, Primary key of the table VMMCCampaign.</param>
        /// <returns>Returns a OptedVMMCCampaign if the VMMCCampaignId is matched.</returns>
        public async Task<OptedVMMCCampaign> GetOptedVMMCCampaignByVMMCCampaignId(int vmmcCampaignId)
        {
            try
            {
                var optedVMMCCampaign = await FirstOrDefaultAsync(p => p.VMMCCampaignId == vmmcCampaignId && p.IsDeleted == false);

                if (optedVMMCCampaign != null)
                    optedVMMCCampaign.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return optedVMMCCampaign;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to a get OptedVMMCCampaign by VMMCServiceId.
        /// </summary>
        /// <param name="vmmcServiceId">Foreign key, Primary key of the table VMMCService.</param>
        /// <returns>Returns a OptedVMMCCampaign if the VMMCServiceId is matched.</returns>     
        public async Task<IEnumerable<OptedVMMCCampaign>> GetVMMCCampaignByVMMCService(Guid vmmcServiceId)
        {
            try
            {
                return await context.OptedVMMCCampaigns.AsNoTracking().Where(p => p.IsDeleted == false && p.VMMCServiceId == vmmcServiceId)
       .Join(
           context.Encounters.AsNoTracking(),
           optedVMMCCampaign => optedVMMCCampaign.EncounterId,
           encounter => encounter.Oid,
           (optedVMMCCampaign, encounter) => new OptedVMMCCampaign
           {
               EncounterId = optedVMMCCampaign.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               IsDeleted = optedVMMCCampaign.IsDeleted,
               InteractionId = optedVMMCCampaign.InteractionId,
               EncounterType = optedVMMCCampaign.EncounterType,
               DateModified = optedVMMCCampaign.DateModified,
               DateCreated = optedVMMCCampaign.DateCreated,
               CreatedBy = optedVMMCCampaign.CreatedBy,
               CreatedIn = optedVMMCCampaign.CreatedIn,
               IsSynced = optedVMMCCampaign.IsSynced,
               ModifiedBy = optedVMMCCampaign.ModifiedBy,
               ModifiedIn = optedVMMCCampaign.ModifiedIn,
               VMMCCampaignId = optedVMMCCampaign.VMMCCampaignId,
               VMMCServiceId = optedVMMCCampaign.VMMCServiceId

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of OptedVMMCCampaigns.
        /// </summary>
        /// <returns>Returns a list of all OptedVMMCCampaigns.</returns>
        public async Task<IEnumerable<OptedVMMCCampaign>> GetOptedVMMCCampaigns()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
