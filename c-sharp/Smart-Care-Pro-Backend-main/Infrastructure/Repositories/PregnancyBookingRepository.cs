using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Biplob Roy
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PregnancyBookingRepository : Repository<PregnancyBooking>, IPregnancyBookingRepository
    {
        public PregnancyBookingRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PregnancyBooking if the ClientID is matched.</returns>
        public async Task<IEnumerable<PregnancyBooking>> GetPregnancyBookingByClient(Guid ClientID)
        {
            try
            {
                return await context.PregnancyBookings.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == ClientID)
              .Join(
                  context.Encounters.AsNoTracking(),
                  pregnancyBooking => pregnancyBooking.EncounterId,
                  encounter => encounter.Oid,
                  (pregnancyBooking, encounter) => new PregnancyBooking
                  {
                      EncounterId = pregnancyBooking.EncounterId,
                      EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                      ClientId = pregnancyBooking.ClientId,
                      CreatedBy = pregnancyBooking.CreatedBy,
                      CreatedIn = pregnancyBooking.CreatedIn,
                      DateCreated = pregnancyBooking.DateCreated,
                      DateModified = pregnancyBooking.DateModified,
                      EncounterType = pregnancyBooking.EncounterType,
                      InteractionId = pregnancyBooking.InteractionId,
                      IsDeleted = pregnancyBooking.IsDeleted,
                      IsSynced = pregnancyBooking.IsSynced,
                      ModifiedBy = pregnancyBooking.ModifiedBy,
                      ModifiedIn = pregnancyBooking.ModifiedIn,
                      PregnancyConfirmedDate = pregnancyBooking.PregnancyConfirmedDate,
                      PregnancyConfirmedFacilityId = pregnancyBooking.PregnancyConfirmedFacilityId,
                      QuickeningDate = pregnancyBooking.QuickeningDate,
                      QuickeningWeeks = pregnancyBooking.QuickeningWeeks,

                  }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PregnancyBooking by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PregnancyBooking by EncounterID.</returns>
        public async Task<IEnumerable<PregnancyBooking>> GetPregnancyBookingByEncounter(Guid EncounterID)
        {
            try
            {
                return await context.PregnancyBookings.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == EncounterID)
               .Join(
                   context.Encounters.AsNoTracking(),
                   pregnancyBooking => pregnancyBooking.EncounterId,
                   encounter => encounter.Oid,
                   (pregnancyBooking, encounter) => new PregnancyBooking
                   {
                       EncounterId = pregnancyBooking.EncounterId,
                       EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                       ClientId = pregnancyBooking.ClientId,
                       CreatedBy = pregnancyBooking.CreatedBy,
                       CreatedIn = pregnancyBooking.CreatedIn,
                       DateCreated = pregnancyBooking.DateCreated,
                       DateModified = pregnancyBooking.DateModified,
                       EncounterType = pregnancyBooking.EncounterType,
                       InteractionId = pregnancyBooking.InteractionId,
                       IsDeleted = pregnancyBooking.IsDeleted,
                       IsSynced = pregnancyBooking.IsSynced,
                       ModifiedBy = pregnancyBooking.ModifiedBy,
                       ModifiedIn = pregnancyBooking.ModifiedIn,
                       PregnancyConfirmedDate = pregnancyBooking.PregnancyConfirmedDate,
                       PregnancyConfirmedFacilityId = pregnancyBooking.PregnancyConfirmedFacilityId,
                       QuickeningDate = pregnancyBooking.QuickeningDate,
                       QuickeningWeeks = pregnancyBooking.QuickeningWeeks,

                   }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a PregnancyBooking by key.
        /// </summary>
        /// <param name="key">Primary key of the table PregnancyBookings.</param>
        /// <returns>Returns a PregnancyBooking if the key is matched.</returns>
        public async Task<PregnancyBooking> GetPregnancyBookingByKey(Guid key)
        {
            try
            {
//                var pregnancyBooking = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);
                var pregnancyBooking = await LoadWithChildAsync<PregnancyBooking>(p => p.InteractionId == key && p.IsDeleted == false,f=>f.Facility);

                if (pregnancyBooking != null)
                    pregnancyBooking.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pregnancyBooking;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of PregnancyBookings.
        /// </summary>
        /// <returns>Returns a list of all PregnancyBooking.</returns>
        public async Task<IEnumerable<PregnancyBooking>> GetPregnancyBookings()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false, g => g.IdentifiedPregnencyConfirmations);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}