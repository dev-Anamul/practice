using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
* Created by   : Bella
* Date created : 25.12.2022
* Modified by  : Bella
* Last modified: 14.08.2023
* Reviewed by  :
* Date reviewed:
*/
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of ICountryRepository interface.
    /// </summary>
    public class IdentifiedAllergyRepository : Repository<IdentifiedAllergy>, IIdentifiedAllergyRepository
    {
        public IdentifiedAllergyRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a identified allergy by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedAllergies.</param>
        /// <returns>Returns a identified allergy if the key is matched.</returns>
        public async Task<IdentifiedAllergy> GetIdentifiedAllergyByKey(Guid key)
        {
            try
            {
                var identifiedAllergy = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedAllergy != null)
                {
                    identifiedAllergy.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedAllergy.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedAllergy.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedAllergy.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedAllergy.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedAllergy.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedAllergy;
                // return await FirstOrDefaultAsync(i => i.InteractionId == key && i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of identified allergies.
        /// </summary>
        /// <returns>Returns a list of all identified allergies.</returns>
        public async Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergies()
        {
            try
            {
                return await LoadListWithChildAsync<IdentifiedAllergy>(i => i.IsDeleted == false, a => a.Allergy, a => a.AllergicDrug);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a identified allergy by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a identified allergy if the ClientID is matched.</returns>
        public async Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClient(Guid clientId)
        {
            try
            {
                return await context.IdentifiedAllergies.Include(a => a.Allergy).Include(x => x.AllergicDrug).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedAllergy => identifiedAllergy.EncounterId,
         encounter => encounter.Oid,
         (identifiedAllergy, encounter) => new IdentifiedAllergy
         {
             EncounterId = identifiedAllergy.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             AllergicDrug = identifiedAllergy.AllergicDrug,
             AllergicDrugId = identifiedAllergy.AllergicDrugId,
             Allergy = identifiedAllergy.Allergy,
             AllergyId = identifiedAllergy.AllergyId,
             DateCreated = identifiedAllergy.DateCreated,
             CreatedIn = identifiedAllergy.CreatedIn,
             ClientId = identifiedAllergy.ClientId,
             CreatedBy = identifiedAllergy.CreatedBy,
             DateModified = identifiedAllergy.DateModified,
             EncounterType = identifiedAllergy.EncounterType,
             InteractionId = identifiedAllergy.InteractionId,
             IsDeleted = identifiedAllergy.IsDeleted,
             IsSynced = identifiedAllergy.IsSynced,
             ModifiedBy = identifiedAllergy.ModifiedBy,
             ModifiedIn = identifiedAllergy.ModifiedIn,
             Severity = identifiedAllergy.Severity,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedAllergy.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedAllergy.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.IdentifiedAllergies.Include(a => a.Allergy).Include(x => x.AllergicDrug).Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedAllergy => identifiedAllergy.EncounterId,
         encounter => encounter.Oid,
         (identifiedAllergy, encounter) => new IdentifiedAllergy
         {
             EncounterId = identifiedAllergy.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             AllergicDrug = identifiedAllergy.AllergicDrug,
             AllergicDrugId = identifiedAllergy.AllergicDrugId,
             Allergy = identifiedAllergy.Allergy,
             AllergyId = identifiedAllergy.AllergyId,
             DateCreated = identifiedAllergy.DateCreated,
             CreatedIn = identifiedAllergy.CreatedIn,
             ClientId = identifiedAllergy.ClientId,
             CreatedBy = identifiedAllergy.CreatedBy,
             DateModified = identifiedAllergy.DateModified,
             EncounterType = identifiedAllergy.EncounterType,
             InteractionId = identifiedAllergy.InteractionId,
             IsDeleted = identifiedAllergy.IsDeleted,
             IsSynced = identifiedAllergy.IsSynced,
             ModifiedBy = identifiedAllergy.ModifiedBy,
             ModifiedIn = identifiedAllergy.ModifiedIn,
             Severity = identifiedAllergy.Severity,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedAllergy.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedAllergy.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedAllergyAsQuerable = context.IdentifiedAllergies.Include(a => a.Allergy).Include(x => x.AllergicDrug).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedAllergy => identifiedAllergy.EncounterId,
         encounter => encounter.Oid,
         (identifiedAllergy, encounter) => new IdentifiedAllergy
         {
             EncounterId = identifiedAllergy.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             AllergicDrug = identifiedAllergy.AllergicDrug,
             AllergicDrugId = identifiedAllergy.AllergicDrugId,
             Allergy = identifiedAllergy.Allergy,
             AllergyId = identifiedAllergy.AllergyId,
             DateCreated = encounter.DateCreated,
             CreatedIn = identifiedAllergy.CreatedIn,
             ClientId = identifiedAllergy.ClientId,
             CreatedBy = identifiedAllergy.CreatedBy,
             DateModified = identifiedAllergy.DateModified,
             EncounterType = identifiedAllergy.EncounterType,
             InteractionId = identifiedAllergy.InteractionId,
             IsDeleted = identifiedAllergy.IsDeleted,
             IsSynced = identifiedAllergy.IsSynced,
             ModifiedBy = identifiedAllergy.ModifiedBy,
             ModifiedIn = identifiedAllergy.ModifiedIn,
             Severity = identifiedAllergy.Severity,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedAllergy.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedAllergy.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


         }).AsQueryable();

                if (encounterType == null)
                    return await identifiedAllergyAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedAllergyAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetIdentifiedAllergyByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedAllergies.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedAllergies.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a identified allergy by encounterId.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a identified allergy if the encounterId is matched.</returns>
        public async Task<IEnumerable<IdentifiedAllergy>> GetIdentifiedAllergyByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedAllergies.Include(a => a.Allergy).Include(x => x.AllergicDrug).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedAllergy => identifiedAllergy.EncounterId,
         encounter => encounter.Oid,
         (identifiedAllergy, encounter) => new IdentifiedAllergy
         {
             EncounterId = identifiedAllergy.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             AllergicDrug = identifiedAllergy.AllergicDrug,
             AllergicDrugId = identifiedAllergy.AllergicDrugId,
             Allergy = identifiedAllergy.Allergy,
             AllergyId = identifiedAllergy.AllergyId,
             DateCreated = encounter.DateCreated,
             CreatedIn = identifiedAllergy.CreatedIn,
             ClientId = identifiedAllergy.ClientId,
             CreatedBy = identifiedAllergy.CreatedBy,
             DateModified = identifiedAllergy.DateModified,
             EncounterType = identifiedAllergy.EncounterType,
             InteractionId = identifiedAllergy.InteractionId,
             IsDeleted = identifiedAllergy.IsDeleted,
             IsSynced = identifiedAllergy.IsSynced,
             ModifiedBy = identifiedAllergy.ModifiedBy,
             ModifiedIn = identifiedAllergy.ModifiedIn,
             Severity = identifiedAllergy.Severity,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedAllergy.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedAllergy.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


         }).OrderByDescending(x => x.EncounterDate).ToListAsync();
                //return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}