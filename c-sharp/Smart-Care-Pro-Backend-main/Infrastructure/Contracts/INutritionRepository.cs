using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 28.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INutritionRepository : IRepository<Nutrition>
    {
        /// <summary>
        /// The method is used to get a nutrition by key.
        /// </summary>
        /// <param name="key">Primary key of the table Nutritions.</param>
        /// <returns>Returns a nutrition if the key is matched.</returns>
        public Task<Nutrition> GetNutritionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of nutritions.
        /// </summary>
        /// <returns>Returns a list of all nutritions.</returns>
        public Task<IEnumerable<Nutrition>> GetNutritions();

        /// <summary>
        /// The method is used to get a nutrition by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a nutrition if the ClientID is matched.</returns>
        public Task<IEnumerable<Nutrition>> GetNutritionByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the list of nutrition by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all nutrition by EncounterID.</returns>
        public Task<IEnumerable<Nutrition>> GetNutritionByEncounter(Guid encounterId);
    }
}