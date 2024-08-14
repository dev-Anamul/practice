using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 01.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PPHTreatmentRepository : Repository<PPHTreatment>, IPPHTreatmentRepository
    {
        /// <summary>
        /// Implementation of IPPHTreatmentRepository interface.
        /// </summary>
        public PPHTreatmentRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a PPHTreatment by key.
        /// </summary>
        /// <param name="key">Primary key of the table PPHTreatments.</param>
        /// <returns>Returns a PPHTreatment if the key is matched.</returns>
        public async Task<PPHTreatment> GetPPHTreatmentByKey(Guid key)
        {
            try
            {
                var pPHTreatment = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (pPHTreatment != null)
                    pPHTreatment.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pPHTreatment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PPHTreatment.
        /// </summary>
        /// <returns>Returns a list of all PPHTreatments.</returns>
        public async Task<IEnumerable<PPHTreatment>> GetPPHTreatments()
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
        /// The method is used to get the list of PPHTreatment by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all PPHTreatment by DeliveryId.</returns>
        public async Task<IEnumerable<PPHTreatment>> GetPPHTreatmentByDelivery(Guid DeliveryId)
        {
            try
            {
                return await context.PPHTreatments.Include(p => p.IdentifiedPPHTreatments).AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == DeliveryId)
             .Join(
                 context.Encounters.AsNoTracking(),
                 pPHTreatment => pPHTreatment.EncounterId,
                 encounter => encounter.Oid,
                 (pPHTreatment, encounter) => new PPHTreatment
                 {
                     EncounterId = pPHTreatment.EncounterId,
                     EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                     DeliveryId = pPHTreatment.DeliveryId,
                     FluidsGiven = pPHTreatment.FluidsGiven,
                     IsDeleted = pPHTreatment.IsDeleted,
                     BloodAmount = pPHTreatment.BloodAmount,
                     BloodType = pPHTreatment.BloodType,
                     CreatedBy = pPHTreatment.CreatedBy,
                     CreatedIn = pPHTreatment.CreatedIn,
                     DateCreated = pPHTreatment.DateCreated,
                     DateModified = pPHTreatment.DateModified,
                     EncounterType = pPHTreatment.EncounterType,
                     FluidAmount = pPHTreatment.FluidAmount,
                     IdentifiedPPHTreatments = pPHTreatment.IdentifiedPPHTreatments.ToList(),
                     InteractionId = pPHTreatment.InteractionId,
                     IsSynced = pPHTreatment.IsSynced,
                     ModifiedBy = pPHTreatment.ModifiedBy,
                     ModifiedIn = pPHTreatment.ModifiedIn,
                     PPHSugery = pPHTreatment.PPHSugery,


                 }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PPHTreatment by EncounterID.
        /// </summary>
        /// <param name="EncounterID"></param>
        /// <returns>Returns a PPHTreatment if the EncounterID is matched.</returns>
        public async Task<IEnumerable<PPHTreatment>> GetPPHTreatmentByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PPHTreatments.Include(p => p.IdentifiedPPHTreatments).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
              .Join(
                  context.Encounters.AsNoTracking(),
                  pPHTreatment => pPHTreatment.EncounterId,
                  encounter => encounter.Oid,
                  (pPHTreatment, encounter) => new PPHTreatment
                  {
                      EncounterId = pPHTreatment.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      DeliveryId = pPHTreatment.DeliveryId,
                      FluidsGiven = pPHTreatment.FluidsGiven,
                      IsDeleted = pPHTreatment.IsDeleted,
                      BloodAmount = pPHTreatment.BloodAmount,
                      BloodType = pPHTreatment.BloodType,
                      CreatedBy = pPHTreatment.CreatedBy,
                      CreatedIn = pPHTreatment.CreatedIn,
                      DateCreated = pPHTreatment.DateCreated,
                      DateModified = pPHTreatment.DateModified,
                      EncounterType = pPHTreatment.EncounterType,
                      FluidAmount = pPHTreatment.FluidAmount,
                      InteractionId = pPHTreatment.InteractionId,
                      IsSynced = pPHTreatment.IsSynced,
                      ModifiedBy = pPHTreatment.ModifiedBy,
                      ModifiedIn = pPHTreatment.ModifiedIn,
                      PPHSugery = pPHTreatment.PPHSugery,


                  }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}