using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class NeonatalAbnormalityRepository : Repository<NeonatalAbnormality>, INeonatalAbnormalityRepository
    {
        /// <summary>
        /// Implementation of INeonatalAbnormalityRepository interface.
        /// </summary>
        public NeonatalAbnormalityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a NeonatalAbnormality by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalAbnormalities.</param>
        /// <returns>Returns a NeonatalAbnormality if the key is matched.</returns>
        public async Task<NeonatalAbnormality> GetNeonatalAbnormalityByKey(Guid key)
        {
            try
            {
                var neonatalAbnormality = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (neonatalAbnormality != null)
                    neonatalAbnormality.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return neonatalAbnormality;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalAbnormality.
        /// </summary>
        /// <returns>Returns a list of all NeonatalAbnormalities.</returns>
        public async Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalities()
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
        /// The method is used to get a NeonatalAbnormality by NeonatalId.
        /// </summary>
        /// <param name="neonatalId"></param>
        /// <returns>Returns a NeonatalAbnormality if the NeonatalId is matched.</returns>
        public async Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalityByNeonatal(Guid neonatalId)
        {
            try
            {
                return await context.NeonatalAbnormalities.Where(p => p.IsDeleted == false && p.NeonatalId == neonatalId).AsNoTracking()
    .Join(
        context.Encounters.AsNoTracking(),
        neonatalAbnormality => neonatalAbnormality.EncounterId,
        encounter => encounter.Oid,
        (neonatalAbnormality, encounter) => new NeonatalAbnormality
        {
            EncounterId = neonatalAbnormality.EncounterId,
            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
            InteractionId = neonatalAbnormality.InteractionId,
            EncounterType = neonatalAbnormality.EncounterType,
            DateModified = neonatalAbnormality.DateModified,
            DateCreated = neonatalAbnormality.DateCreated,
            CreatedBy = neonatalAbnormality.CreatedBy,
            CreatedIn = neonatalAbnormality.CreatedIn,
            IsDeleted = neonatalAbnormality.IsDeleted,
            IsSynced = neonatalAbnormality.IsSynced,
            ModifiedBy = neonatalAbnormality.ModifiedBy,
            ModifiedIn = neonatalAbnormality.ModifiedIn,
            NeonatalId = neonatalAbnormality.NeonatalId,
            NewBornDetail = neonatalAbnormality.NewBornDetail,
            Other = neonatalAbnormality.Other,
            Abnormalities = neonatalAbnormality.Abnormalities

        }).OrderByDescending(x => x.EncounterDate).ToListAsync();


            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NeonatalAbnormality by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NeonatalAbnormality by EncounterID.</returns>
        public async Task<IEnumerable<NeonatalAbnormality>> GetNeonatalAbnormalityByEncounter(Guid encounterId)
        {
            try
            {
                return await context.NeonatalAbnormalities.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
  .Join(
      context.Encounters.AsNoTracking(),
      neonatalAbnormality => neonatalAbnormality.EncounterId,
      encounter => encounter.Oid,
      (neonatalAbnormality, encounter) => new NeonatalAbnormality
      {
          EncounterId = neonatalAbnormality.EncounterId,
          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
          InteractionId = neonatalAbnormality.InteractionId,
          EncounterType = neonatalAbnormality.EncounterType,
          DateModified = neonatalAbnormality.DateModified,
          DateCreated = neonatalAbnormality.DateCreated,
          CreatedBy = neonatalAbnormality.CreatedBy,
          CreatedIn = neonatalAbnormality.CreatedIn,
          IsDeleted = neonatalAbnormality.IsDeleted,
          IsSynced = neonatalAbnormality.IsSynced,
          ModifiedBy = neonatalAbnormality.ModifiedBy,
          ModifiedIn = neonatalAbnormality.ModifiedIn,
          NeonatalId = neonatalAbnormality.NeonatalId,
          NewBornDetail = neonatalAbnormality.NewBornDetail,
          Other = neonatalAbnormality.Other,
          Abnormalities = neonatalAbnormality.Abnormalities

      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}