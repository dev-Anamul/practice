using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PreScreeningVisitRepository : Repository<PreScreeningVisit>, IPreScreeningVisitRepository
    {
        public PreScreeningVisitRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisit()
        {
            try
            {
                return await context.PreScreeningVisits.AsNoTracking().Where(p => p.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        preScreeningVisits => preScreeningVisits.EncounterId,
                        encounter => encounter.Oid,
                        (preScreeningVisits, encounter) => new PreScreeningVisit
                        {
                            InteractionId = preScreeningVisits.InteractionId,
                            IsPostAntibiotic = preScreeningVisits.IsPostAntibiotic,
                            CreatedBy = preScreeningVisits.CreatedBy,
                            CreatedIn = preScreeningVisits.CreatedIn,
                            DateCreated = preScreeningVisits.DateCreated,
                            DateModified = preScreeningVisits.DateModified,
                            EncounterId = preScreeningVisits.EncounterId,
                            EncounterType = preScreeningVisits.EncounterType,
                            IsDeleted = preScreeningVisits.IsDeleted,
                            IsPostBiopsy = preScreeningVisits.IsPostBiopsy,
                            IsPostLeep = preScreeningVisits.IsPostLeep,
                            IsPostThermoAblation = preScreeningVisits.IsPostThermoAblation,
                            IsRoutineFollowup = preScreeningVisits.IsRoutineFollowup,
                            ClientId = preScreeningVisits.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            IsSynced = preScreeningVisits.IsSynced,
                            ModifiedBy = preScreeningVisits.ModifiedBy,
                            ModifiedIn = preScreeningVisits.ModifiedIn,
                            Other = preScreeningVisits.Other,
                            PreScreeningVisitType = preScreeningVisits.PreScreeningVisitType,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == preScreeningVisits.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == preScreeningVisits.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PreScreeningVisit> GetPreScreeningVisitByKey(Guid key)
        {
            try
            {
                PreScreeningVisit preScreeningVisit = await context.PreScreeningVisits.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (preScreeningVisit is not null)
                {
                    preScreeningVisit.EncounterDate = await context.Encounters.Where(x => x.Oid == preScreeningVisit.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    preScreeningVisit.ClinicianName = await context.UserAccounts.Where(x => x.Oid == preScreeningVisit.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    preScreeningVisit.FacilityName = await context.Facilities.Where(x => x.Oid == preScreeningVisit.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return preScreeningVisit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PreScreeningVisit by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a  PreScreeningVisitbyClienId if the clientId is matched.</returns>

        public async Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisitbyClienId(Guid clientId)
        {
            try
            {
                return await context.PreScreeningVisits.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        preScreeningVisits => preScreeningVisits.EncounterId,
                        encounter => encounter.Oid,
                        (preScreeningVisits, encounter) => new PreScreeningVisit
                        {
                            InteractionId = preScreeningVisits.InteractionId,
                            IsPostAntibiotic = preScreeningVisits.IsPostAntibiotic,
                            CreatedBy = preScreeningVisits.CreatedBy,
                            CreatedIn = preScreeningVisits.CreatedIn,
                            DateCreated = preScreeningVisits.DateCreated,
                            DateModified = preScreeningVisits.DateModified,
                            EncounterId = preScreeningVisits.EncounterId,
                            EncounterType = preScreeningVisits.EncounterType,
                            IsDeleted = preScreeningVisits.IsDeleted,
                            IsPostBiopsy = preScreeningVisits.IsPostBiopsy,
                            IsPostLeep = preScreeningVisits.IsPostLeep,
                            IsPostThermoAblation = preScreeningVisits.IsPostThermoAblation,
                            IsRoutineFollowup = preScreeningVisits.IsRoutineFollowup,
                            ClientId = preScreeningVisits.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            IsSynced = preScreeningVisits.IsSynced,
                            ModifiedBy = preScreeningVisits.ModifiedBy,
                            ModifiedIn = preScreeningVisits.ModifiedIn,
                            Other = preScreeningVisits.Other,
                            PreScreeningVisitType = preScreeningVisits.PreScreeningVisitType,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == preScreeningVisits.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == preScreeningVisits.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                            IsOnART = context.Clients.Where(a => a.Oid == preScreeningVisits.ClientId).Select(a => a.IsOnART).FirstOrDefault()
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PreScreeningVisit>> GetPreScreeningVisitsByEncounter(Guid encounterId)
        {
            try
            {
                return await context.PreScreeningVisits.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          preScreeningVisits => preScreeningVisits.EncounterId,
                          encounter => encounter.Oid,
                          (preScreeningVisits, encounter) => new PreScreeningVisit
                          {
                              InteractionId = preScreeningVisits.InteractionId,
                              IsPostAntibiotic = preScreeningVisits.IsPostAntibiotic,
                              CreatedBy = preScreeningVisits.CreatedBy,
                              CreatedIn = preScreeningVisits.CreatedIn,
                              DateCreated = preScreeningVisits.DateCreated,
                              DateModified = preScreeningVisits.DateModified,
                              EncounterId = preScreeningVisits.EncounterId,
                              EncounterType = preScreeningVisits.EncounterType,
                              IsDeleted = preScreeningVisits.IsDeleted,
                              IsPostBiopsy = preScreeningVisits.IsPostBiopsy,
                              IsPostLeep = preScreeningVisits.IsPostLeep,
                              IsPostThermoAblation = preScreeningVisits.IsPostThermoAblation,
                              IsRoutineFollowup = preScreeningVisits.IsRoutineFollowup,
                              ClientId = preScreeningVisits.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              IsSynced = preScreeningVisits.IsSynced,
                              ModifiedBy = preScreeningVisits.ModifiedBy,
                              ModifiedIn = preScreeningVisits.ModifiedIn,
                              Other = preScreeningVisits.Other,
                              PreScreeningVisitType = preScreeningVisits.PreScreeningVisitType,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == preScreeningVisits.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == preScreeningVisits.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                          }).OrderByDescending(b => b.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}