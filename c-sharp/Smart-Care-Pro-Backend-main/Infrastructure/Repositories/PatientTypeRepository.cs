using Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class PatientTypeRepository : Repository<DFZPatientType>, IPatientTypeRepository
    {
        public PatientTypeRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get the list of patientType.
        /// </summary>
        /// <returns>Returns a list of all patientType.</returns>
        public async Task<IEnumerable<DFZPatientType>> GetPatientTypeByArmedForce(int armedForceId)
        {
            try
            {
                var patientType = await QueryAsync(p => p.IsDeleted == false && p.ArmedForceId == armedForceId, a => a.ArmedForceService);

                return patientType.OrderBy(p => p.Description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a patientType by key.
        /// </summary>
        /// <param name="key">Primary key of the table PatientType.</param>
        /// <returns>Returns a patientType if the key is matched.</returns>
        public async Task<DFZPatientType> GetPatientTypeByKey(int key)
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

        /// <summary>
        /// The method is used to get a patientType by patientType name.
        /// </summary>
        /// <param name="patientTypeName">Name of a patientType.</param>
        /// <returns>Returns a patientTyepe if the patientType name is matched.</returns>
        public async Task<DFZPatientType> GetPatientTypeByName(string patientTypeName)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Description.ToLower().Trim() == patientTypeName.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of patientType.
        /// </summary>
        /// <returns>Returns a list of all poatientType.</returns>
        public async Task<IEnumerable<DFZPatientType>> GetPatientTypes()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false, a => a.ArmedForceService);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}