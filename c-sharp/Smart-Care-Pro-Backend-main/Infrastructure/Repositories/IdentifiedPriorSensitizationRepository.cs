using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 19.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedPriorSensitizationRepository : Repository<IdentifiedPriorSensitization>, IIdentifiedPriorSensitizationRepository
    {
        public IdentifiedPriorSensitizationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPriorSensitization by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPriorSensitization by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizationByEncounter(Guid EncounterID)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == EncounterID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedPriorSensitization by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPriorSensitizations.</param>
        /// <returns>Returns a IdentifiedPriorSensitization if the key is matched.</returns>
        public async Task<IdentifiedPriorSensitization> GetIdentifiedPriorSensitizationByKey(Guid key)
        {
            try
            {
                var identifiedPriorSensitization = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPriorSensitization != null)
                {
                    identifiedPriorSensitization.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPriorSensitization.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPriorSensitization.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPriorSensitization.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPriorSensitization.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPriorSensitization.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
     }
                return identifiedPriorSensitization;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPriorSensitization by BloodTransfusionID.
        /// </summary>
        /// <param name="bloodTransfusionId">Primary key of the table bloodTransfusion.</param>
        /// <returns>Returns a list of all IdentifiedPriorSensitization by BloodTransfusionID.</returns>
        public async Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizationByBloodTransfusion(Guid bloodTransfusionId)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.BloodTransfusionId == bloodTransfusionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPriorSensitizations.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPriorSensitization.</returns>
        public async Task<IEnumerable<IdentifiedPriorSensitization>> GetIdentifiedPriorSensitizations()
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
    }
}