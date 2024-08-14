using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 16.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class AdverseEventRepository : Repository<AdverseEvent>, IAdverseEventRepository
    {
        public AdverseEventRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by key.
        /// </summary>
        /// <param name="key">Primary key of the table AdverseEvents.</param>
        /// <returns>Returns a birth record if the key is matched.</returns>
        public async Task<AdverseEvent> GetAdverseEventByKey(Guid key)
        {
            try
            {
                AdverseEvent adverseEvent = await context.AdverseEvents.AsNoTracking().FirstOrDefaultAsync(x => x.InteractionId == key && x.IsDeleted == false);

                if (adverseEvent is not null)
                {
                    adverseEvent.EncounterDate = await context.Encounters.Where(x => x.Oid == adverseEvent.EncounterId).Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();
                    adverseEvent.ClinicianName = context.UserAccounts.Where(x => x.Oid == adverseEvent.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    adverseEvent.FacilityName = context.Facilities.Where(x => x.Oid == adverseEvent.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                }

                return adverseEvent;

                ///return await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<AdverseEvent>> GetAdverseEvents()
        {
            try
            {
                return await context.AdverseEvents.AsNoTracking()
                   .Join(
                    context.Encounters.AsNoTracking(),
                    adverseEvent => adverseEvent.EncounterId,
                    encounter => encounter.Oid,
                    (adverseEvent, encounter) => new AdverseEvent
                    {
                        AEFIDate = adverseEvent.AEFIDate,
                        AllergicReaction = adverseEvent.AllergicReaction,
                        BodyAches = adverseEvent.BodyAches,
                        OtherAdverseEvent = adverseEvent.OtherAdverseEvent,
                        OtherAEFI = adverseEvent.OtherAEFI,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        CreatedBy = adverseEvent.CreatedBy,
                        CreatedIn = adverseEvent.CreatedIn,
                        DateCreated = adverseEvent.DateCreated,
                        DateModified = adverseEvent.DateModified,
                        EncounterType = adverseEvent.EncounterType,
                        Fever = adverseEvent.Fever,
                        ImmunizationId = adverseEvent.ImmunizationId,
                        ImmunizationRecord = adverseEvent.ImmunizationRecord,
                        EncounterId = adverseEvent.EncounterId,
                        InteractionId = adverseEvent.InteractionId,
                        IsDeleted = adverseEvent.IsDeleted,
                        IsSynced = adverseEvent.IsSynced,
                        Joint = adverseEvent.Joint,
                        Malaise = adverseEvent.Malaise,
                        ModifiedBy = adverseEvent.ModifiedBy,
                        ModifiedIn = adverseEvent.ModifiedIn,
                        Swelling = adverseEvent.Swelling,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == adverseEvent.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == adverseEvent.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                    })
                   .OrderByDescending(x => x.EncounterDate)
                   .ToListAsync();
                /// return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by FluidID.
        /// </summary>
        /// <param name="immunizationId"></param>
        /// <returns>Returns a birth record if the FluidID is matched.</returns>
        public async Task<IEnumerable<AdverseEvent>> GetAdverseEventByImmunization(Guid immunizationId)
        {
            try
            {

                return await context.AdverseEvents.Where(b => b.IsDeleted == false && b.ImmunizationId == immunizationId).AsNoTracking()
                    .Join(
                     context.Encounters.AsNoTracking(),
                     adverseEvent => adverseEvent.EncounterId,
                     encounter => encounter.Oid,
                     (adverseEvent, encounter) => new AdverseEvent
                     {
                         AEFIDate = adverseEvent.AEFIDate,
                         AllergicReaction = adverseEvent.AllergicReaction,
                         BodyAches = adverseEvent.BodyAches,
                         OtherAdverseEvent = adverseEvent.OtherAdverseEvent,
                         OtherAEFI = adverseEvent.OtherAEFI,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         CreatedBy = adverseEvent.CreatedBy,
                         CreatedIn = adverseEvent.CreatedIn,
                         DateCreated = adverseEvent.DateCreated,
                         DateModified = adverseEvent.DateModified,
                         EncounterType = adverseEvent.EncounterType,
                         Fever = adverseEvent.Fever,
                         ImmunizationId = adverseEvent.ImmunizationId,
                         ImmunizationRecord = adverseEvent.ImmunizationRecord,
                         EncounterId = adverseEvent.EncounterId,
                         InteractionId = adverseEvent.InteractionId,
                         IsDeleted = adverseEvent.IsDeleted,
                         IsSynced = adverseEvent.IsSynced,
                         Joint = adverseEvent.Joint,
                         Malaise = adverseEvent.Malaise,
                         ModifiedBy = adverseEvent.ModifiedBy,
                         ModifiedIn = adverseEvent.ModifiedIn,
                         Swelling = adverseEvent.Swelling,
                         ClinicianName = context.UserAccounts.Where(x => x.Oid == adverseEvent.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                         FacilityName = context.Facilities.Where(x => x.Oid == adverseEvent.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                     })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();



                ///return await QueryAsync(b => b.IsDeleted == false && b.ImmunizationId == immunizationId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a birth record by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a birth record if the OPD EncounterID is matched.</returns>
        public async Task<IEnumerable<AdverseEvent>> GetAdverseEventByEncounter(Guid encounterId)
        {
            try
            {
                return await context.AdverseEvents.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == encounterId)
                    .Join(
                    context.Encounters.AsNoTracking(),
                    adverseEvent => adverseEvent.EncounterId,
                    encounter => encounter.Oid,
                    (adverseEvent, encounter) => new AdverseEvent
                    {
                        AEFIDate = adverseEvent.AEFIDate,
                        AllergicReaction = adverseEvent.AllergicReaction,
                        BodyAches = adverseEvent.BodyAches,
                        OtherAdverseEvent = adverseEvent.OtherAdverseEvent,
                        OtherAEFI = adverseEvent.OtherAEFI,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        CreatedBy = adverseEvent.CreatedBy,
                        CreatedIn = adverseEvent.CreatedIn,
                        DateCreated = adverseEvent.DateCreated,
                        DateModified = adverseEvent.DateModified,
                        EncounterType = adverseEvent.EncounterType,
                        Fever = adverseEvent.Fever,
                        ImmunizationId = adverseEvent.ImmunizationId,
                        ImmunizationRecord = adverseEvent.ImmunizationRecord,
                        EncounterId = adverseEvent.EncounterId,
                        InteractionId = adverseEvent.InteractionId,
                        IsDeleted = adverseEvent.IsDeleted,
                        IsSynced = adverseEvent.IsSynced,
                        Joint = adverseEvent.Joint,
                        Malaise = adverseEvent.Malaise,
                        ModifiedBy = adverseEvent.ModifiedBy,
                        ModifiedIn = adverseEvent.ModifiedIn,
                        Swelling = adverseEvent.Swelling,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == adverseEvent.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == adverseEvent.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                    })

                    .OrderByDescending(x => x.EncounterDate).ToListAsync();
                ///return await context.AdverseEvents.Where(c => c.IsDeleted == false && c.EncounterId == encounterId).OrderByDescending(f => f.DateCreated).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}