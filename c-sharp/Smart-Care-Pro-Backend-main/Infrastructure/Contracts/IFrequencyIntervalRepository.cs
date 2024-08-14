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
   public interface IFrequencyIntervalRepository : IRepository<FrequencyInterval>
   {
      /// <summary>
      /// The method is used to get a FrequencyInterval by key.
      /// </summary>
      /// <param name="key">Primary key of the table FrequencyInterval.</param>
      /// <returns>Returns a FrequencyInterval if the key is matched.</returns>
      public Task<FrequencyInterval> GetFrequencyIntervalByKey(int key);

      /// <summary>
      /// The method is used to get the list of FrequencyInterval by TimeInterval .
      /// </summary>
      /// <returns>Returns a list of all FrequencyInterval.</returns>        
      public Task<IEnumerable<FrequencyInterval>> GetFrequencyIntervalByTimeInterval();

      /// <summary>
      /// The method is used to get the list of FrequencyInterval by Frequency .
      /// </summary>
      /// <returns>Returns a list of all FrequencyInterval.</returns>        
      public Task<IEnumerable<FrequencyInterval>> GetFrequencyIntervalByFrequency();

      /// <summary>
      /// The method is used to get an FrequencyInterval by FrequencyInterval Description.
      /// </summary>
      /// <param name="frequencyInterval">Description of an FrequencyInterval.</param>
      /// <returns>Returns an FrequencyInterval if the FrequencyInterval name is matched.</returns>
      public Task<FrequencyInterval> GetFrequencyIntervalByName(string frequencyInterval);

      public Task<IEnumerable<FrequencyInterval>> GetFrequencyInterval();
    }
}