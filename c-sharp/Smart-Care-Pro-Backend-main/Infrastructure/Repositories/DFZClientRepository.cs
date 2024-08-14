using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class DFZClientRepository : Repository<DFZClient>, IDFZClientRepository
    {
        public DFZClientRepository(DataContext context) : base(context)
        {

        }

        public async Task<DFZClient> GetDFZByHospitalNo(string hospitalNo)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.HospitalNo.ToLower().Trim() == hospitalNo.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DFZClient> GetDFZClientByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(x => x.Oid == key && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<DFZClient> GetDFZByServiceNo(string serviceNo)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.ServiceNo.ToLower().Trim() == serviceNo.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}