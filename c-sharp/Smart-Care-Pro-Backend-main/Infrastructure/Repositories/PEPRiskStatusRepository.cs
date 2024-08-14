using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Utilities.Constants.Enums;

/*
 * Created by   : Rezwana
 * Date created : 19.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IPEPRiskStatusRepository interface.
    /// </summary>
    public class PEPRiskStatusRepository : Repository<RiskStatus>, IPEPRiskStatusRepository
    {
        public PEPRiskStatusRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PEP risk status by key.
        /// </summary>
        /// <param name="key">Primary key of the table PEPRiskStatuses.</param>
        /// <returns>Returns a PEP risk status if the key is matched.</returns>
        public async Task<RiskStatus> GetPEPRiskStatusByKey(Guid key)
        {
            try
            {
                var riskStatus = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (riskStatus != null)
                    riskStatus.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return riskStatus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PEP risk statuses.
        /// </summary>
        /// <returns>Returns a list of all PEP risk statuses.</returns>
        public async Task<IEnumerable<RiskStatus>> GetPEPRiskStatuses()
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

        public async Task<IEnumerable<RiskStatus>> GetPEPRiskStatusByClientID(Guid clientId)
        {
            try
            {
                return await context.RiskStatuses.Include(x => x.Risk).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
            .Join(
                context.Encounters.AsNoTracking(),
                riskStatus => riskStatus.EncounterId,
                encounter => encounter.Oid,
                (riskStatus, encounter) => new RiskStatus
                {
                    EncounterId = riskStatus.EncounterId,
                    EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                    InteractionId = riskStatus.InteractionId,
                    EncounterType = riskStatus.EncounterType,
                    DateModified = riskStatus.DateModified,
                    DateCreated = riskStatus.DateCreated,
                    CreatedIn = riskStatus.CreatedIn,
                    CreatedBy = riskStatus.CreatedBy,
                    ClientId = riskStatus.ClientId,
                    IsDeleted = riskStatus.IsDeleted,
                    ModifiedIn = riskStatus.ModifiedIn,
                    Risk = riskStatus.Risk,
                    RiskId = riskStatus.RiskId,
                    IsSynced = riskStatus.IsSynced,
                    ModifiedBy = riskStatus.ModifiedBy,

                }).OrderByDescending(x => x.EncounterDate).ToListAsync();


            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<RiskStatus>> GetPEPRiskStatusByEncounterID(Guid EncounterID)
        {
            try
            {
                return await context.RiskStatuses.Include(x => x.Risk).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
           .Join(
               context.Encounters.AsNoTracking(),
               riskStatus => riskStatus.EncounterId,
               encounter => encounter.Oid,
               (riskStatus, encounter) => new RiskStatus
               {
                   EncounterId = riskStatus.EncounterId,
                   EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                   InteractionId = riskStatus.InteractionId,
                   EncounterType = riskStatus.EncounterType,
                   DateModified = riskStatus.DateModified,
                   DateCreated = riskStatus.DateCreated,
                   CreatedIn = riskStatus.CreatedIn,
                   CreatedBy = riskStatus.CreatedBy,
                   ClientId = riskStatus.ClientId,
                   IsDeleted = riskStatus.IsDeleted,
                   ModifiedIn = riskStatus.ModifiedIn,
                   RiskId = riskStatus.RiskId,
                   IsSynced = riskStatus.IsSynced,
                   ModifiedBy = riskStatus.ModifiedBy,

               }).OrderByDescending(x => x.EncounterDate).ToListAsync();
  
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}