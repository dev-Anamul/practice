using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tariqul Islam
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PregnencyBookingRepository : Repository<PregnancyBooking>, IPregnencyBookingRepository
    {
        public PregnencyBookingRepository(DataContext context) : base(context)
        {

        }

        public async Task<PregnancyBooking> GetPregnancyBookingByKey(Guid key)
        {
            try
            {
                var pregnancyBooking = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (pregnancyBooking != null)
                    pregnancyBooking.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return pregnancyBooking;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PregnancyBooking>> GetPregnancyBookings()
        {
            try
            {
                return await LoadListWithChildAsync<PregnancyBooking>(p => p.IsDeleted == false, m => m.IdentifiedPregnencyConfirmations);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}