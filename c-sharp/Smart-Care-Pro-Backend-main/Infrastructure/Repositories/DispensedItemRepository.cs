using Domain.Entities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class DispensedItemRepository : Repository<DispensedItem>
    {
        public DispensedItemRepository(DataContext context) : base(context)
        {

        }
    }
}