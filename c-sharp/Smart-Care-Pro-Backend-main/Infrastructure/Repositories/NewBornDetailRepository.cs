using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

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
    public class NewBornDetailRepository : Repository<NewBornDetail>, INewBornDetailRepository
    {
        /// <summary>
        /// Implementation of INewBornDetailRepository interface.
        /// </summary>
        public NewBornDetailRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a NewBornDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table NewBornDetails.</param>
        /// <returns>Returns a NewBornDetail if the key is matched.</returns>
        public async Task<NewBornDetail> GetNewBornDetailByKey(Guid key)
        {
            try
            {
                var newBornDetail = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (newBornDetail != null)
                    newBornDetail.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return newBornDetail;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NewBornDetail.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetails.</returns>
        public async Task<IEnumerable<NewBornDetail>> GetNewBornDetails()
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
        /// The method is used to get the list of NewBornDetail by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetail by EncounterID.</returns>
        public async Task<IEnumerable<NewBornDetail>> GetNewBornDetailByEncounter(Guid encounterId)
        {
            try
            {
                return await context.NewBornDetails.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
      .Join(
          context.Encounters.AsNoTracking(),
          newBornDetail => newBornDetail.EncounterId,
          encounter => encounter.Oid,
          (newBornDetail, encounter) => new NewBornDetail
          {
              EncounterId = newBornDetail.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              Other = newBornDetail.Other,
              InteractionId = newBornDetail.InteractionId,
              CreatedIn = newBornDetail.CreatedIn,
              CreatedBy = newBornDetail.CreatedBy,
              BirthHeight = newBornDetail.BirthHeight,
              BirthWeight = newBornDetail.BirthWeight,
              BreechId = newBornDetail.BreechId,
              CauseOfStillbirthId = newBornDetail.CauseOfStillbirthId,
              DateCreated = newBornDetail.DateCreated,
              DateModified = newBornDetail.DateModified,
              DeliveredBy = newBornDetail.DeliveredBy,
              DateOfDelivery = newBornDetail.DateOfDelivery,
              DeliveryId = newBornDetail.DeliveryId,
              EncounterType = newBornDetail.EncounterType,
              Gender = newBornDetail.Gender,
              IsDeleted = newBornDetail.IsDeleted,
              IsSynced = newBornDetail.IsSynced,
              ModeOfDelivery = newBornDetail.ModeOfDelivery,
              ModeOfDeliveryId = newBornDetail.ModeOfDeliveryId,
              ModifiedBy = newBornDetail.ModifiedBy,
              ModifiedIn = newBornDetail.ModifiedIn,
              PresentingPartId = newBornDetail.PresentingPartId,
              TimeOfDelivery = newBornDetail.TimeOfDelivery,
              NeonatalBirthOutcomeId = newBornDetail.NeonatalBirthOutcomeId,


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of NewBornDetail by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all NewBornDetail by DeliveryId.</returns>
        public async Task<IEnumerable<NewBornDetail>> GetNewBornDetailByDelivery(Guid deliveryId)
        {
            try
            {
                return await context.NewBornDetails.AsNoTracking().Where(p => p.IsDeleted == false && p.DeliveryId == deliveryId)
      .Join(
          context.Encounters.AsNoTracking(),
          newBornDetail => newBornDetail.EncounterId,
          encounter => encounter.Oid,
          (newBornDetail, encounter) => new NewBornDetail
          {
              EncounterId = newBornDetail.EncounterId,
              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
              Other = newBornDetail.Other,
              InteractionId = newBornDetail.InteractionId,
              CreatedIn = newBornDetail.CreatedIn,
              CreatedBy = newBornDetail.CreatedBy,
              BirthHeight = newBornDetail.BirthHeight,
              BirthWeight = newBornDetail.BirthWeight,
              BreechId = newBornDetail.BreechId,
              CauseOfStillbirthId = newBornDetail.CauseOfStillbirthId,
              DateCreated = newBornDetail.DateCreated,
              DateModified = newBornDetail.DateModified,
              DeliveredBy = newBornDetail.DeliveredBy,
              DateOfDelivery = newBornDetail.DateOfDelivery,
              DeliveryId = newBornDetail.DeliveryId,
              EncounterType = newBornDetail.EncounterType,
              Gender = newBornDetail.Gender,
              IsDeleted = newBornDetail.IsDeleted,
              IsSynced = newBornDetail.IsSynced,
              ModeOfDelivery = newBornDetail.ModeOfDelivery,
              ModeOfDeliveryId = newBornDetail.ModeOfDeliveryId,
              ModifiedBy = newBornDetail.ModifiedBy,
              ModifiedIn = newBornDetail.ModifiedIn,
              PresentingPartId = newBornDetail.PresentingPartId,
              TimeOfDelivery = newBornDetail.TimeOfDelivery,
              NeonatalBirthOutcomeId = newBornDetail.NeonatalBirthOutcomeId,


          }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}