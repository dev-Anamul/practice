using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ApgarScoreRepository : Repository<ApgarScore>, IApgarScoreRepository
    {
        /// <summary>
        /// Implementation of IApgarScoreRepository interface.
        /// </summary>
        public ApgarScoreRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ApgarScore by key.
        /// </summary>
        /// <param name="key">Primary key of the table ApgarScores.</param>
        /// <returns>Returns a ApgarScore if the key is matched.</returns>
        public async Task<ApgarScore> GetApgarScoreByKey(Guid key)
        {
            try
            {
                var apgarScore = await context.ApgarScores
                    .AsNoTracking()
                    .Where(p => p.InteractionId == key && p.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (apgarScore != null)
                {
                    apgarScore.ClinicianName = context.UserAccounts.Where(x => x.Oid == apgarScore.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "";
                    apgarScore.FacilityName = context.Facilities.Where(x => x.Oid == apgarScore.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "";
                    apgarScore.EncounterDate = await context.Encounters.Where(x => x.Oid == apgarScore.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return apgarScore;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ApgarScore.
        /// </summary>
        /// <returns>Returns a list of all ApgarScores.</returns>
        public async Task<IEnumerable<ApgarScore>> GetApgarScores()
        {
            try
            {
                return await context.ApgarScores.AsNoTracking().Where(a => a.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        apgarScore => apgarScore.EncounterId,
                        encounter => encounter.Oid,
                        (apgarScore, encounter) => new ApgarScore
                        {
                            InteractionId = apgarScore.InteractionId,
                            ApgarTimeSpan = apgarScore.ApgarTimeSpan,
                            Appearance = apgarScore.Appearance,
                            Pulses = apgarScore.Pulses,
                            Grimace = apgarScore.Grimace,
                            Activity = apgarScore.Activity,
                            Respiration = apgarScore.Respiration,
                            TotalScore = apgarScore.TotalScore,
                            NeonatalId = apgarScore.NeonatalId,
                            NewBornDetail = apgarScore.NewBornDetail,
                            ApgarScores = apgarScore.ApgarScores,
                            // Add other properties as needed
                            EncounterId = encounter.Oid,
                            EncounterType = apgarScore.EncounterType,
                            CreatedBy = apgarScore.CreatedBy,
                            CreatedIn = apgarScore.CreatedIn,
                            DateCreated = apgarScore.DateCreated,
                            DateModified = apgarScore.DateModified,
                            ModifiedBy = apgarScore.ModifiedBy,
                            ModifiedIn = apgarScore.ModifiedIn,
                            IsDeleted = apgarScore.IsDeleted,
                            IsSynced = apgarScore.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == apgarScore.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == apgarScore.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ApgarScore by NeonatalId.
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a ApgarScore if the NeonatalId is matched.</returns>
        public async Task<IEnumerable<ApgarScore>> GetApgarScoreByNeonatal(Guid neonatalId)
        {
            try
            {
                return await context.ApgarScores.AsNoTracking().Where(a => a.IsDeleted == false && a.NeonatalId == neonatalId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        apgarScore => apgarScore.EncounterId,
                        encounter => encounter.Oid,
                        (apgarScore, encounter) => new ApgarScore
                        {
                            InteractionId = apgarScore.InteractionId,
                            ApgarTimeSpan = apgarScore.ApgarTimeSpan,
                            Appearance = apgarScore.Appearance,
                            Pulses = apgarScore.Pulses,
                            Grimace = apgarScore.Grimace,
                            Activity = apgarScore.Activity,
                            Respiration = apgarScore.Respiration,
                            TotalScore = apgarScore.TotalScore,
                            NeonatalId = apgarScore.NeonatalId,
                            NewBornDetail = apgarScore.NewBornDetail,
                            ApgarScores = apgarScore.ApgarScores,
                            // Add other properties as needed
                            EncounterId = encounter.Oid,
                            EncounterType = apgarScore.EncounterType,
                            CreatedBy = apgarScore.CreatedBy,
                            CreatedIn = apgarScore.CreatedIn,
                            DateCreated = apgarScore.DateCreated,
                            DateModified = apgarScore.DateModified,
                            ModifiedBy = apgarScore.ModifiedBy,
                            ModifiedIn = apgarScore.ModifiedIn,
                            IsDeleted = apgarScore.IsDeleted,
                            IsSynced = apgarScore.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == apgarScore.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == apgarScore.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// The method is used to get the list of ApgarScore by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ApgarScore by EncounterID.</returns>
        //public async Task<IEnumerable<ApgarScore>> GetApgarScoreByEncounter(Guid encounterID)
        //{
        //    try
        //    {
        //        return await QueryAsync(n => n.IsDeleted == false && n.EncounterId == encounterID);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<IEnumerable<ApgarScore>> GetApgarScoreByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ApgarScores.AsNoTracking().Where(a => a.IsDeleted == false && a.EncounterId == encounterId)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        apgarScore => apgarScore.EncounterId,
                        encounter => encounter.Oid,
                        (apgarScore, encounter) => new ApgarScore
                        {
                            InteractionId = apgarScore.InteractionId,
                            ApgarTimeSpan = apgarScore.ApgarTimeSpan,
                            Appearance = apgarScore.Appearance,
                            Pulses = apgarScore.Pulses,
                            Grimace = apgarScore.Grimace,
                            Activity = apgarScore.Activity,
                            Respiration = apgarScore.Respiration,
                            TotalScore = apgarScore.TotalScore,
                            NeonatalId = apgarScore.NeonatalId,
                            NewBornDetail = apgarScore.NewBornDetail,
                            ApgarScores = apgarScore.ApgarScores,
                            // Add other properties as needed
                            EncounterId = encounter.Oid,
                            EncounterType = apgarScore.EncounterType,
                            CreatedBy = apgarScore.CreatedBy,
                            CreatedIn = apgarScore.CreatedIn,
                            DateCreated = apgarScore.DateCreated,
                            DateModified = apgarScore.DateModified,
                            ModifiedBy = apgarScore.ModifiedBy,
                            ModifiedIn = apgarScore.ModifiedIn,
                            IsDeleted = apgarScore.IsDeleted,
                            IsSynced = apgarScore.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == apgarScore.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == apgarScore.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        })
                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}