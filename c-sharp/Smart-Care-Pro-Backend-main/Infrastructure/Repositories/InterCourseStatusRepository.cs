using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InterCourseStatusRepository : Repository<InterCourseStatus>, IInterCourseStatusRepository
    {
        public InterCourseStatusRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<InterCourseStatus>> GetInterCourseStatus()
        {
            try
            {
                return await QueryAsync(x => x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<InterCourseStatus> GetInterCourseStatusByKey(int key)
        {
            return await FirstOrDefaultAsync(x => x.Oid == key && x.IsDeleted == false);
        }
    }
}
