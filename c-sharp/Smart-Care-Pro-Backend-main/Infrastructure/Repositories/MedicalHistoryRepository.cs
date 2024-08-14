using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IMedicalHistoryRepository interface.
    /// </summary>
    public class MedicalHistoryRepository : Repository<MedicalHistory>, IMedicalHistoryRepository
    {
        public MedicalHistoryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a medical history by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalHistories.</param>
        /// <returns>Returns a medical history if the key is matched.</returns>
        public async Task<MedicalHistory> GetMedicalHistoryByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of medical histories.
        /// </summary>
        /// <returns>Returns a list of all medical histories.</returns>
        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistories()
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
        /// The method is used to get a OPD Visit by key.
        /// </summary>
        /// <param name="visitId">Primary key of the table OPDVisits.</param>
        /// <returns>Returns a OPD Visit if the key is matched.</returns>
        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByVisitID(Guid visitId)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == visitId);
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
        /// <returns>Returns a Visit if the key is matched.</returns>
        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistoriesByClient(Guid clientId)
        {
            try
            {
                return await context.MedicalHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
           .Join(
                  context.Encounters.AsNoTracking(),
                  medicalHistories => medicalHistories.EncounterId,
                  encounter => encounter.Oid,
                  (medicalHistories, encounter) => new MedicalHistory
                  {
                      EncounterId = medicalHistories.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      History = medicalHistories.History,
                      InformationType = medicalHistories.InformationType,
                      ClientId = medicalHistories.ClientId,
                      CreatedBy = medicalHistories.CreatedBy,
                      DateModified = medicalHistories.DateModified,
                      DateCreated = medicalHistories.DateCreated,
                      CreatedIn = medicalHistories.CreatedIn,
                      EncounterType = medicalHistories.EncounterType,
                      ModifiedIn = medicalHistories.ModifiedIn,
                      InteractionId = medicalHistories.InteractionId,
                      IsDeleted = medicalHistories.IsDeleted,
                      IsSynced = medicalHistories.IsSynced,
                      ModifiedBy = medicalHistories.ModifiedBy,

                  }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IEnumerable<MedicalHistory>> GetPastMedicalHistoriesByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var medicalHistories = context.MedicalHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId && ((p.EncounterType == encounterType &&
            p.InformationType == Enums.InformationType.AdmissionHistory) || (p.EncounterType == encounterType &&
                p.InformationType == Enums.InformationType.SurgicalHistory) || (p.EncounterType == encounterType &&
                p.InformationType == Enums.InformationType.PastDrugHistory)))
           .Join(
                  context.Encounters.AsNoTracking(),
                  medicalHistories => medicalHistories.EncounterId,
                  encounter => encounter.Oid,
                  (medicalHistories, encounter) => new MedicalHistory
                  {
                      EncounterId = medicalHistories.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      History = medicalHistories.History,
                      InformationType = medicalHistories.InformationType,
                      ClientId = medicalHistories.ClientId,
                      CreatedBy = medicalHistories.CreatedBy,
                      DateModified = medicalHistories.DateModified,
                      DateCreated = medicalHistories.DateCreated,
                      CreatedIn = medicalHistories.CreatedIn,
                      EncounterType = medicalHistories.EncounterType,
                      ModifiedIn = medicalHistories.ModifiedIn,
                      InteractionId = medicalHistories.InteractionId,
                      IsDeleted = medicalHistories.IsDeleted,
                      IsSynced = medicalHistories.IsSynced,
                      ModifiedBy = medicalHistories.ModifiedBy,

                  }).AsQueryable();

                if (encounterType == null)
                    return await medicalHistories.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await medicalHistories.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetPastMedicalHistoriesByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.MedicalHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.MedicalHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && ((x.EncounterType == encounterType &&
            x.InformationType == Enums.InformationType.AdmissionHistory) || (x.EncounterType == encounterType &&
                x.InformationType == Enums.InformationType.SurgicalHistory) || (x.EncounterType == encounterType &&
               x.InformationType == Enums.InformationType.PastDrugHistory))).Count();
        }
        public async Task<IEnumerable<MedicalHistory>> GetFamilyFoodHistoryByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var medicalHistories = context.MedicalHistories.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId && ((p.EncounterType == encounterType &&
           p.InformationType == Enums.InformationType.FamilyMedicalHistory) || (p.EncounterType == encounterType &&
               p.InformationType == Enums.InformationType.SiblingHistory) || (p.EncounterType == encounterType &&
               p.InformationType == Enums.InformationType.AlcoholandSmokingHabits)))
          .Join(
                 context.Encounters.AsNoTracking(),
                 medicalHistories => medicalHistories.EncounterId,
                 encounter => encounter.Oid,
                 (medicalHistories, encounter) => new MedicalHistory
                 {
                     EncounterId = medicalHistories.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     History = medicalHistories.History,
                     InformationType = medicalHistories.InformationType,
                     ClientId = medicalHistories.ClientId,
                     CreatedBy = medicalHistories.CreatedBy,
                     DateModified = medicalHistories.DateModified,
                     DateCreated = medicalHistories.DateCreated,
                     CreatedIn = medicalHistories.CreatedIn,
                     EncounterType = medicalHistories.EncounterType,
                     ModifiedIn = medicalHistories.ModifiedIn,
                     InteractionId = medicalHistories.InteractionId,
                     IsDeleted = medicalHistories.IsDeleted,
                     IsSynced = medicalHistories.IsSynced,
                     ModifiedBy = medicalHistories.ModifiedBy,

                 }).AsQueryable();

                if (encounterType == null)
                    return await medicalHistories.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await medicalHistories.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetFamilyFoodHistoryByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.MedicalHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.MedicalHistories.Where(x => x.IsDeleted == false && x.ClientId == clientID && ((x.InformationType == Enums.InformationType.FamilyMedicalHistory && x.EncounterType == encounterType) ||
                   (x.InformationType == Enums.InformationType.SiblingHistory && x.EncounterType == encounterType) ||
                   (x.InformationType == Enums.InformationType.AlcoholandSmokingHabits && x.EncounterType == encounterType))).Count();
        }
    }
}