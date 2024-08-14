using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

namespace Infrastructure.Repositories
{
    public class DrugAdherenceRepository : Repository<DrugAdherence>, IDrugAdherenceRepository
    {
        public DrugAdherenceRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get DrugAdherence by key.
        /// </summary>
        /// <param name="key">Primary key of the table DrugAdherences.</param>
        /// <returns>Returns a DrugAdherence if the key is matched.</returns>
        public async Task<DrugAdherence> GetDrugAdherenceByKey(Guid key)
        {
            try
            {
                var drugAdherence = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (drugAdherence != null)
                {
                    drugAdherence.ClinicianName = await context.UserAccounts.Where(x => x.Oid == drugAdherence.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    drugAdherence.FacilityName = await context.Facilities.Where(x => x.Oid == drugAdherence.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    drugAdherence.EncounterDate = await context.Encounters.Where(x => x.Oid == drugAdherence.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return drugAdherence;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get a DrugAdherence by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a DrugAdherence if the ClientID is matched.</returns>
        public async Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByClient(Guid ClientID)
        {
            try
            {
                return await context.DrugAdherences.Where(p => p.IsDeleted == false && p.ClientId == ClientID).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        drugAdherences => drugAdherences.EncounterId,
        encounter => encounter.Oid,
        (drugAdherences, encounter) => new DrugAdherence
        {
            EncounterId = drugAdherences.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = drugAdherences.ClientId,
            CreatedBy = drugAdherences.CreatedBy,
            DateCreated = drugAdherences.DateCreated,
            DateModified = drugAdherences.DateModified,
            DosesMissed = drugAdherences.DosesMissed,
            CreatedIn = drugAdherences.CreatedIn,
            EncounterType = drugAdherences.EncounterType,
            HaveTroubleTakingPills = drugAdherences.HaveTroubleTakingPills,
            InteractionId = drugAdherences.InteractionId,
            IsDeleted = drugAdherences.IsDeleted,
            IsPatientComplainedOnMedication = drugAdherences.IsPatientComplainedOnMedication,
            IsSynced = drugAdherences.IsSynced,
            IsTakingMedications = drugAdherences.IsTakingMedications,
            ModifiedBy = drugAdherences.ModifiedBy,
            ModifiedIn = drugAdherences.ModifiedIn,
            Note = drugAdherences.Note,
            ReasonForMissing = drugAdherences.ReasonForMissing,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == drugAdherences.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == drugAdherences.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var drugAdherenceAsQuerable = context.DrugAdherences.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        drugAdherences => drugAdherences.EncounterId,
        encounter => encounter.Oid,
        (drugAdherences, encounter) => new DrugAdherence
        {
            EncounterId = drugAdherences.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            ClientId = drugAdherences.ClientId,
            CreatedBy = drugAdherences.CreatedBy,
            DateCreated = drugAdherences.DateCreated,
            DateModified = drugAdherences.DateModified,
            DosesMissed = drugAdherences.DosesMissed,
            CreatedIn = drugAdherences.CreatedIn,
            EncounterType = drugAdherences.EncounterType,
            HaveTroubleTakingPills = drugAdherences.HaveTroubleTakingPills,
            InteractionId = drugAdherences.InteractionId,
            IsDeleted = drugAdherences.IsDeleted,
            IsPatientComplainedOnMedication = drugAdherences.IsPatientComplainedOnMedication,
            IsSynced = drugAdherences.IsSynced,
            IsTakingMedications = drugAdherences.IsTakingMedications,
            ModifiedBy = drugAdherences.ModifiedBy,
            ModifiedIn = drugAdherences.ModifiedIn,
            Note = drugAdherences.Note,
            ReasonForMissing = drugAdherences.ReasonForMissing,
            ClinicianName = context.UserAccounts.Where(x => x.Oid == drugAdherences.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
            FacilityName = context.Facilities.Where(x => x.Oid == drugAdherences.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


        }).AsQueryable();

                if (encounterType == null)
                    return await drugAdherenceAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await drugAdherenceAsQuerable.Where(p => p.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetDrugAdherenceByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.DrugAdherences.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.DrugAdherences.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of drug adherence.
        /// </summary>
        /// <returns>Returns a list of all drug adherence.</returns>
        public async Task<IEnumerable<DrugAdherence>> GetDrugAdherences()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DrugAdherence>> GetDrugAdherenceByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.DrugAdherences.Where(p => p.IsDeleted == false && p.EncounterId == EncounterID).AsNoTracking()
        .Join(
            context.Encounters.AsNoTracking(),
            drugAdherences => drugAdherences.EncounterId,
            encounter => encounter.Oid,
            (drugAdherences, encounter) => new DrugAdherence
            {
                EncounterId = drugAdherences.EncounterId,
                EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                ClientId = drugAdherences.ClientId,
                CreatedBy = drugAdherences.CreatedBy,
                DateCreated = drugAdherences.DateCreated,
                DateModified = drugAdherences.DateModified,
                DosesMissed = drugAdherences.DosesMissed,
                CreatedIn = drugAdherences.CreatedIn,
                EncounterType = drugAdherences.EncounterType,
                HaveTroubleTakingPills = drugAdherences.HaveTroubleTakingPills,
                InteractionId = drugAdherences.InteractionId,
                IsDeleted = drugAdherences.IsDeleted,
                IsPatientComplainedOnMedication = drugAdherences.IsPatientComplainedOnMedication,
                IsSynced = drugAdherences.IsSynced,
                IsTakingMedications = drugAdherences.IsTakingMedications,
                ModifiedBy = drugAdherences.ModifiedBy,
                ModifiedIn = drugAdherences.ModifiedIn,
                Note = drugAdherences.Note,
                ReasonForMissing = drugAdherences.ReasonForMissing,
                ClinicianName = context.UserAccounts.Where(x => x.Oid == drugAdherences.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                FacilityName = context.Facilities.Where(x => x.Oid == drugAdherences.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

            }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}