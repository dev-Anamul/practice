using Domain.Entities;

namespace Infrastructure.Contracts
{
   public interface IPartographRepository : IRepository<Partograph>
   {
      Task<Guid> GetPartographIdByAdmissionId(Guid aid);

      Task<Partograph> GetPartographByEncounter(Guid encounterId);

      Task< Partograph> GetPartographByClient(Guid clientId);
    }
}