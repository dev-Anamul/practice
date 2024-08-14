using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 04.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class ObstetricExaminationRepository : Repository<ObstetricExamination>, IObstetricExaminationRepository
    {
        public ObstetricExaminationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a ObstetricExamination if the ClientID is matched.</returns>
        public async Task<IEnumerable<ObstetricExamination>> GetObstetricExaminationByClient(Guid clientId)
        {
            try
            {
                return await context.ObstericExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
       .Join(
           context.Encounters.AsNoTracking(),
           obstetricExamination => obstetricExamination.EncounterId,
           encounter => encounter.Oid,
           (obstetricExamination, encounter) => new ObstetricExamination
           {
               EncounterId = obstetricExamination.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               ModifiedIn = encounter.ModifiedIn,
               ModifiedBy = obstetricExamination.ModifiedBy,
               ClientId = obstetricExamination.ClientId,
               Contraction = obstetricExamination.Contraction,
               CreatedBy = obstetricExamination.CreatedBy,
               CreatedIn = obstetricExamination.CreatedIn,
               DateCreated = obstetricExamination.DateCreated,
               DateModified = obstetricExamination.DateModified,
               EncounterType = obstetricExamination.EncounterType,
               Engaged = obstetricExamination.Engaged,
               FetalHeart = obstetricExamination.FetalHeart,
               FetalHeartRate = obstetricExamination.FetalHeartRate,
               InteractionId = obstetricExamination.InteractionId,
               IsDeleted = obstetricExamination.IsDeleted,
               IsSynced = obstetricExamination.IsSynced,
               Lie = obstetricExamination.Lie,
               SFH = obstetricExamination.SFH,
               Presentation = obstetricExamination.Presentation,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ObstetricExamination by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all ObstetricExamination by EncounterID.</returns>
        public async Task<IEnumerable<ObstetricExamination>> GetObstetricExaminationByEncounter(Guid encounterId)
        {
            try
            {
                return await context.ObstericExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
       .Join(
           context.Encounters.AsNoTracking(),
           obstetricExamination => obstetricExamination.EncounterId,
           encounter => encounter.Oid,
           (obstetricExamination, encounter) => new ObstetricExamination
           {
               EncounterId = obstetricExamination.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               ModifiedIn = encounter.ModifiedIn,
               ModifiedBy = obstetricExamination.ModifiedBy,
               ClientId = obstetricExamination.ClientId,
               Contraction = obstetricExamination.Contraction,
               CreatedBy = obstetricExamination.CreatedBy,
               CreatedIn = obstetricExamination.CreatedIn,
               DateCreated = obstetricExamination.DateCreated,
               DateModified = obstetricExamination.DateModified,
               EncounterType = obstetricExamination.EncounterType,
               Engaged = obstetricExamination.Engaged,
               FetalHeart = obstetricExamination.FetalHeart,
               FetalHeartRate = obstetricExamination.FetalHeartRate,
               InteractionId = obstetricExamination.InteractionId,
               IsDeleted = obstetricExamination.IsDeleted,
               IsSynced = obstetricExamination.IsSynced,
               Lie = obstetricExamination.Lie,
               SFH = obstetricExamination.SFH,
               Presentation = obstetricExamination.Presentation,
               
           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a ObstetricExamination by key.
        /// </summary>
        /// <param name="key">Primary key of the table ObstetricExaminations.</param>
        /// <returns>Returns a ObstetricExamination if the key is matched.</returns>
        public async Task<ObstetricExamination> GetObstetricExaminationByKey(Guid key)
        {
            try
            {
                var obstetricExamination = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (obstetricExamination != null)
                    obstetricExamination.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return obstetricExamination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ObstetricExaminations.
        /// </summary>
        /// <returns>Returns a list of all ObstetricExamination.</returns>
        public async Task<IEnumerable<ObstetricExamination>> GetObstetricExaminations()
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