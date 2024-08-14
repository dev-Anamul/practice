using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IOxytocinRepository : IRepository<Oxytocin>
    {
        Oxytocin UpdateOxytocin(Oxytocin oxytocin);
    }
}