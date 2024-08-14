using Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class MedicineBrandRepository :Repository<MedicineBrand>, IMedicineBrandRepository
    {
        public MedicineBrandRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get Medicine Brand  by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicineBrand.</param>
        /// <returns>Returns an Medicine Brand if the key is matched.</returns>
        public async Task<MedicineBrand> GetMedicineBrandByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(n => n.Oid == key && n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Medicine Brand.
        /// </summary>
        /// <returns>Returns a list of all Medicine Brand.</returns>
        public async Task<IEnumerable<MedicineBrand>> GetMedicineBrand()
        {
            try
            {
                return await QueryAsync(n => n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an MedicineBrand by MedicineBrand name.
        /// </summary>
        /// <param name="medicineBrand">Name of an MedicineBrand.</param>
        /// <returns>Returns an MedicineBrand if the MedicineBrand name is matched.</returns>
        public async Task<MedicineBrand> GetMedicineBrandByName(string medicineBrand)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == medicineBrand.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

   
}
