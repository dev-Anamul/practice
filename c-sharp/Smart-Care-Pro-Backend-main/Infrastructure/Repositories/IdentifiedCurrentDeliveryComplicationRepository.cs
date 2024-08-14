using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 01.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedCurrentDeliveryComplicationRepository : Repository<IdentifiedCurrentDeliveryComplication>, IIdentifiedCurrentDeliveryComplicationRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedCurrentDeliveryComplicationRepository interface.
        /// </summary>
        public IdentifiedCurrentDeliveryComplicationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedCurrentDeliveryComplication by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedCurrentDeliveryComplications.</param>
        /// <returns>Returns a IdentifiedCurrentDeliveryComplication if the key is matched.</returns>
        public async Task<IdentifiedCurrentDeliveryComplication> GetIdentifiedCurrentDeliveryComplicationByKey(Guid key)
        {
            try
            {
                var identifiedCurrentDeliveryComplication = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedCurrentDeliveryComplication != null)
                {
                    identifiedCurrentDeliveryComplication.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedCurrentDeliveryComplication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedCurrentDeliveryComplication.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedCurrentDeliveryComplication.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedCurrentDeliveryComplication.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedCurrentDeliveryComplication.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedCurrentDeliveryComplication;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedCurrentDeliveryComplication.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedCurrentDeliveryComplications.</returns>
        public async Task<IEnumerable<IdentifiedCurrentDeliveryComplication>> GetIdentifiedCurrentDeliveryComplications()
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
        /// The method is used to get the list of IdentifiedCurrentDeliveryComplication by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedCurrentDeliveryComplication by DeliveryId.</returns>
        public async Task<IEnumerable<IdentifiedCurrentDeliveryComplication>> GetIdentifiedCurrentDeliveryComplicationByDelivery(Guid deliveryId)
        {
            try
            {
                return await context.IdentifiedCurrentDeliveryComplications.Where(p => p.IsDeleted == false && p.DeliveryId == deliveryId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedCurrentDeliveryComplications => identifiedCurrentDeliveryComplications.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedCurrentDeliveryComplication, encounter) => new IdentifiedCurrentDeliveryComplication
                   {
                       EncounterId = identifiedCurrentDeliveryComplication.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       CreatedIn = identifiedCurrentDeliveryComplication.CreatedIn,
                       DeliveryId = identifiedCurrentDeliveryComplication.DeliveryId,
                       ModifiedIn = identifiedCurrentDeliveryComplication.ModifiedIn,
                       ModifiedBy = identifiedCurrentDeliveryComplication.ModifiedBy,
                       IsSynced = identifiedCurrentDeliveryComplication.IsSynced,
                       IsDeleted = identifiedCurrentDeliveryComplication.IsDeleted,
                       Complications = identifiedCurrentDeliveryComplication.Complications,
                       CreatedBy = identifiedCurrentDeliveryComplication.CreatedBy,
                       DateCreated = identifiedCurrentDeliveryComplication.DateCreated,
                       DateModified = identifiedCurrentDeliveryComplication.DateModified,
                       EncounterType = identifiedCurrentDeliveryComplication.EncounterType,
                       InteractionId = identifiedCurrentDeliveryComplication.InteractionId,
                       Other = identifiedCurrentDeliveryComplication.Other,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedCurrentDeliveryComplication.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedCurrentDeliveryComplication.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}