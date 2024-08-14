using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IMouldingsRepository : IRepository<Moulding>
    {
        Moulding UpdateMoulding(Moulding moulding);
    }
}