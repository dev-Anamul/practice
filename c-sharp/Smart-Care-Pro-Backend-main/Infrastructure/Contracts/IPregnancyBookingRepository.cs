using Domain.Entities;

/*
 * Created by   : Biplob Roy
 * Date created : 18.04.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IPregnancyBookingRepository : IRepository<PregnancyBooking>
    {
        /// <summary>
        /// The method is used to get a PregnancyBooking by key.
        /// </summary>
        /// <param name="key">Primary key of the table PregnancyBookings.</param>
        /// <returns>Returns a PregnancyBooking if the key is matched.</returns>
        public Task<PregnancyBooking> GetPregnancyBookingByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of PregnancyBooking.
        /// </summary>
        /// <returns>Returns a list of all PregnancyBooking.</returns>
        public Task<IEnumerable<PregnancyBooking>> GetPregnancyBookings();

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns>Returns a PregnancyBooking if the ClientID is matched.</returns>
        public Task<IEnumerable<PregnancyBooking>> GetPregnancyBookingByClient(Guid ClientID);

        /// <summary>
        /// The method is used to get the list of PregnancyBooking by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all PregnancyBooking by EncounterID.</returns>
        public Task<IEnumerable<PregnancyBooking>> GetPregnancyBookingByEncounter(Guid EncounterID);
    }
}