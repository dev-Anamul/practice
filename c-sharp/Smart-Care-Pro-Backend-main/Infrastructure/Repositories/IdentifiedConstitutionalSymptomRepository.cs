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
    public class IdentifiedConstitutionalSymptomRepository : Repository<IdentifiedConstitutionalSymptom>, IIdentifiedConstitutionalSymptomRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedConstitutionalSymptomRepository interface.
        /// </summary>
        public IdentifiedConstitutionalSymptomRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a identifiedConstitutionalSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedConstitutionalSymptom.</param>
        /// <returns>Returns a IdentifiedConstitutionalSymptom if the key is matched.</returns>
        public async Task<IdentifiedConstitutionalSymptom> GetIdentifiedConstitutionalSymptomByKey(Guid key)
        {
            try
            {
                var identifiedConstitutionalSymptom = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedConstitutionalSymptom != null)
                {
                    identifiedConstitutionalSymptom.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedConstitutionalSymptom.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedConstitutionalSymptom.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedConstitutionalSymptom.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedConstitutionalSymptom;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of identifiedConstitutionalSymptoms.
        /// </summary>
        /// <returns>Returns a list of all chief identifiedConstitutionalSymptom.</returns>
        public async Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptoms()
        {
            try
            {
                return await QueryAsync(u => u.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByClientID(Guid clientId)
        {
            try
            {
                return await context.IdentifiedConstitutionalSymptoms.Include(p => p.ConstitutionalSymptomType).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedConstitutionalSymptom => identifiedConstitutionalSymptom.EncounterId,
         encounter => encounter.Oid,
         (identifiedConstitutionalSymptom, encounter) => new IdentifiedConstitutionalSymptom
         {
             EncounterId = identifiedConstitutionalSymptom.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedConstitutionalSymptom.ClientId,
             ConstitutionalSymptomType = identifiedConstitutionalSymptom.ConstitutionalSymptomType,
             ConstitutionalSymptomTypeId = identifiedConstitutionalSymptom.ConstitutionalSymptomTypeId,
             CreatedBy = identifiedConstitutionalSymptom.CreatedBy,
             CreatedIn = identifiedConstitutionalSymptom.CreatedIn,
             DateCreated = identifiedConstitutionalSymptom.DateCreated,
             DateModified = identifiedConstitutionalSymptom.DateModified,
             EncounterType = identifiedConstitutionalSymptom.EncounterType,
             InteractionId = identifiedConstitutionalSymptom.InteractionId,
             IsDeleted = identifiedConstitutionalSymptom.IsDeleted,
             IsSynced = identifiedConstitutionalSymptom.IsSynced,
             ModifiedBy = identifiedConstitutionalSymptom.ModifiedBy,
             ModifiedIn = identifiedConstitutionalSymptom.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


         }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedConstitutionalSymptomAsQuerable = context.IdentifiedConstitutionalSymptoms.Include(p => p.ConstitutionalSymptomType).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedConstitutionalSymptom => identifiedConstitutionalSymptom.EncounterId,
         encounter => encounter.Oid,
         (identifiedConstitutionalSymptom, encounter) => new IdentifiedConstitutionalSymptom
         {
             EncounterId = identifiedConstitutionalSymptom.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedConstitutionalSymptom.ClientId,
             ConstitutionalSymptomType = identifiedConstitutionalSymptom.ConstitutionalSymptomType,
             ConstitutionalSymptomTypeId = identifiedConstitutionalSymptom.ConstitutionalSymptomTypeId,
             CreatedBy = identifiedConstitutionalSymptom.CreatedBy,
             CreatedIn = identifiedConstitutionalSymptom.CreatedIn,
             DateCreated = identifiedConstitutionalSymptom.DateCreated,
             DateModified = identifiedConstitutionalSymptom.DateModified,
             EncounterType = identifiedConstitutionalSymptom.EncounterType,
             InteractionId = identifiedConstitutionalSymptom.InteractionId,
             IsDeleted = identifiedConstitutionalSymptom.IsDeleted,
             IsSynced = identifiedConstitutionalSymptom.IsSynced,
             ModifiedBy = identifiedConstitutionalSymptom.ModifiedBy,
             ModifiedIn = identifiedConstitutionalSymptom.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


         }).AsQueryable();

                if (encounterType == null)
                    return await identifiedConstitutionalSymptomAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedConstitutionalSymptomAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetIdentifiedConstitutionalSymptomsByClientIDTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedConstitutionalSymptoms.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedConstitutionalSymptoms.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a encounter by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table encounters.</param>
        /// <returns>Returns a Encounter if the key is matched.</returns>
        public async Task<IEnumerable<IdentifiedConstitutionalSymptom>> GetIdentifiedConstitutionalSymptomsByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedConstitutionalSymptoms.Include(p => p.ConstitutionalSymptomType).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         identifiedConstitutionalSymptom => identifiedConstitutionalSymptom.EncounterId,
         encounter => encounter.Oid,
         (identifiedConstitutionalSymptom, encounter) => new IdentifiedConstitutionalSymptom
         {
             EncounterId = identifiedConstitutionalSymptom.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             ClientId = identifiedConstitutionalSymptom.ClientId,
             ConstitutionalSymptomType = identifiedConstitutionalSymptom.ConstitutionalSymptomType,
             ConstitutionalSymptomTypeId = identifiedConstitutionalSymptom.ConstitutionalSymptomTypeId,
             CreatedBy = identifiedConstitutionalSymptom.CreatedBy,
             CreatedIn = identifiedConstitutionalSymptom.CreatedIn,
             DateCreated = identifiedConstitutionalSymptom.DateCreated,
             DateModified = identifiedConstitutionalSymptom.DateModified,
             EncounterType = identifiedConstitutionalSymptom.EncounterType,
             InteractionId = identifiedConstitutionalSymptom.InteractionId,
             IsDeleted = identifiedConstitutionalSymptom.IsDeleted,
             IsSynced = identifiedConstitutionalSymptom.IsSynced,
             ModifiedBy = identifiedConstitutionalSymptom.ModifiedBy,
             ModifiedIn = identifiedConstitutionalSymptom.ModifiedIn,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == identifiedConstitutionalSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


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