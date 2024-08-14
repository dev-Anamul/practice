using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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
    /// Implementation of IMedicalConditionRepository interface.
    /// </summary>
    public class MedicalConditionRepository : Repository<MedicalCondition>, IMedicalConditionRepository
    {
        public MedicalConditionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a MedicalCondition by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicalConditions.</param>
        /// <returns>Returns a MedicalCondition if the key is matched.</returns>
        public async Task<MedicalCondition> GetMedicalConditionByKey(Guid key)
        {
            try
            {
                var medicalCondition = await FirstOrDefaultAsync(m => m.InteractionId == key && m.IsDeleted == false);

                if (medicalCondition != null)
                    medicalCondition.EncounterDate = await context.Encounters.Select(e => e.OPDVisitDate ?? e.IPDAdmissionDate ?? e.DateCreated).FirstOrDefaultAsync();

                return medicalCondition;

                ///return await FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of MedicalConditions.
        /// </summary>
        /// <returns>Returns a list of all MedicalConditions.</returns>
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditions()
        {
            try
            {
                //return await QueryAsync(c => c.IsDeleted == false);
                return await LoadListWithChildAsync<MedicalCondition>(s => s.IsDeleted == false, p => p.PastMedicalConditon, p => p.STIRisk);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a EncounterID by key.
        /// </summary>
        /// <param name="encounterId">Primary key of the table Encounter.</param>
        /// <returns>Returns a EncounterID if the key is matched.</returns>
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditionByEncounterID(Guid encounterId)
        {
            try
            {
                var medicalCondition = context.MedicalConditions.Where(m => m.IsDeleted == false && m.EncounterId == encounterId).Include(p => p.PastMedicalConditon).Include(s => s.STIRisk).AsNoTracking()
                 .Join(context.Encounters.AsNoTracking(),
                 medicalCondition => medicalCondition.EncounterId,
                 encounter => encounter.Oid,
                 (medicalCondition, encounter) => new MedicalCondition
                 {
                     DoesAnyHealthConditonScreened = medicalCondition.DoesAnyHealthConditonScreened,
                     ClientId = medicalCondition.ClientId,
                     CreatedBy = medicalCondition.CreatedBy,
                     CreatedIn = medicalCondition.CreatedIn,
                     DateCreated = medicalCondition.DateCreated,
                     DateModified = medicalCondition.DateModified,
                     DoesRiskOfSTIIncreased = medicalCondition.DoesRiskOfSTIIncreased,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     EncounterId = medicalCondition.EncounterId,
                     STIRisk = medicalCondition.STIRisk,
                     EncounterType = medicalCondition.EncounterType,
                     InteractionId = medicalCondition.InteractionId,
                     IsDeleted = medicalCondition.IsDeleted,
                     IsSynced = medicalCondition.IsSynced,
                     LastUnprotectedSexDays = medicalCondition.LastUnprotectedSexDays,
                     ModifiedBy = medicalCondition.ModifiedBy,
                     ModifiedIn = medicalCondition.ModifiedIn,
                     PastMedicalConditon = medicalCondition.PastMedicalConditon,
                     PastMedicalConditonId = medicalCondition.PastMedicalConditonId,
                     STIRiskId = medicalCondition.STIRiskId,
                 }).AsQueryable();

                return medicalCondition.OrderByDescending(x => x.EncounterDate);

                //return await QueryAsync(c => c.IsDeleted == false && c.EncounterID == EncounterID);
                /// return await QueryAsync(n => n.IsDeleted == false && n.EncounterId == encounterId, p => p.PastMedicalConditon, p => p.STIRisk);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a MedicalCondition by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a MedicalCondition if the ClientID is matched.</returns>
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditionByClient(Guid clientId)
        {
            try
            {
                return await context.MedicalConditions.Where(m => m.IsDeleted == false && m.ClientId == clientId).Include(p => p.PastMedicalConditon).Include(s => s.STIRisk).AsNoTracking()
                .Join(context.Encounters.AsNoTracking(),
                medicalCondition => medicalCondition.EncounterId,
                encounter => encounter.Oid,
                (medicalCondition, encounter) => new MedicalCondition
                {
                    DoesAnyHealthConditonScreened = medicalCondition.DoesAnyHealthConditonScreened,
                    ClientId = medicalCondition.ClientId,
                    CreatedBy = medicalCondition.CreatedBy,
                    CreatedIn = medicalCondition.CreatedIn,
                    DateCreated = medicalCondition.DateCreated,
                    DateModified = medicalCondition.DateModified,
                    DoesRiskOfSTIIncreased = medicalCondition.DoesRiskOfSTIIncreased,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    EncounterId = medicalCondition.EncounterId,
                    STIRisk = medicalCondition.STIRisk,
                    EncounterType = medicalCondition.EncounterType,
                    InteractionId = medicalCondition.InteractionId,
                    IsDeleted = medicalCondition.IsDeleted,
                    IsSynced = medicalCondition.IsSynced,
                    LastUnprotectedSexDays = medicalCondition.LastUnprotectedSexDays,
                    ModifiedBy = medicalCondition.ModifiedBy,
                    ModifiedIn = medicalCondition.ModifiedIn,
                    PastMedicalConditon = medicalCondition.PastMedicalConditon,
                    PastMedicalConditonId = medicalCondition.PastMedicalConditonId,
                    STIRiskId = medicalCondition.STIRiskId,
                    
                }).OrderByDescending(e => e.EncounterDate).ToListAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<MedicalCondition>> GetMedicalConditionByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var medicalCondition = context.MedicalConditions.Where(m => m.IsDeleted == false && m.ClientId == clientId).Include(p => p.PastMedicalConditon).Include(s => s.STIRisk).AsNoTracking()
                 .Join(context.Encounters.AsNoTracking(),
                 medicalCondition => medicalCondition.EncounterId,
                 encounter => encounter.Oid,
                 (medicalCondition, encounter) => new MedicalCondition
                 {
                     DoesAnyHealthConditonScreened = medicalCondition.DoesAnyHealthConditonScreened,
                     ClientId = medicalCondition.ClientId,
                     CreatedBy = medicalCondition.CreatedBy,
                     CreatedIn = medicalCondition.CreatedIn,
                     DateCreated = medicalCondition.DateCreated,
                     DateModified = medicalCondition.DateModified,
                     DoesRiskOfSTIIncreased = medicalCondition.DoesRiskOfSTIIncreased,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     EncounterId = medicalCondition.EncounterId,
                     STIRisk = medicalCondition.STIRisk,
                     EncounterType = medicalCondition.EncounterType,
                     InteractionId = medicalCondition.InteractionId,
                     IsDeleted = medicalCondition.IsDeleted,
                     IsSynced = medicalCondition.IsSynced,
                     LastUnprotectedSexDays = medicalCondition.LastUnprotectedSexDays,
                     ModifiedBy = medicalCondition.ModifiedBy,
                     ModifiedIn = medicalCondition.ModifiedIn,
                     PastMedicalConditon = medicalCondition.PastMedicalConditon,
                     PastMedicalConditonId = medicalCondition.PastMedicalConditonId,
                     STIRiskId = medicalCondition.STIRiskId,
                 }).AsQueryable();

                if (encounterType == null)
                    return await medicalCondition.OrderByDescending(e => e.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await medicalCondition.Where(p => p.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetMedicalConditionByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.MedicalConditions.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.MedicalConditions.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}