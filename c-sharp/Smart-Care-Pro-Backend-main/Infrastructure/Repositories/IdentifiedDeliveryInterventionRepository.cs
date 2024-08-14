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
    public class IdentifiedDeliveryInterventionRepository : Repository<IdentifiedDeliveryIntervention>, IIdentifiedDeliveryInterventionRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedDeliveryInterventionRepository interface.
        /// </summary>
        public IdentifiedDeliveryInterventionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a IdentifiedDeliveryIntervention by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedDeliveryInterventions.</param>
        /// <returns>Returns a IdentifiedDeliveryIntervention if the key is matched.</returns>
        public async Task<IdentifiedDeliveryIntervention> GetIdentifiedDeliveryInterventionByKey(Guid key)
        {
            try
            {
                var identifiedDeliveryIntervention = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedDeliveryIntervention != null)
                {
                    identifiedDeliveryIntervention.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedDeliveryIntervention.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedDeliveryIntervention.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedDeliveryIntervention.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedDeliveryIntervention.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedDeliveryIntervention.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedDeliveryIntervention;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedDeliveryIntervention.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedDeliveryInterventions.</returns>
        public async Task<IEnumerable<IdentifiedDeliveryIntervention>> GetIdentifiedDeliveryInterventions()
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
        /// The method is used to get the list of IdentifiedDeliveryIntervention by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedDeliveryIntervention by DeliveryId.</returns>
        public async Task<IEnumerable<IdentifiedDeliveryIntervention>> GetIdentifiedDeliveryInterventionByDelivery(Guid deliveryId)
        {
            try
            {
                return await context.IdentifiedDeliveryInterventions.Where(p => p.IsDeleted == false && p.DeliveryId == deliveryId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                identifiedDeliveryIntervention => identifiedDeliveryIntervention.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedDeliveryIntervention, encounter) => new IdentifiedDeliveryIntervention
                   {
                       EncounterId = identifiedDeliveryIntervention.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       CreatedBy = identifiedDeliveryIntervention.CreatedBy,
                       ModifiedIn = identifiedDeliveryIntervention.ModifiedIn,
                       ModifiedBy = identifiedDeliveryIntervention.ModifiedBy,
                       IsSynced = identifiedDeliveryIntervention.IsSynced,
                       IsDeleted = identifiedDeliveryIntervention.IsDeleted,
                       CreatedIn = identifiedDeliveryIntervention.CreatedIn,
                       DateCreated = identifiedDeliveryIntervention.DateCreated,
                       DateModified = identifiedDeliveryIntervention.DateModified,
                       DeliveryId = identifiedDeliveryIntervention.DeliveryId,
                       EncounterType = identifiedDeliveryIntervention.EncounterType,
                       InteractionId = identifiedDeliveryIntervention.InteractionId,
                       Other = identifiedDeliveryIntervention.Other,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedDeliveryIntervention.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedDeliveryIntervention.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}