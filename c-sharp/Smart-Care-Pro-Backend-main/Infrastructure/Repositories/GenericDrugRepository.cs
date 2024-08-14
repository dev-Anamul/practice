using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class GenericDrugRepository : Repository<GenericDrug>, IGenericDrugRepository
   {
      /// <summary>
      /// Implementation of GenericMedicineRepository.
      /// </summary>
      public GenericDrugRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get the list of Generic Medicines.
      /// </summary>
      /// <returns>Returns a list of all Generic Medicines.</returns>
      public async Task<IEnumerable<GenericDrug>> GetGenericMedicines()
      {
         try
         {
            return await QueryAsync(g => g.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Generic Medicine by key.
      /// </summary>
      /// <param name="key">Primary key of the table Generic Medicines.</param>
      /// <returns>Returns a Generic Medicine if the key is matched.</returns>
      public async Task<GenericDrug> GetGenericMedicineByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(g => g.Oid == key && g.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Generic Medicine by Generic Name.
      /// </summary>
      /// <param name="genericName">Name of a Generic Medicine.</param>
      /// <returns>Returns a Generic Medicine if the Generic Name is matched.</returns>
      public async Task<GenericDrug> GetGenericMedicineByName(string genericName)
      {
         try
         {
            return await FirstOrDefaultAsync(g => g.Description.Trim() == genericName.Trim() && g.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the GenericMedicines by DrugUtilityID.
      /// </summary>
      /// <param name="drugUtilityId">DrugUtilityID of the table GenericMedicines.</param>
      /// <returns>Returns a GenericMedicines if the DrugUtilityID is matched.</returns>
      public async Task<IEnumerable<GenericDrug>> GetGenericMedicinesByDrugUtilityID(int drugUtilityId)
      {
         try
         {
            List<GenericDrug> GenericDrugList = new List<GenericDrug>();

            var drugDefinitionData = context.GeneralDrugDefinitions.Where(x => x.DrugUtilityId == drugUtilityId && x.IsDeleted == false);

            foreach (GeneralDrugDefinition drugDefinition in drugDefinitionData)
            {
               GenericDrug genericDrug = new GenericDrug();

               genericDrug = await GetGenericMedicineByKey(drugDefinition.GenericDrugId);

               GenericDrugList.Add(genericDrug);
            }

            return GenericDrugList;
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the GenericMedicines by DrugSubClassID.
      /// </summary>
      /// <param name="drugSubClassId">DrugSubClassID of the table GenericMedicines.</param>
      /// <returns>Returns a GenericMedicines if the DrugSubClassID is matched.</returns>
      public async Task<IEnumerable<GenericDrug>> GetGenericMedicinesByDrugSubClassID(int drugSubClassId)
      {
         try
         {
            return await QueryAsync(g => g.IsDeleted == false && g.SubclassId == drugSubClassId);
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}