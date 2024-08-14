using Domain.Entities;
using Infrastructure.Contracts;
using static Utilities.Constants.Enums;

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
   /// <summary>
   /// Implementation of FrequencyInterval class.
   /// </summary>
   public class FrequencyIntervalRepository : Repository<FrequencyInterval>, IFrequencyIntervalRepository
   {
      public FrequencyIntervalRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get a FrequencyInterval by key.
      /// </summary>
      /// <param name="key">Primary key of the table FrequencyInterval.</param>
      /// <returns>Returns a FrequencyInterval if the key is matched.</returns>
      public async Task<FrequencyInterval> GetFrequencyIntervalByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(s => s.Oid == key && s.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of FrequencyInterval by TimeInterval .
      /// </summary>
      /// <returns>Returns a list of all FrequencyInterval.</returns>        
      public async Task<IEnumerable<FrequencyInterval>> GetFrequencyIntervalByTimeInterval()
      {
         try
         {
            return await QueryAsync(d => d.IsDeleted == false && d.FrequencyType == FrequencyType.TimeInterval);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of FrequencyInterval by Frequency .
      /// </summary>
      /// <returns>Returns a list of all FrequencyInterval.</returns>        
      public async Task<IEnumerable<FrequencyInterval>> GetFrequencyIntervalByFrequency()
      {
         try
         {
            return await QueryAsync(d => d.IsDeleted == false && d.FrequencyType == FrequencyType.FrequencyInterval);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get an FrequencyInterval by FrequencyInterval Description.
      /// </summary>
      /// <param name="frequencyInterval">Name of an FrequencyInterval.</param>
      /// <returns>Returns an FrequencyInterval if the FrequencyInterval description is matched.</returns>
      public async Task<FrequencyInterval> GetFrequencyIntervalByName(string frequencyInterval)
      {
         try
         {
            return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == frequencyInterval.ToLower().Trim() && a.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

        /// <summary>
        /// The method is used to get the list of FrequencyInterval.
        /// </summary>
        /// <returns>Returns a list of all FrequencyInterval.</returns>
        public async Task<IEnumerable<FrequencyInterval>> GetFrequencyInterval()
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
    }
}