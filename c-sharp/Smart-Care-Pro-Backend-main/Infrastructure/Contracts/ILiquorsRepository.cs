using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12.09.2022
 * Modified by  : 
 * Last modified:  
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface ILiquorsRepository : IRepository<Liquor>
    {
        Liquor UpdateLiquor(Liquor liquor);
    }
}