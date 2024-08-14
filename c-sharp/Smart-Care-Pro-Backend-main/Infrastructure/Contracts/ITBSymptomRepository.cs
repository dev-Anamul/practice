using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface ITBSymptomRepository : IRepository<TBSymptom>
   {
      /// <summary>
      /// The method is used to get the list of tbsymptom.
      /// </summary>
      /// <returns>Returns a list of all tbsymptoms.</returns>
      public Task<IEnumerable<TBSymptom>> GetTBSymptoms();
   }
}