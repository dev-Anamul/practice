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
    public class ThermoAblationRepository : Repository<ThermoAblation>, IThermoAblationRepository
    {
        public ThermoAblationRepository(DataContext context) : base(context)
        {
        }
        public async Task<ThermoAblation> GetThermoAblationByKey(Guid key)
        {
            try
            {
                ThermoAblation thermoAblation = await context.ThermoAblations.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (thermoAblation is not null)
                {
                    thermoAblation.EncounterDate = await context.Encounters.Where(x => x.Oid == thermoAblation.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    thermoAblation.ClinicianName = await context.UserAccounts.Where(x => x.Oid == thermoAblation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    thermoAblation.FacilityName = await context.Facilities.Where(x => x.Oid == thermoAblation.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return thermoAblation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ThermoAblation>> GetThermoAblation()
        {
            try
            {
                return await context.ThermoAblations.Include(x => x.ThermoAblationTreatmentMethod).AsNoTracking().Where(p => p.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        thermoAblation => thermoAblation.EncounterId,
                        encounter => encounter.Oid,
                        (thermoAblation, encounter) => new ThermoAblation
                        {
                            InteractionId = thermoAblation.InteractionId,
                            CreatedBy = thermoAblation.CreatedBy,
                            EncounterId = thermoAblation.EncounterId,
                            CreatedIn = thermoAblation.CreatedIn,
                            DateCreated = thermoAblation.DateCreated,
                            DateModified = thermoAblation.DateModified,
                            ClientId = thermoAblation.ClientId,
                            EncounterType = thermoAblation.EncounterType,
                            EstimatedTimeForProcedure = thermoAblation.EstimatedTimeForProcedure,
                            IsClientCounseled = thermoAblation.IsClientCounseled,
                            IsConsentObtained = thermoAblation.IsConsentObtained,
                            IsDeleted = thermoAblation.IsDeleted,
                            IsSynced = thermoAblation.IsSynced,
                            ModifiedBy = thermoAblation.ModifiedBy,
                            ModifiedIn = thermoAblation.ModifiedIn,
                            ThermoAblationComment = thermoAblation.ThermoAblationComment,
                            ThermoAblationTreatmentMethod = thermoAblation.ThermoAblationTreatmentMethod,
                            ThermoAblationTreatmentMethodId = thermoAblation.ThermoAblationTreatmentMethodId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == thermoAblation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == thermoAblation.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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

        /// <summary>
        /// The method is used to get a thermoAblation by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a  thermoAblationbyClienId if the clientId is matched.</returns>

        public async Task<IEnumerable<ThermoAblation>> GetThermoAblationbyClienId(Guid clientId)
        {
            try
            {
                return await context.ThermoAblations.Include(x => x.ThermoAblationTreatmentMethod).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        thermoAblation => thermoAblation.EncounterId,
                        encounter => encounter.Oid,
                        (thermoAblation, encounter) => new ThermoAblation
                        {
                            InteractionId = thermoAblation.InteractionId,
                            CreatedBy = thermoAblation.CreatedBy,
                            EncounterId = thermoAblation.EncounterId,
                            CreatedIn = thermoAblation.CreatedIn,
                            DateCreated = thermoAblation.DateCreated,
                            DateModified = thermoAblation.DateModified,
                            ClientId = thermoAblation.ClientId,
                            EncounterType = thermoAblation.EncounterType,
                            EstimatedTimeForProcedure = thermoAblation.EstimatedTimeForProcedure,
                            IsClientCounseled = thermoAblation.IsClientCounseled,
                            IsConsentObtained = thermoAblation.IsConsentObtained,
                            IsDeleted = thermoAblation.IsDeleted,
                            IsSynced = thermoAblation.IsSynced,
                            ModifiedBy = thermoAblation.ModifiedBy,
                            ModifiedIn = thermoAblation.ModifiedIn,
                            ThermoAblationComment = thermoAblation.ThermoAblationComment,
                            ThermoAblationTreatmentMethod = thermoAblation.ThermoAblationTreatmentMethod,
                            ThermoAblationTreatmentMethodId = thermoAblation.ThermoAblationTreatmentMethodId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == thermoAblation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == thermoAblation.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// </summary>
        /// <returns>Returns a list of all Screening by EncounterID.</returns>
        public async Task<IEnumerable<ThermoAblation>> GetThermoAblationByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ThermoAblations.Include(x => x.ThermoAblationTreatmentMethod).AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          thermoAblation => thermoAblation.EncounterId,
                          encounter => encounter.Oid,
                          (thermoAblation, encounter) => new ThermoAblation
                          {
                              InteractionId = thermoAblation.InteractionId,
                              CreatedBy = thermoAblation.CreatedBy,
                              EncounterId = thermoAblation.EncounterId,
                              CreatedIn = thermoAblation.CreatedIn,
                              DateCreated = thermoAblation.DateCreated,
                              DateModified = thermoAblation.DateModified,
                              ClientId = thermoAblation.ClientId,
                              EncounterType = thermoAblation.EncounterType,
                              EstimatedTimeForProcedure = thermoAblation.EstimatedTimeForProcedure,
                              IsClientCounseled = thermoAblation.IsClientCounseled,
                              IsConsentObtained = thermoAblation.IsConsentObtained,
                              IsDeleted = thermoAblation.IsDeleted,
                              IsSynced = thermoAblation.IsSynced,
                              ModifiedBy = thermoAblation.ModifiedBy,
                              ModifiedIn = thermoAblation.ModifiedIn,
                              ThermoAblationComment = thermoAblation.ThermoAblationComment,
                              ThermoAblationTreatmentMethod = thermoAblation.ThermoAblationTreatmentMethod,
                              ThermoAblationTreatmentMethodId = thermoAblation.ThermoAblationTreatmentMethodId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == thermoAblation.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == thermoAblation.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

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
