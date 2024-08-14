using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IInterCourseStatusRepository : IRepository<InterCourseStatus>
    {
        public Task<IEnumerable<InterCourseStatus>> GetInterCourseStatus();
        /// <summary>
        /// The method is used to get a InterCourseStatus by key.
        /// </summary>
        /// <param name="key">Primary key of the table InterCourseStatus.</param>
        /// <returns>Returns a InterCourseStatus if the key is matched.</returns>
        public Task<InterCourseStatus> GetInterCourseStatusByKey(int key);
    }
}
