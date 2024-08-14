using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 13.04.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IIdentifyComplicationRepository interface.
    /// </summary>
    public class IdentifiedPregnancyConfirmationRepository : Repository<IdentifiedPregnancyConfirmation>, IIdentifiedPregnancyConfirmationRepository
    {
        public IdentifiedPregnancyConfirmationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPregnancyConfirmation by EncounterID.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounters.</param>
        /// <returns>Returns a list of all IdentifiedPregnancyConfirmation by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPregnancyConfirmation>> GetIdentifiedPregnancyConfirmationByEncounter(Guid encounterId)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedPregnancyConfirmation by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPregnancyConfirmations.</param>
        /// <returns>Returns a IdentifiedPregnancyConfirmation if the key is matched.</returns>
        public async Task<IdentifiedPregnancyConfirmation> GetIdentifiedPregnancyConfirmationByKey(Guid key)
        {
            try
            {
                var identifiedPregnancyConfirmation = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPregnancyConfirmation != null)
                {
                    identifiedPregnancyConfirmation.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPregnancyConfirmation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPregnancyConfirmation.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPregnancyConfirmation.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPregnancyConfirmation.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPregnancyConfirmation.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedPregnancyConfirmation;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPregnancyConfirmations.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPregnancyConfirmation.</returns>
        public async Task<IEnumerable<IdentifiedPregnancyConfirmation>> GetIdentifiedPregnancyConfirmations()
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