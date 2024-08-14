using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using static Utilities.Constants.Enums;
using Utilities.Constants;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 04.01.2023
 * Modified by  : Shakil
 * Last modified: 18.01.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IConditionRepository interface.
    /// </summary>
    public class ConditionRepository : Repository<Condition>, IConditionRepository
    {
        public ConditionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a condition by key.
        /// </summary>
        /// <param name="key">Primary key of the table Conditions.</param>
        /// <returns>Returns a condition if the key is matched.</returns>
        public async Task<Condition> GetConditionByKey(Guid key)
        {
            try
            {
                var condition = await LoadWithChildAsync<Condition>(i => i.InteractionId == key && i.IsDeleted == false, p => p.ICDDiagnosis, p => p.NTGLevelThreeDiagnosis);

                if (condition != null)
                {
                    condition.ClinicianName = await context.UserAccounts.Where(x => x.Oid == condition.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    condition.FacilityName = await context.Facilities.Where(x => x.Oid == condition.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    condition.EncounterDate = await context.Encounters.Where(x => x.Oid == condition.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }
                return condition;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of conditions.
        /// </summary>
        /// <returns>Returns a list of all conditions.</returns>
        public async Task<IEnumerable<Condition>> GetConditions()
        {
            try
            {
                return await context.Conditions.AsNoTracking().Where(x => x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        condition => condition.EncounterId,
                        encounter => encounter.Oid,
                        (condition, encounter) => new Condition
                        {
                            // Properties from Condition
                            InteractionId = condition.InteractionId,
                            ConditionType = condition.ConditionType,
                            DateDiagnosed = condition.DateDiagnosed,
                            DateResolved = condition.DateResolved,
                            IsOngoing = condition.IsOngoing,
                            Certainty = condition.Certainty,
                            Comments = condition.Comments,
                            NTGId = condition.NTGId,
                            NTGLevelThreeDiagnosis = condition.NTGLevelThreeDiagnosis,
                            ICDId = condition.ICDId,
                            ICDDiagnosis = condition.ICDDiagnosis,
                            ClientId = condition.ClientId,
                            Client = condition.Client,

                            // Properties from EncounterBaseModel
                            EncounterId = condition.EncounterId,
                            EncounterType = condition.EncounterType,
                            CreatedIn = condition.CreatedIn,
                            DateCreated = condition.DateCreated,
                            CreatedBy = condition.CreatedBy,
                            ModifiedIn = condition.ModifiedIn,
                            DateModified = condition.DateModified,
                            ModifiedBy = condition.ModifiedBy,
                            IsDeleted = condition.IsDeleted,
                            IsSynced = condition.IsSynced,
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

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<Condition>> GetConditionByClient(Guid ClientID)
        {
            try
            {
                return await context.Conditions.Include(x => x.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(c => c.IsDeleted == false && c.ClientId == ClientID)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        condition => condition.EncounterId,
                        encounter => encounter.Oid,
                        (condition, encounter) => new Condition
                        {
                            // Properties from Condition
                            InteractionId = condition.InteractionId,
                            ConditionType = condition.ConditionType,
                            DateDiagnosed = condition.DateDiagnosed,
                            DateResolved = condition.DateResolved,
                            IsOngoing = condition.IsOngoing,
                            Certainty = condition.Certainty,
                            Comments = condition.Comments,
                            NTGId = condition.NTGId,
                            NTGLevelThreeDiagnosis = condition.NTGLevelThreeDiagnosis,
                            ICDId = condition.ICDId,
                            ICDDiagnosis = condition.ICDDiagnosis,
                            ClientId = condition.ClientId,
                            Client = condition.Client,

                            // Properties from EncounterBaseModel
                            EncounterId = condition.EncounterId,
                            EncounterType = condition.EncounterType,
                            CreatedIn = condition.CreatedIn,
                            DateCreated = condition.DateCreated,
                            CreatedBy = condition.CreatedBy,
                            ModifiedIn = condition.ModifiedIn,
                            DateModified = condition.DateModified,
                            ModifiedBy = condition.ModifiedBy,
                            IsDeleted = condition.IsDeleted,
                            IsSynced = condition.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == condition.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == condition.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    ).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Condition>> GetConditionByClientLast24Hours(Guid ClientID)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24); 

              return await context.Conditions.Include(x => x.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(c => c.IsDeleted == false && c.ClientId == ClientID && c.DateCreated >= Last24Hours)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        condition => condition.EncounterId,
                        encounter => encounter.Oid,
                        (condition, encounter) => new Condition
                        {
                            // Properties from Condition
                            InteractionId = condition.InteractionId,
                            ConditionType = condition.ConditionType,
                            DateDiagnosed = condition.DateDiagnosed,
                            DateResolved = condition.DateResolved,
                            IsOngoing = condition.IsOngoing,
                            Certainty = condition.Certainty,
                            Comments = condition.Comments,
                            NTGId = condition.NTGId,
                            NTGLevelThreeDiagnosis = condition.NTGLevelThreeDiagnosis,
                            ICDId = condition.ICDId,
                            ICDDiagnosis = condition.ICDDiagnosis,
                            ClientId = condition.ClientId,
                            Client = condition.Client,

                            // Properties from EncounterBaseModel
                            EncounterId = condition.EncounterId,
                            EncounterType = condition.EncounterType,
                            CreatedIn = condition.CreatedIn,
                            DateCreated = condition.DateCreated,
                            CreatedBy = condition.CreatedBy,
                            ModifiedIn = condition.ModifiedIn,
                            DateModified = condition.DateModified,
                            ModifiedBy = condition.ModifiedBy,
                            IsDeleted = condition.IsDeleted,
                            IsSynced = condition.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == condition.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == condition.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    ).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Condition>> GetConditionByClient(Guid ClientID, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var conditions = context.Conditions.Include(x => x.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(x => x.IsDeleted == false && x.ClientId == ClientID)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        condition => condition.EncounterId,
                        encounter => encounter.Oid,
                        (condition, encounter) => new Condition
                        {
                            // Properties from Condition
                            InteractionId = condition.InteractionId,
                            ConditionType = condition.ConditionType,
                            DateDiagnosed = condition.DateDiagnosed,
                            DateResolved = condition.DateResolved,
                            IsOngoing = condition.IsOngoing,
                            Certainty = condition.Certainty,
                            Comments = condition.Comments,
                            NTGId = condition.NTGId,
                            NTGLevelThreeDiagnosis = condition.NTGLevelThreeDiagnosis,
                            ICDId = condition.ICDId,
                            ICDDiagnosis = condition.ICDDiagnosis,
                            ClientId = condition.ClientId,
                            Client = condition.Client,

                            // Properties from EncounterBaseModel
                            EncounterId = condition.EncounterId,
                            EncounterType = condition.EncounterType,
                            CreatedIn = condition.CreatedIn,
                            DateCreated = condition.DateCreated,
                            CreatedBy = condition.CreatedBy,
                            ModifiedIn = condition.ModifiedIn,
                            DateModified = condition.DateModified,
                            ModifiedBy = condition.ModifiedBy,
                            IsDeleted = condition.IsDeleted,
                            IsSynced = condition.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == condition.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == condition.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    ).AsQueryable();

                if (encounterType == null)
                    return await conditions.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await conditions.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetConditionByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Conditions.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Conditions.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<Condition>> GetConditionByOPDVisitID(Guid encounterId)
        {
            return await context.Conditions.Include(x => x.ICDDiagnosis).Include(n => n.NTGLevelThreeDiagnosis).AsNoTracking().Where(c => c.IsDeleted == false && c.EncounterId == encounterId)
                   .Join(
                       context.Encounters.AsNoTracking(),
                       condition => condition.EncounterId,
                       encounter => encounter.Oid,
                       (condition, encounter) => new Condition
                       {
                           // Properties from Condition
                           InteractionId = condition.InteractionId,
                           ConditionType = condition.ConditionType,
                           DateDiagnosed = condition.DateDiagnosed,
                           DateResolved = condition.DateResolved,
                           IsOngoing = condition.IsOngoing,
                           Certainty = condition.Certainty,
                           Comments = condition.Comments,
                           NTGId = condition.NTGId,
                           NTGLevelThreeDiagnosis = condition.NTGLevelThreeDiagnosis,
                           ICDId = condition.ICDId,
                           ICDDiagnosis = condition.ICDDiagnosis,
                           ClientId = condition.ClientId,
                           Client = condition.Client,

                           // Properties from EncounterBaseModel
                           EncounterId = condition.EncounterId,
                           EncounterType = condition.EncounterType,
                           CreatedIn = condition.CreatedIn,
                           DateCreated = condition.DateCreated,
                           CreatedBy = condition.CreatedBy,
                           ModifiedIn = condition.ModifiedIn,
                           DateModified = condition.DateModified,
                           ModifiedBy = condition.ModifiedBy,
                           IsDeleted = condition.IsDeleted,
                           IsSynced = condition.IsSynced,
                           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                           ClinicianName = context.UserAccounts.Where(x => x.Oid == condition.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                           FacilityName = context.Facilities.Where(x => x.Oid == condition.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                       }
                   ).ToListAsync();

        }
    }
}