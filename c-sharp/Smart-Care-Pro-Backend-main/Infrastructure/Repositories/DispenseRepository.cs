using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by   : Md Sayem
 * Date created : 05.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DispenseRepository : Repository<Dispense>, IDispenseRepository
    {
        public DispenseRepository(DataContext context) : base(context)
        {

        }
    }
}