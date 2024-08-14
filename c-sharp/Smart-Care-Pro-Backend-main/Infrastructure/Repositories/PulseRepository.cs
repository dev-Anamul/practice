using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PulseRepository : Repository<Pulse>, IPulseRepository
    {
        private readonly DataContext context;

        public PulseRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Pulse UpdatePulse(Pulse pulse)
        {
            try
            {
                var existingInDb = context.Pulses
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(pulse.PartographId) &&
                        i.PulseTime.Equals(pulse.PulseTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Pulse()
                    {
                        PartographId = pulse.PartographId,
                        PulseDetails = pulse.PulseDetails,
                        PulseTime = pulse.PulseTime,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Pulses.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.PulseDetails != pulse.PulseDetails)
                    {
                        existingInDb.PulseDetails = pulse.PulseDetails;
                        existingInDb.IsDeleted = false;
                        existingInDb.IsSynced = false;
                        context.Entry(existingInDb).State = EntityState.Modified;
                    }
                }

                return existingInDb;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}