using Domain.Entities;

namespace Infrastructure.Contracts
{
   public interface IProteinsRepository : IRepository<Protein>
   {
      Protein UpdateProtein(Protein protein);
   }
}