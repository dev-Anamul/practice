using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ICervixRepository : IRepository<Cervix>
   {
      Cervix UpdateCervix(Cervix cervix);
   }
}