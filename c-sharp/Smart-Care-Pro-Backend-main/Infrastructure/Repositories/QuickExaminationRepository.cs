using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tariqul Islam
 * Date created : 05.03.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class QuickExaminationRepository : Repository<QuickExamination>, IQuickExaminationRepository
    {
        public QuickExaminationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a QuickExamination if the ClientID is matched.</returns>
        public async Task<IEnumerable<QuickExamination>> ReadQuickExaminationByClient(Guid ClientID)
        {
            try
            {
                return await context.QuickExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
                 .Join(
                     context.Encounters.AsNoTracking(),
                     quickExamination => quickExamination.EncounterId,
                     encounter => encounter.Oid,
                     (quickExamination, encounter) => new QuickExamination
                     {
                         EncounterId = quickExamination.EncounterId,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         ClientId = quickExamination.ClientId,
                         CreatedIn = quickExamination.CreatedIn,
                         DateCreated = quickExamination.DateCreated,
                         CreatedBy = quickExamination.CreatedBy,
                         Armpits = quickExamination.Armpits,
                         BreastLumps = quickExamination.BreastLumps,
                         CervicalGlandsEnlarged = quickExamination.CervicalGlandsEnlarged,
                         DateModified = quickExamination.DateModified,
                         DentalDisease = quickExamination.DentalDisease,
                         EncounterType = quickExamination.EncounterType,
                         FibroidPalpable = quickExamination.FibroidPalpable,
                         HairHealthy = quickExamination.HairHealthy,
                         HairWellSpread = quickExamination.Thrash,
                         Thrash = quickExamination.Thrash,
                         HeadInfection = quickExamination.HeadInfection,
                         InteractionId = quickExamination.InteractionId,
                         IsDeleted = quickExamination.IsDeleted,
                         IsSynced = quickExamination.IsSynced,
                         LiverPalpable = quickExamination.LiverPalpable,
                         Masses = quickExamination.Masses,
                         ModifiedBy = quickExamination.ModifiedBy,
                         ModifiedIn = quickExamination.ModifiedIn,
                         NeckAbnormal = quickExamination.NeckAbnormal,
                         Scars = quickExamination.Scars,
                         Tenderness = quickExamination.Tenderness,

                     }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of QuickExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all QuickExamination by EncounterID.</returns>
        public async Task<IEnumerable<QuickExamination>> ReadQuickExaminationByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.QuickExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
                 .Join(
                     context.Encounters.AsNoTracking(),
                     quickExamination => quickExamination.EncounterId,
                     encounter => encounter.Oid,
                     (quickExamination, encounter) => new QuickExamination
                     {
                         EncounterId = quickExamination.EncounterId,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         ClientId = quickExamination.ClientId,
                         CreatedIn = quickExamination.CreatedIn,
                         DateCreated = quickExamination.DateCreated,
                         CreatedBy = quickExamination.CreatedBy,
                         Armpits = quickExamination.Armpits,
                         BreastLumps = quickExamination.BreastLumps,
                         CervicalGlandsEnlarged = quickExamination.CervicalGlandsEnlarged,
                         DateModified = quickExamination.DateModified,
                         DentalDisease = quickExamination.DentalDisease,
                         EncounterType = quickExamination.EncounterType,
                         FibroidPalpable = quickExamination.FibroidPalpable,
                         HairHealthy = quickExamination.HairHealthy,
                         HairWellSpread = quickExamination.Thrash,
                         Thrash = quickExamination.Thrash,
                         HeadInfection = quickExamination.HeadInfection,
                         InteractionId = quickExamination.InteractionId,
                         IsDeleted = quickExamination.IsDeleted,
                         IsSynced = quickExamination.IsSynced,
                         LiverPalpable = quickExamination.LiverPalpable,
                         Masses = quickExamination.Masses,
                         ModifiedBy = quickExamination.ModifiedBy,
                         ModifiedIn = quickExamination.ModifiedIn,
                         NeckAbnormal = quickExamination.NeckAbnormal,
                         Scars = quickExamination.Scars,
                         Tenderness = quickExamination.Tenderness,

                     }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a QuickExaminationByKey by key.
        /// </summary>
        /// <param name="key">Primary key of the table QuickExaminationByKey.</param>
        /// <returns>Returns a QuickExaminationByKey if the key is matched.</returns>
        public async Task<QuickExamination> ReadQuickExaminationByKey(Guid key)
        {
            try
            {
                var quickExamination = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (quickExamination != null)
                    quickExamination.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return quickExamination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a QuickExaminationByKey by key.
        /// </summary>
        /// <returns>Returns a QuickExaminationByKey if the key is matched.</returns>
        public async Task<IEnumerable<QuickExamination>> ReadQuickExaminations()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
