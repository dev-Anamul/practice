using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Rezwana
 * Date created : 21.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IResultRepository interface.
    /// </summary>
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(DataContext context) : base(context)
        {

        }


        /// <summary>
        /// The method is used to get a result by key.
        /// </summary>
        /// <param name="key">Primary key of the table Results.</param>
        /// <returns>Returns a result if the key is matched.</returns>
        public async Task<Result> GetResultByKey(Guid key)
        {
            try
            {
                var referralModule = await LoadWithChildAsync<Result>(r => r.InteractionId == key && r.IsDeleted == false, k => k.Investigation, z => z.Investigation.Test, m => m.Investigation.Test.MeasuringUnits, o => o.ResultOption);

                if (referralModule != null)
                    referralModule.EncounterDate = await context.Encounters.Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                return referralModule;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of results.
        /// </summary>
        /// <returns>Returns a list of all results.</returns>
        public async Task<IEnumerable<Result>> GetResults()
        {
            try
            {
                return await QueryAsync(r => r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<Result> GetLatestResultByClient(Guid ClientID)
        {
            return await context.Results.Include(x => x.Investigation).ThenInclude(z => z.Test).AsNoTracking().Where(p => p.IsDeleted == false && p.Investigation.ClientId == ClientID)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        result => result.EncounterId,
                        encounter => encounter.Oid,
                        (result, encounter) => new Result
                        {
                            EncounterId = result.EncounterId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            DateModified = result.DateModified,
                            DateCreated = result.DateCreated,
                            Investigation = result.Investigation,
                            CommentOnResult = result.CommentOnResult,
                            CreatedBy = result.CreatedBy,
                            CreatedIn = result.CreatedIn,
                            EncounterType = result.EncounterType,
                            InteractionId = result.InteractionId,
                            InvestigationId = result.InvestigationId,
                            IsDeleted = result.IsDeleted,
                            IsResultNormal = result.IsResultNormal,
                            IsSynced = result.IsSynced,
                            MeasuringUnitId = result.MeasuringUnitId,
                            ModifiedBy = result.ModifiedBy,
                            ModifiedIn = result.ModifiedIn,
                            ResultDate = result.ResultDate,
                            ResultDescriptive = result.ResultDescriptive,
                            ResultNumeric = result.ResultNumeric,
                            ResultOptionId = result.ResultOptionId,


                        }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();

        }
    }
}