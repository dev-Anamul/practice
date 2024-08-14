using Domain.Entities;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Stephan
 * Date created : 02.1.2024
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class DFZClientDependentRepository : Repository<DFZDependent>, IDFZClientDependentRepository
    {
        public DFZClientDependentRepository(DataContext context) : base(context)
        {

        }
        public async Task<IEnumerable<DFZDependent>> GetDFZDependentsByPrincipleId(Guid key)
        {
            try
            {
                return await context.DFZDependents.Where(x => x.PrincipalId == key && x.IsDeleted == false).Include(x => x.Client).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DFZDependent> GetDFZDependentByDFZClientId(Guid dfzClientId)
        {
            try
            {
                return await context.DFZDependents.Where(x => x.DependentClientId == dfzClientId && x.IsDeleted == false).Include(x => x.Client).FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<DFZDependent> GetDFZDependentByKey(Guid key)
        {
            try
            {
                return await context.DFZDependents.Where(x => x.Oid == key && x.IsDeleted == false).Include(x => x.Client).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DFZDependent> GetDependentByHospitalNo(string hospitalNo)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.HospitalNo == hospitalNo && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}