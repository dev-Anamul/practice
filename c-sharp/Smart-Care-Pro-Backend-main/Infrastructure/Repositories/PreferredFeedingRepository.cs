using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : Biplob Roy
 * Date created : 29.04.2023
 * Modified by  : Biplob Roy
 * Last modified: 02.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   /// <summary>
   /// Implementation of IOPDVisitRepository interface.
   /// </summary>
   public class PreferredFeedingRepository : Repository<PreferredFeeding>, IPreferredFeedingRepository
   {
      public PreferredFeedingRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get an PreferredFeeding by key.
      /// </summary>
      /// <param name="key">Primary key of the table PreferredFeedings.</param>
      /// <returns>Returns an PreferredFeeding if the key is matched.</returns>
      public async Task<PreferredFeeding> GetPreferredFeedingByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(i => i.Oid == key && i.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of PreferredFeedings.
      /// </summary>
      /// <returns>Returns a list of all PreferredFeedings.</returns>
      public async Task<IEnumerable<PreferredFeeding>> GetPreferredFeedings()
      {
         try
         {
            return await QueryAsync(i => i.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

        /// <summary>
        /// The method is used to get an PreferredFeeding by PreferredFeeding Description.
        /// </summary>
        /// <param name="description">Name of an PreferredFeeding.</param>
        /// <returns>Returns an PreferredFeeding if the PreferredFeeding description is matched.</returns>
        public async Task<PreferredFeeding> GetPreferredFeedingByName(string preferredFeeding)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == preferredFeeding.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}