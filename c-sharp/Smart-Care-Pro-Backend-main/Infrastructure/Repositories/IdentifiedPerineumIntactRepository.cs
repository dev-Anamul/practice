using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Brian
 * Date created : 02.05.2023
 * Modified by  : Bella
 * Last modified: 14.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class IdentifiedPerineumIntactRepository : Repository<IdentifiedPerineumIntact>, IIdentifiedPerineumIntactRepository
    {
        /// <summary>
        /// Implementation of IIdentifiedPerineumIntactRepository interface.
        /// </summary>
        public IdentifiedPerineumIntactRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get IdentifiedPerineumIntact by key.
        /// </summary>
        /// <param name="key">Primary key of the table IdentifiedPerineumIntacts.</param>
        /// <returns>Returns a IdentifiedPerineumIntact if the key is matched.</returns>
        public async Task<IdentifiedPerineumIntact> GetIdentifiedPerineumIntactByKey(Guid key)
        {
            try
            {
                var identifiedPerineumIntact = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (identifiedPerineumIntact != null)
                {
                    identifiedPerineumIntact.ClinicianName = await context.UserAccounts.Where(x => x.Oid == identifiedPerineumIntact.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    identifiedPerineumIntact.FacilityName = await context.Facilities.Where(x => x.Oid == identifiedPerineumIntact.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    identifiedPerineumIntact.EncounterDate = await context.Encounters.Where(x => x.Oid == identifiedPerineumIntact.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return identifiedPerineumIntact;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPerineumIntacts.
        /// </summary>
        /// <returns>Returns a list of all Medical treatments.</returns>
        public async Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntacts()
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

        /// <summary>
        /// The method is used to get the list of IdentifiedPerineumIntact by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPerineumIntact by EncounterID.</returns>
        public async Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntactByEncounter(Guid encounterId)
        {
            try
            {
                return await context.IdentifiedPeriuneumIntacts.Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPerineumIntact => identifiedPerineumIntact.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPerineumIntact, encounter) => new IdentifiedPerineumIntact
                   {
                       EncounterId = identifiedPerineumIntact.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       DeliveryId = identifiedPerineumIntact.DeliveryId,
                       InteractionId = identifiedPerineumIntact.InteractionId,
                       EncounterType = identifiedPerineumIntact.EncounterType,
                       DateCreated = identifiedPerineumIntact.DateCreated,
                       DateModified = identifiedPerineumIntact.DateModified,
                       CreatedBy = identifiedPerineumIntact.CreatedBy,
                       CreatedIn = identifiedPerineumIntact.CreatedIn,
                       IsDeleted = identifiedPerineumIntact.IsDeleted,
                       IsSynced = identifiedPerineumIntact.IsSynced,
                       ModifiedBy = identifiedPerineumIntact.ModifiedBy,
                       ModifiedIn = identifiedPerineumIntact.ModifiedIn,
                       Perineums = identifiedPerineumIntact.Perineums,
                       TearDegree = identifiedPerineumIntact.TearDegree,
                       TearRepaired = identifiedPerineumIntact.TearRepaired,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPerineumIntact.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPerineumIntact.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of IdentifiedPerineumIntact by DeliveryId.
        /// </summary>
        /// <returns>Returns a list of all IdentifiedPerineumIntact by DeliveryId.</returns>
        public async Task<IEnumerable<IdentifiedPerineumIntact>> GetIdentifiedPerineumIntactByDelivery(Guid deliveryId)
        {
            try
            {
                return await context.IdentifiedPeriuneumIntacts.Where(p => p.IsDeleted == false && p.DeliveryId == deliveryId).AsNoTracking()
               .Join(
                   context.Encounters.AsNoTracking(),
                 identifiedPerineumIntact => identifiedPerineumIntact.EncounterId,
                   encounter => encounter.Oid,
                   (identifiedPerineumIntact, encounter) => new IdentifiedPerineumIntact
                   {
                       EncounterId = identifiedPerineumIntact.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       DeliveryId = identifiedPerineumIntact.DeliveryId,
                       InteractionId = identifiedPerineumIntact.InteractionId,
                       EncounterType = identifiedPerineumIntact.EncounterType,
                       DateCreated = identifiedPerineumIntact.DateCreated,
                       DateModified = identifiedPerineumIntact.DateModified,
                       CreatedBy = identifiedPerineumIntact.CreatedBy,
                       CreatedIn = identifiedPerineumIntact.CreatedIn,
                       IsDeleted = identifiedPerineumIntact.IsDeleted,
                       IsSynced = identifiedPerineumIntact.IsSynced,
                       ModifiedBy = identifiedPerineumIntact.ModifiedBy,
                       ModifiedIn = identifiedPerineumIntact.ModifiedIn,
                       Perineums = identifiedPerineumIntact.Perineums,
                       TearDegree = identifiedPerineumIntact.TearDegree,
                       TearRepaired = identifiedPerineumIntact.TearRepaired,
                       ClinicianName = context.UserAccounts.Where(x => x.Oid == identifiedPerineumIntact.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                       FacilityName = context.Facilities.Where(x => x.Oid == identifiedPerineumIntact.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}