using Domain.Entities;
using Infrastructure.Contracts;


namespace Infrastructure.Repositories
{
    public class MedicineManufactureRepository : Repository<MedicineManufacturer>, IMedicineManufactureRepository
    {
        public MedicineManufactureRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get Medicine Manufacturer  by key.
        /// </summary>
        /// <param name="key">Primary key of the table MedicineManufacturer.</param>
        /// <returns>Returns an Medicine Manufacturer if the key is matched.</returns>
        public async Task<MedicineManufacturer> GetMedicineManufacturerByKey(int key)
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
        /// The method is used to get the list of Medicine Manufacturer.
        /// </summary>
        /// <returns>Returns a list of all Medicine Manufacturer.</returns>
        public async Task<IEnumerable<MedicineManufacturer>> GetMedicineManufacturer()
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
        /// The method is used to get an MedicineManufacturer by MedicineManufacturer name.
        /// </summary>
        /// <param name="medicineManufacturer">Name of an Medicine Manufacturer.</param>
        /// <returns>Returns an MedicineManufacturer if the MedicineManufacturer name is matched.</returns>
        public async Task<MedicineManufacturer> GetMedicineManufacturerByName(string medicineManufacturer)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == medicineManufacturer.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
