using Domain.Entities;

namespace Infrastructure.Contracts
{
   public interface IPulseRepository : IRepository<Pulse>
   {
      Pulse UpdatePulse(Pulse pulse);
   }
}