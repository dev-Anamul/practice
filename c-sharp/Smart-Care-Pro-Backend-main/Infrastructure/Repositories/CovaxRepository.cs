using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Infrastructure.Repositories
{
    public class CovaxRepository : Repository<Covax>, ICovaxRepository
    {
        public CovaxRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table Covaxes.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public async Task<Covax> GetCovaxByKey(Guid key)
        {
            try
            {
                var covax = await context.Covaxes.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (covax != null)
                {
                    covax.ClinicianName = await context.UserAccounts.Where(x => x.Oid == covax.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    covax.FacilityName = await context.Facilities.Where(x => x.Oid == covax.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    covax.EncounterDate = await context.Encounters.Where(x => x.Oid == covax.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
                return covax;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a birth history by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a birth history if the ClientID is matched.</returns>
        public async Task<IEnumerable<Covax>> GetCovaxByClient(Guid clientId)
        {
            try
            {
                var covax = context.Covaxes
                    .Where(b => b.IsDeleted == false && b.ClientId == clientId)
                    .AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        covax => covax.EncounterId,
                        encounter => encounter.Oid,
                        (covax, encounter) => new Covax
                        {
                            InteractionId = covax.InteractionId,
                            CovaxNumber = covax.CovaxNumber,
                            WasCovaxOffered = covax.WasCovaxOffered,
                            DoesClientGetVaccinatedToday = covax.DoesClientGetVaccinatedToday,
                            ReasonClientRefusedForVaccination = covax.ReasonClientRefusedForVaccination,
                            OtherReasonClientRefusedForVaccination = covax.OtherReasonClientRefusedForVaccination,
                            IsPregnantOrLactating = covax.IsPregnantOrLactating,
                            HasCancer = covax.HasCancer,
                            HasDiabetes = covax.HasDiabetes,
                            HasHeartDisease = covax.HasHeartDisease,
                            HasHyperTension = covax.HasHyperTension,
                            HasHIV = covax.HasHIV,
                            OtherComorbidities = covax.OtherComorbidities,
                            ClientId = covax.ClientId,
                            //Client = covax.Client,
                            //CovaxRecords = covax.CovaxRecords,

                            //Properties from EncounterBaseModel
                            EncounterId = covax.EncounterId,
                            EncounterType = covax.EncounterType,
                            CreatedIn = covax.CreatedIn,
                            DateCreated = covax.DateCreated,
                            CreatedBy = covax.CreatedBy,
                            ModifiedIn = covax.ModifiedIn,
                            DateModified = covax.DateModified,
                            ModifiedBy = covax.ModifiedBy,
                            IsDeleted = covax.IsDeleted,
                            IsSynced = covax.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == covax.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == covax.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }).AsQueryable();

                return await covax.OrderByDescending(e => e.EncounterDate).ToListAsync();
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
        public async Task<IEnumerable<Covax>> GetCovaxes()
        {
            try
            {

                return await context.Covaxes.AsNoTracking().Where(b => b.IsDeleted == false)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       covax => covax.EncounterId,
               encounter => encounter.Oid,
                       (covax, encounter) => new Covax
                       {
                           InteractionId = covax.InteractionId,
                           CovaxNumber = covax.CovaxNumber,
                           WasCovaxOffered = covax.WasCovaxOffered,
                           DoesClientGetVaccinatedToday = covax.DoesClientGetVaccinatedToday,
                           ReasonClientRefusedForVaccination = covax.ReasonClientRefusedForVaccination,
                           OtherReasonClientRefusedForVaccination = covax.OtherReasonClientRefusedForVaccination,
                           IsPregnantOrLactating = covax.IsPregnantOrLactating,
                           HasCancer = covax.HasCancer,
                           HasDiabetes = covax.HasDiabetes,
                           HasHeartDisease = covax.HasHeartDisease,
                           HasHyperTension = covax.HasHyperTension,
                           HasHIV = covax.HasHIV,
                           OtherComorbidities = covax.OtherComorbidities,
                           ClientId = covax.ClientId,
                           Client = covax.Client,
                           CovaxRecords = covax.CovaxRecords,

                           // Properties from EncounterBaseModel
                           EncounterId = covax.EncounterId,
                           EncounterType = covax.EncounterType,
                           CreatedIn = covax.CreatedIn,
                           DateCreated = covax.DateCreated,
                           CreatedBy = covax.CreatedBy,
                           ModifiedIn = covax.ModifiedIn,
                           DateModified = covax.DateModified,
                           ModifiedBy = covax.ModifiedBy,
                           IsDeleted = covax.IsDeleted,
                           IsSynced = covax.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated
                       }
                   )
                   .OrderByDescending(x => x.EncounterDate)
                   .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Covax>> GetCovaxByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.Covaxes.AsNoTracking().Where(b => b.IsDeleted == false && b.EncounterId == EncounterID)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       covax => covax.EncounterId,
               encounter => encounter.Oid,
                       (covax, encounter) => new Covax
                       {
                           InteractionId = covax.InteractionId,
                           CovaxNumber = covax.CovaxNumber,
                           WasCovaxOffered = covax.WasCovaxOffered,
                           DoesClientGetVaccinatedToday = covax.DoesClientGetVaccinatedToday,
                           ReasonClientRefusedForVaccination = covax.ReasonClientRefusedForVaccination,
                           OtherReasonClientRefusedForVaccination = covax.OtherReasonClientRefusedForVaccination,
                           IsPregnantOrLactating = covax.IsPregnantOrLactating,
                           HasCancer = covax.HasCancer,
                           HasDiabetes = covax.HasDiabetes,
                           HasHeartDisease = covax.HasHeartDisease,
                           HasHyperTension = covax.HasHyperTension,
                           HasHIV = covax.HasHIV,
                           OtherComorbidities = covax.OtherComorbidities,
                           ClientId = covax.ClientId,
                           Client = covax.Client,
                           CovaxRecords = covax.CovaxRecords,

                           // Properties from EncounterBaseModel
                           EncounterId = covax.EncounterId,
                           EncounterType = covax.EncounterType,
                           CreatedIn = covax.CreatedIn,
                           DateCreated = covax.DateCreated,
                           CreatedBy = covax.CreatedBy,
                           ModifiedIn = covax.ModifiedIn,
                           DateModified = covax.DateModified,
                           ModifiedBy = covax.ModifiedBy,
                           IsDeleted = covax.IsDeleted,
                           IsSynced = covax.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated
                       }
                   )
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