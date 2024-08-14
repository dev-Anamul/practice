using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 12.09.2022
 * Modified by  : Bella
 * Last modified: 14.08.2023 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IKeyPopulationRepository : IRepository<KeyPopulation>
   {
      /// <summary>
      /// The method is used to get a key population by key.
      /// </summary>
      /// <param name="key">Primary key of the table KeyPopulations.</param>
      /// <returns>Returns a key population if the key is matched.</returns>
      public Task<KeyPopulation> GetKeyPopulationByKey(int key);

      /// <summary>
      /// The method is used to get the list of key population.
      /// </summary>
      /// <returns>Returns a list of all key population.</returns>
      public Task<IEnumerable<KeyPopulation>> GetKeyPopulations();

      /// <summary>
      /// The method is used to get a key Population by keyPopulation name.
      /// </summary>
      /// <param name="keyPopulation">Name of a key Population.</param>
      /// <returns>Returns a keyPopulations if the key Population is matched.</returns>
      public Task<KeyPopulation> GetKeyPopulationByName(string keyPopulation);
   }
}