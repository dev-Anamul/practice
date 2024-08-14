using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IGenericDrugRepository : IRepository<GenericDrug>
   {
      /// <summary>
      /// The method is used to get the list of Generic Medicines.
      /// </summary>
      /// <returns>Returns a list of all Generic Medicines.</returns>
      public Task<IEnumerable<GenericDrug>> GetGenericMedicines();

      /// <summary>
      /// The method is used to get a Generic Medicine by key.
      /// </summary>
      /// <param name="key">Primary key of the table Generic Medicines.</param>
      /// <returns>Returns a Generic Medicine if the key is matched.</returns>
      public Task<GenericDrug> GetGenericMedicineByKey(int key);

      /// <summary>
      /// The method is used to get a Generic Medicine by Generic Name.
      /// </summary>
      /// <param name="genericName">Name of a Generic Medicine.</param>
      /// <returns>Returns a Generic Medicine if the Generic Name is matched.</returns>
      public Task<GenericDrug> GetGenericMedicineByName(string genericName);

      /// <summary>
      /// The method is used to get the GenericMedicines by DrugUtilityID.
      /// </summary>
      /// <param name="drugUtilityId">DrugUtilityID of the table GenericMedicines.</param>
      /// <returns>Returns a GenericMedicines if the DrugUtilityID is matched.</returns>
      public Task<IEnumerable<GenericDrug>> GetGenericMedicinesByDrugUtilityID(int drugUtilityId);

      /// <summary>
      /// The method is used to get the GenericMedicines by DrugSubClassesID.
      /// </summary>
      /// <param name="drugSubClassesId">DrugSubClassesID of the table GenericMedicines.</param>
      /// <returns>Returns a GenericMedicines if the DrugSubClassesID is matched.</returns>
      public Task<IEnumerable<GenericDrug>> GetGenericMedicinesByDrugSubClassID(int drugSubClassesId);
   }
}