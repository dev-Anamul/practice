using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TBIdentificationMethodRepository : Repository<TBIdentificationMethod>, ITBIdentificationMethodRepository
    {
        public TBIdentificationMethodRepository(DataContext context) : base(context)
        {

        }

        public async Task<TBIdentificationMethod> GetTBIdentificationMethodByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TBIdentificationMethod> GetTBIdentificationMethodByName(string tBIdentificationMethod)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Description.ToLower().Trim() == tBIdentificationMethod.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TBIdentificationMethod>> GetTBIdentificationMethods()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}