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
    public class CaCXPlanRepository : Repository<CaCXPlan>, ICaCXPlanRepository
    {
        public CaCXPlanRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<CaCXPlan>> GetCaCXPlan()
        {
            try
            {
                return await context.CaCXPlans.AsNoTracking().Where(p => p.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        cacxplan => cacxplan.EncounterId,
                        encounter => encounter.Oid,
                        (cacxplan, encounter) => new CaCXPlan
                        {
                            InteractionId = cacxplan.InteractionId,
                            CreatedBy = cacxplan.CreatedBy,
                            EncounterId = cacxplan.EncounterId,
                            CreatedIn = cacxplan.CreatedIn,
                            DateCreated = cacxplan.DateCreated,
                            DateModified = cacxplan.DateModified,
                            ClientId = cacxplan.ClientId,
                            EncounterType = cacxplan.EncounterType,
                            IsAtypicalVessels = cacxplan.IsAtypicalVessels,
                            IsClientReffered = cacxplan.IsClientReffered,
                            IsDeleted = cacxplan.IsDeleted,
                            IsLesionCovers = cacxplan.IsLesionCovers,
                            IsLesionExtendsIntoTheCervicalOs = cacxplan.IsLesionExtendsIntoTheCervicalOs,
                            IsLesionTooLargeForThermoAblation = cacxplan.IsLesionTooLargeForThermoAblation,
                            IsLesionToThickForAblation = cacxplan.IsLesionToThickForAblation,
                            IsPoly = cacxplan.IsPoly,
                            IsPostLEEPLesion = cacxplan.IsPostLEEPLesion,
                            IsPunctationsOrMoasicm = cacxplan.IsPunctationsOrMoasicm,
                            IsQueryICC = cacxplan.IsQueryICC,
                            IsSynced = cacxplan.IsSynced,
                            ModifiedBy = cacxplan.ModifiedBy,
                            ModifiedIn = cacxplan.ModifiedIn,
                            Others = cacxplan.Others,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == cacxplan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == cacxplan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })

                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CaCXPlan> GetCaCXPlanByKey(Guid key)
        {
            try
            {
                var cacxplan = await LoadWithChildAsync<CaCXPlan>(b => b.InteractionId == key && b.IsDeleted == false);

                if (cacxplan is not null)
                {
                    cacxplan.EncounterDate = await context.Encounters.Where(x => x.Oid == cacxplan.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    cacxplan.ClinicianName = await context.UserAccounts.Where(x => x.Oid == cacxplan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    cacxplan.FacilityName = await context.Facilities.Where(x => x.Oid == cacxplan.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return cacxplan;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a CaCXPlan by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a  CaCXPlanbyClienId if the clientId is matched.</returns>

        public async Task<IEnumerable<CaCXPlan>> GetCaCXPlanbyClienId(Guid clientId)
        {
            try
            {
                return await context.CaCXPlans.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        cacxplan => cacxplan.EncounterId,
                        encounter => encounter.Oid,
                        (cacxplan, encounter) => new CaCXPlan
                        {
                            InteractionId = cacxplan.InteractionId,
                            CreatedBy = cacxplan.CreatedBy,
                            EncounterId = cacxplan.EncounterId,
                            CreatedIn = cacxplan.CreatedIn,
                            DateCreated = cacxplan.DateCreated,
                            DateModified = cacxplan.DateModified,
                            ClientId = cacxplan.ClientId,
                            EncounterType = cacxplan.EncounterType,
                            IsAtypicalVessels = cacxplan.IsAtypicalVessels,
                            IsClientReffered = cacxplan.IsClientReffered,
                            IsDeleted = cacxplan.IsDeleted,
                            IsLesionCovers = cacxplan.IsLesionCovers,
                            IsLesionExtendsIntoTheCervicalOs = cacxplan.IsLesionExtendsIntoTheCervicalOs,
                            IsLesionTooLargeForThermoAblation = cacxplan.IsLesionTooLargeForThermoAblation,
                            IsLesionToThickForAblation = cacxplan.IsLesionToThickForAblation,
                            IsPoly = cacxplan.IsPoly,
                            IsPostLEEPLesion = cacxplan.IsPostLEEPLesion,
                            IsPunctationsOrMoasicm = cacxplan.IsPunctationsOrMoasicm,
                            IsQueryICC = cacxplan.IsQueryICC,
                            IsSynced = cacxplan.IsSynced,
                            ModifiedBy = cacxplan.ModifiedBy,
                            ModifiedIn = cacxplan.ModifiedIn,
                            Others = cacxplan.Others,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == cacxplan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == cacxplan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// </summary>
        /// <returns>Returns a list of all CaCXPlan by EncounterID.</returns>
        public async Task<IEnumerable<CaCXPlan>> GetCaCXPlanByEncounter(Guid encounterId)
        {
            try
            {
                return await context.CaCXPlans.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          cacxplan => cacxplan.EncounterId,
                          encounter => encounter.Oid,
                          (cacxplan, encounter) => new CaCXPlan
                          {
                              InteractionId = cacxplan.InteractionId,
                              CreatedBy = cacxplan.CreatedBy,
                              EncounterId = cacxplan.EncounterId,
                              CreatedIn = cacxplan.CreatedIn,
                              DateCreated = cacxplan.DateCreated,
                              DateModified = cacxplan.DateModified,
                              ClientId = cacxplan.ClientId,
                              EncounterType = cacxplan.EncounterType,
                              IsAtypicalVessels = cacxplan.IsAtypicalVessels,
                              IsClientReffered = cacxplan.IsClientReffered,
                              IsDeleted = cacxplan.IsDeleted,
                              IsLesionCovers = cacxplan.IsLesionCovers,
                              IsLesionExtendsIntoTheCervicalOs = cacxplan.IsLesionExtendsIntoTheCervicalOs,
                              IsLesionTooLargeForThermoAblation = cacxplan.IsLesionTooLargeForThermoAblation,
                              IsLesionToThickForAblation = cacxplan.IsLesionToThickForAblation,
                              IsPoly = cacxplan.IsPoly,
                              IsPostLEEPLesion = cacxplan.IsPostLEEPLesion,
                              IsPunctationsOrMoasicm = cacxplan.IsPunctationsOrMoasicm,
                              IsQueryICC = cacxplan.IsQueryICC,
                              IsSynced = cacxplan.IsSynced,
                              ModifiedBy = cacxplan.ModifiedBy,
                              ModifiedIn = cacxplan.ModifiedIn,
                              Others = cacxplan.Others,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == cacxplan.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == cacxplan.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                              
                              // Add other properties as needed
                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
