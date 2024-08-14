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
    /// Implementation of IIdentifiedTBSymptomRepository interface.
    /// </summary>
    public class IdentifiedTBSymptomRepository : Repository<IdentifiedTBSymptom>, IIdentifiedTBSymptomRepository
    {
        public IdentifiedTBSymptomRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="clientId">Primary key of the table Clients.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByClient(Guid clientId)
        {
            try
            {
                return await context.IdentifiedTBSymptoms.Include(p => p.TBSymptom).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
          .Join(
              context.Encounters.AsNoTracking(),
              identifiedTBSymptom => identifiedTBSymptom.EncounterId,
              encounter => encounter.Oid,
              (identifiedTBSymptom, encounter) => new IdentifiedTBSymptom
              {
                  EncounterId = identifiedTBSymptom.EncounterId,
                  EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                  ClientId = identifiedTBSymptom.ClientId,
                  CreatedBy = encounter.CreatedBy,
                  CreatedIn = encounter.CreatedIn,
                  DateCreated = encounter.DateCreated,
                  DateModified = encounter.DateModified,
                  EncounterType = identifiedTBSymptom.EncounterType,
                  InteractionId = identifiedTBSymptom.InteractionId,
                  IsDeleted = encounter.IsDeleted,
                  IsSynced = encounter.IsSynced,
                  ModifiedBy = encounter.ModifiedBy,
                  ModifiedIn = encounter.ModifiedIn,
                  TBSymptom = identifiedTBSymptom.TBSymptom,
                  TBSymptomId = identifiedTBSymptom.TBSymptomId,
                  ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                  FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

              }).ToListAsync();
                //    return await LoadListWithChildAsync<IdentifiedTBSymptom>(u => u.IsDeleted == false && u.ClientId == clientId, t => t.TBSymptom);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var identifiedTBSymptomAsQuerable = context.IdentifiedTBSymptoms.Include(p => p.TBSymptom).Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
             identifiedTBSymptom => identifiedTBSymptom.EncounterId,
             encounter => encounter.Oid,
             (identifiedTBSymptom, encounter) => new IdentifiedTBSymptom
             {
                 EncounterId = identifiedTBSymptom.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 ClientId = identifiedTBSymptom.ClientId,
                 CreatedBy = encounter.CreatedBy,
                 CreatedIn = encounter.CreatedIn,
                 DateCreated = encounter.DateCreated,
                 DateModified = encounter.DateModified,
                 EncounterType = identifiedTBSymptom.EncounterType,
                 InteractionId = identifiedTBSymptom.InteractionId,
                 IsDeleted = encounter.IsDeleted,
                 IsSynced = encounter.IsSynced,
                 ModifiedBy = encounter.ModifiedBy,
                 ModifiedIn = encounter.ModifiedIn,
                 TBSymptom = identifiedTBSymptom.TBSymptom,
                 TBSymptomId = identifiedTBSymptom.TBSymptomId,
                 ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                 FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

             }).AsQueryable();

                if (encounterType == null)
                    return await identifiedTBSymptomAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await identifiedTBSymptomAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetIdentifiedTBSymptomByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.IdentifiedTBSymptoms.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.IdentifiedTBSymptoms.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }

        /// <summary>
        /// The method is used to get a IdentifiedTBSymptomBy by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounters.</param>
        /// <returns>Returns a Encounters if the key is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptomByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedTBSymptoms.Include(p => p.TBSymptom).Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
           .Join(
               context.Encounters.AsNoTracking(),
               identifiedTBSymptom => identifiedTBSymptom.EncounterId,
               encounter => encounter.Oid,
               (identifiedTBSymptom, encounter) => new IdentifiedTBSymptom
               {
                   EncounterId = identifiedTBSymptom.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   ClientId = identifiedTBSymptom.ClientId,
                   CreatedBy = encounter.CreatedBy,
                   CreatedIn = encounter.CreatedIn,
                   DateCreated = encounter.DateCreated,
                   DateModified = encounter.DateModified,
                   EncounterType = identifiedTBSymptom.EncounterType,
                   InteractionId = identifiedTBSymptom.InteractionId,
                   IsDeleted = encounter.IsDeleted,
                   IsSynced = encounter.IsSynced,
                   ModifiedBy = encounter.ModifiedBy,
                   ModifiedIn = encounter.ModifiedIn,
                   TBSymptom = identifiedTBSymptom.TBSymptom,
                   TBSymptomId = identifiedTBSymptom.TBSymptomId,
                   ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedTBSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                   FacilityName = context.Facilities.Where(x => x.Oid == identifiedTBSymptom.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

               }).OrderByDescending(x => x.EncounterDate).ToListAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedTBSymptom by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedTBSymptoms.</param>
        /// <returns>Returns a IdentifiedTBSymptom if the key is matched.</returns>
        public async Task<IdentifiedTBSymptom> GetIdentifiedTBSymptomByKey(Guid key)
        {
            try
            {
                var identifiedTBSymptom = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedTBSymptom != null)
                {
                    identifiedTBSymptom.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedTBSymptom.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedTBSymptom.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedTBSymptom.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedTBSymptom.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedTBSymptom.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
   
                return identifiedTBSymptom;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a IdentifiedTBSymptom by key.
        /// </summary>
        /// <returns>Returns a IdentifiedTBSymptom if the key is matched.</returns>
        public async Task<IEnumerable<IdentifiedTBSymptom>> GetIdentifiedTBSymptoms()
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}