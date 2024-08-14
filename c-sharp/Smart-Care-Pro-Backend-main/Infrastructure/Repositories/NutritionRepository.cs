using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 28.03.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of INutritionRepository interface.
    /// </summary>
    public class NutritionRepository : Repository<Nutrition>, INutritionRepository
    {
        public NutritionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Nutrition by key.
        /// </summary>
        /// <param name="key">Primary key of the table Nutrition.</param>
        /// <returns>Returns a Nutrition if the key is matched.</returns>
        public async Task<Nutrition> GetNutritionByKey(Guid key)
        {
            try
            {
                var nutrition = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (nutrition != null)
                    nutrition.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return nutrition;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Nutrition.
        /// </summary>
        /// <returns>Returns a list of all Nutritions.</returns>
        public async Task<IEnumerable<Nutrition>> GetNutritions()
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
        /// The method is used to get a Nutrition by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Nutrition if the ClientID is matched.</returns>
        public async Task<IEnumerable<Nutrition>> GetNutritionByClient(Guid clientId)
        {
            try
            {
                return await context.Nutritions.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId)
       .Join(
           context.Encounters.AsNoTracking(),
           nutrition => nutrition.EncounterId,
           encounter => encounter.Oid,
           (nutrition, encounter) => new Nutrition
           {
               EncounterId = nutrition.EncounterId,
               EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
               ClientId = nutrition.ClientId,
               CreatedBy = nutrition.CreatedBy,
               CreatedIn = nutrition.CreatedIn,
               DateCreated = nutrition.DateCreated,
               DateModified = nutrition.DateModified,
               EncounterType = nutrition.EncounterType,
               InteractionId = nutrition.InteractionId,
               IsDeleted = nutrition.IsDeleted,
               IsSynced = nutrition.IsSynced,
               MAM = nutrition.MAM,
               ModifiedBy = nutrition.ModifiedBy,
               ModifiedIn = nutrition.ModifiedIn,
               Obesity = nutrition.Obesity,
               SAM = nutrition.SAM,
               Status = nutrition.Status,
               Stunting = nutrition.Stunting,
               UnderWeight = nutrition.UnderWeight,

           }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Nutrition by EncounterID.
        /// </summary>
        /// <returns>Returns a list of all Nutrition by EncounterID.</returns>
        public async Task<IEnumerable<Nutrition>> GetNutritionByEncounter(Guid encounterId)
        {
            try
            {
                //         return await context.Nutritions.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId)
                //.Join(
                //    context.Encounters.AsNoTracking(),
                //    nutrition => nutrition.EncounterId,
                //    encounter => encounter.Oid,
                //    (nutrition, encounter) => new Nutrition
                //    {
                //        EncounterId = nutrition.EncounterId,
                //        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                //        ClientId = nutrition.ClientId,
                //        CreatedBy = nutrition.CreatedBy,
                //        CreatedIn = nutrition.CreatedIn,
                //        DateCreated = nutrition.DateCreated,
                //        DateModified = nutrition.DateModified,
                //        EncounterType = nutrition.EncounterType,
                //        InteractionId = nutrition.InteractionId,
                //        IsDeleted = nutrition.IsDeleted,
                //        IsSynced = nutrition.IsSynced,
                //        MAM = nutrition.MAM,
                //        ModifiedBy = nutrition.ModifiedBy,
                //        ModifiedIn = nutrition.ModifiedIn,
                //        Obesity = nutrition.Obesity,
                //        SAM = nutrition.SAM,
                //        Status = nutrition.Status,
                //        Stunting = nutrition.Stunting,
                //        UnderWeight = nutrition.UnderWeight,

                //    }).OrderByDescending(x => x.EncounterDate).ToListAsync();
                return await QueryAsync(c => c.IsDeleted == false && c.EncounterId == encounterId);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}