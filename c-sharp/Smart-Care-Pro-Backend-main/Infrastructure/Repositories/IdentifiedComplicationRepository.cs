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
    public class IdentifiedComplicationRepository : Repository<IdentifiedComplication>, IIdentifiedComplicationRepository
    {
        public IdentifiedComplicationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an identify complication by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifyComplications.</param>
        /// <returns>Returns an identify complication if the key is matched.</returns>
        public async Task<IdentifiedComplication> GetIdentifiedComplicationByKey(Guid key)
        {
            try
            {
                var identifiedComplication = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedComplication != null)
                {
                    identifiedComplication.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedComplication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedComplication.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedComplication.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedComplication.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedComplication.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedComplication;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of identify complications.
        /// </summary>
        /// <returns>Returns a list of all identify complications.</returns>
        public async Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplications()
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an identify complication by OPD visit.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns an identify complication if the Encounter is matched.</returns>
        public async Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplicationByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedComplications.Include(c => c.Complication).Include(a => a.ComplicationType).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedComplication => identifiedComplication.EncounterId,
         encounter => encounter.Oid,
         (identifiedComplication, encounter) => new IdentifiedComplication
         {
             EncounterId = identifiedComplication.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             Complication = identifiedComplication.Complication,
             ComplicationId = identifiedComplication.ComplicationId,
             ComplicationType = identifiedComplication.ComplicationType,
             ComplicationTypeId = identifiedComplication.ComplicationTypeId,
             CreatedBy = identifiedComplication.CreatedBy,
             CreatedIn = identifiedComplication.CreatedIn,
             DateCreated = identifiedComplication.DateCreated,
             DateModified = identifiedComplication.DateModified,
             EncounterType = identifiedComplication.EncounterType,
             InteractionId = identifiedComplication.InteractionId,
             IsDeleted = identifiedComplication.IsDeleted,
             IsSynced = identifiedComplication.IsSynced,
             ModifiedBy = identifiedComplication.ModifiedBy,
             ModifiedIn = identifiedComplication.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedComplication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedComplication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an identified complication by ComplicationID.
        /// </summary>
        /// <param name="complicationId"></param>
        /// <returns>Returns an identified complication if the ComplicationID is matched.</returns>
        public async Task<IEnumerable<IdentifiedComplication>> GetIdentifiedComplicationByComplication(Guid complicationId)
        {
            try
            {
                return await context.IdentifiedComplications.Include(c => c.Complication).Include(a => a.ComplicationType).Where(p => p.IsDeleted == false && p.ComplicationId == complicationId).AsNoTracking()
       .Join(
           context.Encounters.AsNoTracking(),
           identifiedComplication => identifiedComplication.EncounterId,
           encounter => encounter.Oid,
           (identifiedComplication, encounter) => new IdentifiedComplication
           {
               EncounterId = identifiedComplication.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               Complication = identifiedComplication.Complication,
               ComplicationId = identifiedComplication.ComplicationId,
               ComplicationType = identifiedComplication.ComplicationType,
               ComplicationTypeId = identifiedComplication.ComplicationTypeId,
               CreatedBy = identifiedComplication.CreatedBy,
               CreatedIn = identifiedComplication.CreatedIn,
               DateCreated = identifiedComplication.DateCreated,
               DateModified = identifiedComplication.DateModified,
               EncounterType = identifiedComplication.EncounterType,
               InteractionId = identifiedComplication.InteractionId,
               IsDeleted = identifiedComplication.IsDeleted,
               IsSynced = identifiedComplication.IsSynced,
               ModifiedBy = identifiedComplication.ModifiedBy,
               ModifiedIn = identifiedComplication.ModifiedIn,
               ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedComplication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
               FacilityName = context.Facilities.Where(x => x.Oid == identifiedComplication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}