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
    public class ThirdStageDeliveryRepository : Repository<ThirdStageDelivery>, IThirdStageDeliveryRepository
    {
        /// <summary>
        /// Implementation of IThirdStageDeliveryRepository interface.
        /// </summary>
        public ThirdStageDeliveryRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a ThirdStageDelivery by key.
        /// </summary>
        /// <param name="key">Primary key of the table ThirdStageDeliveries.</param>
        /// <returns>Returns a ThirdStageDelivery if the key is matched.</returns>
        public async Task<ThirdStageDelivery> GetThirdStageDeliveryByKey(Guid key)
        {
            try
            {
                var thirdStageDelivery = await FirstOrDefaultAsync(s => s.InteractionId == key && s.IsDeleted == false);

                if (thirdStageDelivery != null)
                    thirdStageDelivery.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return thirdStageDelivery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ThirdStageDelivery.
        /// </summary>
        /// <returns>Returns a list of all ThirdStageDeliveries.</returns>
        public async Task<IEnumerable<ThirdStageDelivery>> GetThirdStageDeliveries()
        {
            try
            {
                return await QueryAsync(t => t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of ThirdStageDelivery by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all ThirdStageDelivery by DeliveryId.</returns>
        public async Task<IEnumerable<ThirdStageDelivery>> GetThirdStageDeliveryByDelivery(Guid DeliveryId)
        {
            try
            {
                return await context.ThirdStageDeliveries.AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == DeliveryId)
         .Join(
             context.Encounters.AsNoTracking(),
             thirdStageDelivery => thirdStageDelivery.EncounterId,
             encounter => encounter.Oid,
             (thirdStageDelivery, encounter) => new ThirdStageDelivery
             {
                 EncounterId = thirdStageDelivery.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 IsUterineRapture = thirdStageDelivery.IsUterineRapture,
                 ModifiedIn = thirdStageDelivery.ModifiedIn,
                 ModifiedBy = thirdStageDelivery.ModifiedBy,
                 IsSynced = thirdStageDelivery.IsSynced,
                 IsDeleted = thirdStageDelivery.IsDeleted,
                 InteractionId = thirdStageDelivery.InteractionId,
                 ActiveManagement = thirdStageDelivery.ActiveManagement,
                 BloodLoss = thirdStageDelivery.BloodLoss,
                 CreatedBy = thirdStageDelivery.CreatedBy,
                 CreatedIn = thirdStageDelivery.CreatedIn,
                 DateCreated = thirdStageDelivery.DateCreated,
                 DateModified = thirdStageDelivery.DateModified,
                 DeliveryId = thirdStageDelivery.DeliveryId,
                 EncounterType = thirdStageDelivery.EncounterType,
                 IsAbnormalPlacemental = thirdStageDelivery.IsAbnormalPlacemental,
                 IsAbnormalPlacentation = thirdStageDelivery.IsAbnormalPlacentation,
                 IsAnesthesia = thirdStageDelivery.IsAnesthesia,
                 IsBirthTrauma = thirdStageDelivery.IsBirthTrauma,
                 IsCoagulationDisorder = thirdStageDelivery.IsCoagulationDisorder,
                 IsFetalMacrosomia = thirdStageDelivery.IsFetalMacrosomia,
                 IsHemophilia = thirdStageDelivery.IsHemophilia,
                 IsLatrogenicInjury = thirdStageDelivery.IsLatrogenicInjury,
                 IsMalpresentationOfFetus = thirdStageDelivery.IsMalpresentationOfFetus,
                 IsMultiplePregnancy = thirdStageDelivery.IsMultiplePregnancy,
                 IsPreviousUterineInversion = thirdStageDelivery.IsPreviousUterineInversion,
                 IsProlongedOxytocinUse = thirdStageDelivery.IsProlongedOxytocinUse,
                 IsRetainedPlacenta = thirdStageDelivery.IsRetainedPlacenta,
                 IsRetainedProductOfConception = thirdStageDelivery.IsRetainedProductOfConception,
                 IsUncontrolledCordContraction = thirdStageDelivery.IsUncontrolledCordContraction,
                 IsUterineAtony = thirdStageDelivery.IsUterineAtony,
                 IsUterineInversion = thirdStageDelivery.IsUterineInversion,
                 IsUterineLeiomyoma = thirdStageDelivery.IsUterineLeiomyoma,
                 IsVelamentousCordInsertion = thirdStageDelivery.IsVelamentousCordInsertion,
                 IsVonWillebrand = thirdStageDelivery.IsVonWillebrand,
                 Others = thirdStageDelivery.Others,
                 PPH = thirdStageDelivery.PPH
             }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}