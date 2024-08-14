using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IAcetonesRepository : IRepository<Acetone>
   {
      Acetone UpdateAcetone(Acetone acetone);
   }
}