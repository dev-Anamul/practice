using Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class GeneralDrugDefinationRepository : Repository<GeneralDrugDefinition>, IGeneralDrugDefinationRepository
    {
        public GeneralDrugDefinationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get an General DrugDefination by key.
        /// </summary>
        /// <param name="key">Primary key of the table General DrugDefination.</param>
        /// <returns>Returns an General DrugDefination if the key is matched.</returns>
        public async Task<GeneralDrugDefinition> GetGeneralDrugDefinitionByKey(int key)
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
        /// The method is used to get the list of General DrugDefination.
        /// </summary>
        /// <returns>Returns a list of all General DrugDefination.</returns>
        public async Task<IEnumerable<GeneralDrugDefinition>> GetGeneralDrugDefinition()
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
        /// The method is used to get an General DrugDefination by General DrugDefination Description.
        /// </summary>
        /// <param name="generalDrugDefinition">Name of a GeneralDrugDefination.</param>
        /// <returns>Returns an GeneralDrugDefination if the General DrugDefination description is matched.</returns>
        public async Task<GeneralDrugDefinition> GetGeneralDrugDefinitionByName(string generalDrugDefinition)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == generalDrugDefinition.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
