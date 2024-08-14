using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IContractionsRepository : IRepository<Contraction>
    {
        Contraction UpdateContraction(Contraction contraction);
    }
}