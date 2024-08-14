using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;

/*
 * Created by: Sphinx(1)
 * Date created: 17.10.2022
 * Modified by: Sphinx(2)
 * Last modified: 06.11.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IRecoveryRequestRepository interface.
    /// </summary>
    public class RecoveryRequestRepository : Repository<RecoveryRequest>, IRecoveryRequestRepository
    {
        public RecoveryRequestRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a recovery request by key.
        /// </summary>
        /// <param name="key">Primary key of the table RecoveryRequests</param>
        /// <returns>Returns a recovery request if the key is matched.</returns>
        public async Task<RecoveryRequest> GetRecoveryRequestByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a recovery request by date.
        /// </summary>
        /// <param name="recoveryRequestDate">Date of a recovery request.</param>
        /// <returns>Returns a recovery request if the date is matched.</returns>
        public async Task<IEnumerable<RecoveryRequest>> GetRecoveryRequestByDate(AdminRecoveryRequestDto recoveryRequestDate)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && ((c.DateRequested) >= (recoveryRequestDate.DateFrom) && (c.DateRequested) <= (recoveryRequestDate.DateTo)) && c.IsRequestOpen == true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a recovery request by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a recovery request if the cellphone number is matched.</returns>
        public async Task<RecoveryRequest> GetRecoveryRequestByCellphone(string cellphone)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Cellphone.Trim() == cellphone.Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a recovery request by Username.
        /// </summary>
        /// <param name="Username">Username of a user.</param>
        /// <returns>Returns a recovery request if the Username is matched.</returns>
        public async Task<RecoveryRequest> GetRecoveryRequestByUsername(string Username)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Username.Trim() == Username.Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of recovery requests.
        /// </summary>
        /// <returns>Returns a list of all recovery requests.</returns>
        public async Task<IEnumerable<RecoveryRequest>> GetRecoveryRequests()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}