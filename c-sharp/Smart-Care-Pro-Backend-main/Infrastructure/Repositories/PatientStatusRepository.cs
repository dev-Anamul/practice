using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Infrastructure.Repositories
{
    public class PatientStatusRepository : Repository<PatientStatus>, IPatientStatusRepository
    {
        public PatientStatusRepository(DataContext context) : base(context)
        {
        }

        public async Task<PatientStatus> GetPatientStatusByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PatientStatus>> PatientStatuses()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<PatientStatus>> PatientStatuses(int page, int pageSize)
        {
            try
            {
                return await LoadListWithChildAsync<PatientStatus>(p => p.IsDeleted == false, page, pageSize, orderBy: d => d.OrderByDescending(y => y.StatusDate));

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int PatientStatusesTotalCount()
        {
            return context.PatientStatuses.Where(x => x.IsDeleted == false).Count();
        }
        public async Task<IEnumerable<PatientStatus>> GetPatientStatusbyArtRegisterId(Guid artRegisterId)
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false && b.ARTRegisterId == artRegisterId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<PatientStatus>> GetPatientStatusbyArtRegisterId(Guid artRegisterId, int page, int pageSize)
        {
            try
            {
                return await LoadListWithChildAsync<PatientStatus>(p => p.IsDeleted == false && p.ARTRegisterId == artRegisterId, page, pageSize, orderBy: d => d.OrderByDescending(y => y.StatusDate));

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetPatientStatusbyArtRegisterIdTotalCount(Guid artRegisterId)
        {
            return context.PatientStatuses.Where(x => x.IsDeleted == false && x.ARTRegisterId == artRegisterId).Count();
        }
    }
}