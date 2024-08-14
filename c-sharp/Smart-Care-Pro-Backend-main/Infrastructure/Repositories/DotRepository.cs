using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 06.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DotRepository : Repository<Dot>, IDotRepository
    {
        public DotRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get Dot by key.
        /// </summary>
        /// <param name="key">Primary key of the table Dots.</param>
        /// <returns>Returns a Dot if the key is matched.</returns>
        public async Task<Dot> GetDotByKey(Guid key)
        {
            try
            {
                var dot = await LoadWithChildAsync<Dot>(b => b.InteractionId == key && b.IsDeleted == false, p => p.DotCalendars);

                if (dot != null)
                {
                    dot.ClinicianName = await context.UserAccounts.Where(x => x.Oid == dot.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    dot.FacilityName = await context.Facilities.Where(x => x.Oid == dot.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    dot.EncounterDate = await context.Encounters.Where(x => x.Oid == dot.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return dot;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Dots.
        /// </summary>
        /// <returns>Returns a list of all Dots.</returns>
        public async Task<IEnumerable<Dot>> GetDots()
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
        /// The method is used to Get dot by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a dots by EncounterID.</returns>
        public async Task<IEnumerable<Dot>> GetDotByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.Dots.Where(p => p.IsDeleted == false && p.EncounterId == EncounterID).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
             dot => dot.EncounterId,
             encounter => encounter.Oid,
             (dot, encounter) => new Dot
             {
                 EncounterId = dot.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 CreatedBy = dot.CreatedBy,
                 CreatedIn = dot.CreatedIn,
                 DateCreated = dot.DateCreated,
                 DateModified = dot.DateModified,
                 DiseaseSite = dot.DiseaseSite,
                 DotEndDate = dot.DotEndDate,
                 DotPlan = dot.DotPlan,
                 DotStartDate = dot.DotStartDate,
                 EncounterType = dot.EncounterType,
                 InteractionId = dot.InteractionId,
                 IsDeleted = dot.IsDeleted,
                 IsSynced = dot.IsSynced,
                 MDRDRRegimen = dot.MDRDRRegimen,
                 MDRDRRegimenGroup = dot.MDRDRRegimenGroup,
                 ModifiedBy = dot.ModifiedBy,
                 ModifiedIn = dot.ModifiedIn,
                 Phase = dot.Phase,
                 Remarks = dot.Remarks,
                 SusceptiblePTType = dot.SusceptiblePTType,
                 TBServiceId = dot.TBServiceId,
                 TBSusceptibleRegimen = dot.TBSusceptibleRegimen,
                 TBType = dot.TBType,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == dot.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == dot.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

             }).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to Get dot by tbServiceId.
        /// </summary>
        /// <param name="TBServiceId"></param>
        /// <returns>Returns a dots by tbSericeId.</returns>
        //public async Task<IEnumerable<Dot>> GetDotByTBService(Guid TBServiceId)
        public async Task<Dot> GetDotByTBService(Guid tbserviceid)
        {
            try
            {
                var dot = await FirstOrDefaultAsync(c => c.IsDeleted == false && c.TBServiceId == tbserviceid);
             
                if (dot == null)
                {
                    dot.ClinicianName = await context.UserAccounts.Where(x => x.Oid == dot.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    dot.FacilityName = await context.Facilities.Where(x => x.Oid == dot.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    dot.EncounterDate = await context.Encounters.Where(x => x.Oid == dot.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return dot;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}