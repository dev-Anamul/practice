using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Labib
 * Date created : 30.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PriorARTExposerRepository : Repository<PriorARTExposure>, IPriorARTExposerRepository
    {
        /// <summary>
        /// Implementation of IPriorARTExposerRepository interface.
        /// </summary>
        public PriorARTExposerRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PriorARTExposer by key.
        /// </summary>
        /// <param name="key">Primary key of the table PriorARTExposers.</param>
        /// <returns>Returns a PriorARTExposer if the key is matched.</returns>
        public async Task<PriorARTExposure> GetPriorARTExposerByKey(Guid key)
        {
            try
            {
                var priorARTExposure = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (priorARTExposure != null)
                    priorARTExposure.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return priorARTExposure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PriorARTExposer.
        /// </summary>
        /// <returns>Returns a list of all PriorARTExposers.</returns>
        public async Task<IEnumerable<PriorARTExposure>> GetPriorARTExposers()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PriorARTExposer by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a PriorARTExposer if the clientId is matched.</returns>
        public async Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByClient(Guid clientId)
        {
            try
            {
                var priorARTExposures = await context.PriorARTExposures.Include(x => x.TakenARTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
               .Join(
                   context.Encounters.AsNoTracking(),
                   priorARTExposure => priorARTExposure.EncounterId,
                   encounter => encounter.Oid,
                   (priorARTExposure, encounter) => new PriorARTExposure
                   {
                       EncounterId = priorARTExposure.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ClientId = priorARTExposure.ClientId,
                       CreatedBy = priorARTExposure.CreatedBy,
                       CreatedIn = priorARTExposure.CreatedIn,
                       DateCreated = priorARTExposure.DateCreated,
                       DateEnd = priorARTExposure.TakenARTDrugs.SingleOrDefault()!.EndDate,
                       DateModified = priorARTExposure.DateModified,
                       DateTaken = priorARTExposure.TakenARTDrugs.SingleOrDefault()!.StartDate,
                       EncounterType = priorARTExposure.EncounterType,
                       ExposureReason = priorARTExposure.ExposureReason,
                       InteractionId = priorARTExposure.InteractionId,
                       IsDeleted = priorARTExposure.IsDeleted,
                       IsSynced = priorARTExposure.IsSynced,
                       ModifiedBy = priorARTExposure.ModifiedBy,
                       ModifiedIn = priorARTExposure.ModifiedIn,
                       StoppingReason = priorARTExposure.StoppingReason,
                       TakenARTDrugs = priorARTExposure.TakenARTDrugs.ToList(),


                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

                foreach (var item in priorARTExposures)
                {
                    foreach (var drug in item.TakenARTDrugs)
                    {
                        drug.ARTDrug = context.ARTDrugs.Where(x => x.Oid == drug.ARTDrugId).FirstOrDefault();
                    }
                }

                return priorARTExposures;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                List<PriorARTExposure> priorARTExposures = new List<PriorARTExposure>();

                var priorARTExposuresAsQuerable = context.PriorARTExposures.Include(x => x.TakenARTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
             .Join(
                 context.Encounters.AsNoTracking(),
                 priorARTExposure => priorARTExposure.EncounterId,
                 encounter => encounter.Oid,
                 (priorARTExposure, encounter) => new PriorARTExposure
                 {
                     EncounterId = priorARTExposure.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     ClientId = priorARTExposure.ClientId,
                     CreatedBy = priorARTExposure.CreatedBy,
                     CreatedIn = priorARTExposure.CreatedIn,
                     DateCreated = priorARTExposure.DateCreated,
                     DateEnd = priorARTExposure.TakenARTDrugs.SingleOrDefault()!.EndDate,
                     DateModified = priorARTExposure.DateModified,
                     DateTaken = priorARTExposure.TakenARTDrugs.SingleOrDefault()!.StartDate,
                     EncounterType = priorARTExposure.EncounterType,
                     ExposureReason = priorARTExposure.ExposureReason,
                     InteractionId = priorARTExposure.InteractionId,
                     IsDeleted = priorARTExposure.IsDeleted,
                     IsSynced = priorARTExposure.IsSynced,
                     ModifiedBy = priorARTExposure.ModifiedBy,
                     ModifiedIn = priorARTExposure.ModifiedIn,
                     StoppingReason = priorARTExposure.StoppingReason,
                     TakenARTDrugs = priorARTExposure.TakenARTDrugs.ToList(),

                 }).AsQueryable();

                if (encounterType == null)
                    priorARTExposures = await priorARTExposuresAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    priorARTExposures = await priorARTExposuresAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

                foreach (var item in priorARTExposures)
                {
                    foreach (var drug in item.TakenARTDrugs)
                    {
                        drug.ARTDrug = context.ARTDrugs.Where(x => x.Oid == drug.ARTDrugId).FirstOrDefault();
                    }
                }

                return priorARTExposures;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetPriorARTExposerByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.PriorARTExposures.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.PriorARTExposures.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get the list of PriorARTExposer by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PriorARTExposer by EncounterID.</returns>
        public async Task<IEnumerable<PriorARTExposure>> GetPriorARTExposerByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PriorARTExposures.Include(x => x.TakenARTDrugs).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
             .Join(
                 context.Encounters.AsNoTracking(),
                 priorARTExposure => priorARTExposure.EncounterId,
                 encounter => encounter.Oid,
                 (priorARTExposure, encounter) => new PriorARTExposure
                 {
                     EncounterId = priorARTExposure.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     ClientId = priorARTExposure.ClientId,
                     CreatedBy = priorARTExposure.CreatedBy,
                     CreatedIn = priorARTExposure.CreatedIn,
                     DateCreated = priorARTExposure.DateCreated,
                     DateEnd = priorARTExposure.DateEnd,
                     DateModified = priorARTExposure.DateModified,
                     DateTaken = priorARTExposure.DateTaken,
                     EncounterType = priorARTExposure.EncounterType,
                     ExposureReason = priorARTExposure.ExposureReason,
                     InteractionId = priorARTExposure.InteractionId,
                     IsDeleted = priorARTExposure.IsDeleted,
                     IsSynced = priorARTExposure.IsSynced,
                     ModifiedBy = priorARTExposure.ModifiedBy,
                     ModifiedIn = priorARTExposure.ModifiedIn,
                     StoppingReason = priorARTExposure.StoppingReason,
                     TakenARTDrugs = priorARTExposure.TakenARTDrugs.ToList(),

                 }).OrderByDescending(x => x.EncounterDate).ToListAsync();
                 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}