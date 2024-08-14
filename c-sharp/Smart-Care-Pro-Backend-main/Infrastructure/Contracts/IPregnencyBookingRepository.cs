using Domain.Entities;

namespace Infrastructure.Contracts
{
    public interface IPregnencyBookingRepository : IRepository<PregnancyBooking>
    {
        /// <summary>
        /// The method is used to get a birth history by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthDetails.</param>
        /// <returns>Returns a birth history if the key is matched.</returns>
        public Task<PregnancyBooking> GetPregnancyBookingByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<PregnancyBooking>> GetPregnancyBookings();

    }
}