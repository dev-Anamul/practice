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
    public class LeepsRepository : Repository<Leeps>, ILeepsRepository
    {
        public LeepsRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Leeps>> GetLeeps()
        {
            try
            {
                return await context.Leeps.Include(x => x.LeepsTreatmentMethod).AsNoTracking().Where(p => p.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        leeps => leeps.EncounterId,
                        encounter => encounter.Oid,
                        (leeps, encounter) => new Leeps
                        {
                            InteractionId = leeps.InteractionId,
                            CreatedBy = leeps.CreatedBy,
                            EncounterId = leeps.EncounterId,
                            CreatedIn = leeps.CreatedIn,
                            DateCreated = leeps.DateCreated,
                            DateModified = leeps.DateModified,
                            ClientId = leeps.ClientId,
                            EncounterType = leeps.EncounterType,
                            EstimatedTimeForProcedure = leeps.EstimatedTimeForProcedure,
                            IsClientCounseled = leeps.IsClientCounseled,
                            IsConsentObtained = leeps.IsConsentObtained,
                            IsDeleted = leeps.IsDeleted,
                            IsSynced = leeps.IsSynced,
                            ModifiedBy = leeps.ModifiedBy,
                            ModifiedIn = leeps.ModifiedIn,
                            IsLesionToThickForAblation = leeps.IsLesionToThickForAblation,
                            LeepsTreatmentMethod = leeps.LeepsTreatmentMethod,
                            LeepTreatmentMethodId = leeps.LeepTreatmentMethodId,
                            AssesmentComment = leeps.AssesmentComment,
                            IsAtypicalVessels = leeps.IsAtypicalVessels,
                            IsLesionCovers = leeps.IsLesionCovers,
                            IsLesionExtendsIntoTheCervicalOs = leeps.IsLesionExtendsIntoTheCervicalOs,
                            IsLesionTooLargeForThermoAblation = leeps.IsLesionTooLargeForThermoAblation,
                            IsPoly = leeps.IsPoly,
                            IsPostLEEPLesion = leeps.IsPostLEEPLesion,
                            IsPunctationsOrMoasicm = leeps.IsPunctationsOrMoasicm,
                            IsQueryICC = leeps.IsQueryICC,
                            ProcedureComment = leeps.ProcedureComment,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == leeps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == leeps.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                            
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
        public async Task<Leeps> GetLeepsByKey(Guid key)
        {
            try
            {
                Leeps leeps = await context.Leeps.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (leeps is not null)
                {
                    leeps.EncounterDate = await context.Encounters.Where(x => x.Oid == leeps.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    leeps.ClinicianName = await context.UserAccounts.Where(x => x.Oid == leeps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    leeps.FacilityName = await context.Facilities.Where(x => x.Oid == leeps.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return leeps;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a thermoAblation by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a  leepsbyClienId if the clientId is matched.</returns>

        public async Task<IEnumerable<Leeps>> GetLeepsbyClienId(Guid clientId)
        {
            try
            {
                return await context.Leeps.Include(x => x.LeepsTreatmentMethod).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        leeps => leeps.EncounterId,
                        encounter => encounter.Oid,
                        (leeps, encounter) => new Leeps
                        {
                            InteractionId = leeps.InteractionId,
                            CreatedBy = leeps.CreatedBy,
                            EncounterId = leeps.EncounterId,
                            CreatedIn = leeps.CreatedIn,
                            DateCreated = leeps.DateCreated,
                            DateModified = leeps.DateModified,
                            ClientId = leeps.ClientId,
                            EncounterType = leeps.EncounterType,
                            EstimatedTimeForProcedure = leeps.EstimatedTimeForProcedure,
                            IsClientCounseled = leeps.IsClientCounseled,
                            IsConsentObtained = leeps.IsConsentObtained,
                            IsDeleted = leeps.IsDeleted,
                            IsSynced = leeps.IsSynced,
                            ModifiedBy = leeps.ModifiedBy,
                            ModifiedIn = leeps.ModifiedIn,
                            IsLesionToThickForAblation = leeps.IsLesionToThickForAblation,
                            LeepsTreatmentMethod = leeps.LeepsTreatmentMethod,
                            LeepTreatmentMethodId = leeps.LeepTreatmentMethodId,
                            AssesmentComment = leeps.AssesmentComment,
                            IsAtypicalVessels = leeps.IsAtypicalVessels,
                            IsLesionCovers = leeps.IsLesionCovers,
                            IsLesionExtendsIntoTheCervicalOs = leeps.IsLesionExtendsIntoTheCervicalOs,
                            IsLesionTooLargeForThermoAblation = leeps.IsLesionTooLargeForThermoAblation,
                            IsPoly = leeps.IsPoly,
                            IsPostLEEPLesion = leeps.IsPostLEEPLesion,
                            IsPunctationsOrMoasicm = leeps.IsPunctationsOrMoasicm,
                            IsQueryICC = leeps.IsQueryICC,
                            ProcedureComment = leeps.ProcedureComment,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == leeps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == leeps.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// </summary>
        /// <returns>Returns a list of all leeps by EncounterID.</returns>
        public async Task<IEnumerable<Leeps>> GetLeepsByEncounter(Guid encounterId)
        {
            try
            {
                return await context.Leeps.Include(x => x.LeepsTreatmentMethod).AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          leeps => leeps.EncounterId,
                          encounter => encounter.Oid,
                          (leeps, encounter) => new Leeps
                          {
                              InteractionId = leeps.InteractionId,
                              CreatedBy = leeps.CreatedBy,
                              EncounterId = leeps.EncounterId,
                              CreatedIn = leeps.CreatedIn,
                              DateCreated = leeps.DateCreated,
                              DateModified = leeps.DateModified,
                              ClientId = leeps.ClientId,
                              EncounterType = leeps.EncounterType,
                              EstimatedTimeForProcedure = leeps.EstimatedTimeForProcedure,
                              IsClientCounseled = leeps.IsClientCounseled,
                              IsConsentObtained = leeps.IsConsentObtained,
                              IsDeleted = leeps.IsDeleted,
                              IsSynced = leeps.IsSynced,
                              ModifiedBy = leeps.ModifiedBy,
                              ModifiedIn = leeps.ModifiedIn,
                              IsLesionToThickForAblation = leeps.IsLesionToThickForAblation,
                              LeepsTreatmentMethod = leeps.LeepsTreatmentMethod,
                              LeepTreatmentMethodId = leeps.LeepTreatmentMethodId,
                              AssesmentComment = leeps.AssesmentComment,
                              IsAtypicalVessels = leeps.IsAtypicalVessels,
                              IsLesionCovers = leeps.IsLesionCovers,
                              IsLesionExtendsIntoTheCervicalOs = leeps.IsLesionExtendsIntoTheCervicalOs,
                              IsLesionTooLargeForThermoAblation = leeps.IsLesionTooLargeForThermoAblation,
                              IsPoly = leeps.IsPoly,
                              IsPostLEEPLesion = leeps.IsPostLEEPLesion,
                              IsPunctationsOrMoasicm = leeps.IsPunctationsOrMoasicm,
                              IsQueryICC = leeps.IsQueryICC,
                              ProcedureComment = leeps.ProcedureComment,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == leeps.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == leeps.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
