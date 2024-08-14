using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

/*
 * Created by   : Rezwana
 * Date created : 13.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IComplicationRepository interface.
    /// </summary>
    public class ComplicationRepository : Repository<Complication>, IComplicationRepository
    {
        public ComplicationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a complication by key.
        /// </summary>
        /// <param name="key">Primary key of the table Complications.</param>
        /// <returns>Returns a complication if the key is matched.</returns>
        public async Task<Complication> GetComplicationByKey(Guid key)
        {
            try
            {
                var complication = await context.Complications.AsNoTracking().FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);

                if (complication != null)
                {
                    complication.ClinicianName = await context.UserAccounts.Where(x => x.Oid == complication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    complication.FacilityName = await context.Facilities.Where(x => x.Oid == complication.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    complication.EncounterDate = await context.Encounters.Where(x => x.Oid == complication.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return complication;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of complications.
        /// </summary>
        /// <returns>Returns a list of all complications.</returns>
        public async Task<IEnumerable<Complication>> GetComplications()
        {
            try
            {
                return await context.Complications.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        complication => complication.EncounterId,
                        encounter => encounter.Oid,
                        (complication, encounter) => new Complication
                        {
                            // Properties from Complication
                            InteractionId = complication.InteractionId,
                            OtherComplications = complication.OtherComplications,
                            Severity = complication.Severity,
                            VMMCServiceId = complication.VMMCServiceId,

                            // Properties from EncounterBaseModel
                            EncounterId = complication.EncounterId,
                            EncounterType = complication.EncounterType,
                            CreatedIn = complication.CreatedIn,
                            DateCreated = complication.DateCreated,
                            CreatedBy = complication.CreatedBy,
                            ModifiedIn = complication.ModifiedIn,
                            DateModified = complication.DateModified,
                            ModifiedBy = complication.ModifiedBy,
                            IsDeleted = complication.IsDeleted,
                            IsSynced = complication.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == complication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == complication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a complication by VMMCID.
        /// </summary>
        /// <param name="VMMCServiceID"></param>
        /// <returns>Returns a complication if the VMMCID is matched.</returns>
        public async Task<IEnumerable<Complication>> GetComplicationByVMMCService(Guid VMMCServiceID)
        {
            try
            {
                return await context.Complications.AsNoTracking().Where(x => x.IsDeleted == false && x.VMMCServiceId == VMMCServiceID)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       complication => complication.EncounterId,
                       encounter => encounter.Oid,
                       (complication, encounter) => new Complication
                       {
                           // Properties from Complication
                           InteractionId = complication.InteractionId,
                           OtherComplications = complication.OtherComplications,
                           Severity = complication.Severity,
                           VMMCServiceId = complication.VMMCServiceId,


                           // Properties from EncounterBaseModel
                           EncounterId = complication.EncounterId,
                           EncounterType = complication.EncounterType,
                           CreatedIn = complication.CreatedIn,
                           DateCreated = complication.DateCreated,
                           CreatedBy = complication.CreatedBy,
                           ModifiedIn = complication.ModifiedIn,
                           DateModified = complication.DateModified,
                           ModifiedBy = complication.ModifiedBy,
                           IsDeleted = complication.IsDeleted,
                           IsSynced = complication.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == complication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == complication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       })
                   .OrderByDescending(x => x.EncounterDate)
                   .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a complication by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a complication if the EncounterID is matched.</returns>
        public async Task<IEnumerable<Complication>> GetComplicationByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.Complications.AsNoTracking().Where(x => x.IsDeleted == false && x.EncounterId == EncounterID)
                  .Join(
                      context.Encounters.AsNoTracking(),
                      complication => complication.EncounterId,
                      encounter => encounter.Oid,
                      (complication, encounter) => new Complication
                      {
                          // Properties from Complication
                          InteractionId = complication.InteractionId,
                          OtherComplications = complication.OtherComplications,
                          Severity = complication.Severity,
                          VMMCServiceId = complication.VMMCServiceId,

                          // Properties from EncounterBaseModel
                          EncounterId = complication.EncounterId,
                          EncounterType = complication.EncounterType,
                          CreatedIn = complication.CreatedIn,
                          DateCreated = complication.DateCreated,
                          CreatedBy = complication.CreatedBy,
                          ModifiedIn = complication.ModifiedIn,
                          DateModified = complication.DateModified,
                          ModifiedBy = complication.ModifiedBy,
                          IsDeleted = complication.IsDeleted,
                          IsSynced = complication.IsSynced,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == complication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == complication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      })
                  .OrderByDescending(x => x.EncounterDate)
                  .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}