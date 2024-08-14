using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bella
 * Date created : 02.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class FacilityAccessRepository : Repository<FacilityAccess>, IFacilityAccessRepository
   {
      /// <summary>
      /// Implementation of IFacilityAccessRepository interface.
      /// </summary>
      public FacilityAccessRepository(DataContext context) : base(context)
      {

      }

      /// <summary>
      /// The method is used to get the list of the FacilityAccesses levels.
      /// </summary>
      /// <returns>Returns a list of all the FacilityAccesses levels.</returns>
      public async Task<IEnumerable<FacilityAccess>> GetFacilityAccesses()
      {
         try
         {
            return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.IsIgnored == false, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get the list of all non admin user.
      /// </summary>
      /// <returns>Returns a list of all the non admin user.</returns>
      public async Task<IEnumerable<UserAccount>> GetNonAdminUsersForAdmin()
      {
         try
         {
            return context.UserAccounts
         .Include(d => d.FacilityAccesses)
         .ThenInclude(c => c.Facility)
         .Where(x => x.UserType != Utilities.Constants.Enums.UserType.SystemAdministrator)
         .OrderBy(x => x.Surname)
         .ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a Facility by key.
      /// </summary>
      /// <param name="facilityId">Primary key of the table Facility.</param>
      /// <returns>Returns a Facility if the key is matched.</returns>
      public async Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByFacility(int facilityId)
      {
         try
         {
            return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<List<FacilityAccess>> GetNonDFZFacilities(Guid userId)
      {
         try
         {
            return await context.FacilityAccesses.Where(f => f.UserAccountId == userId && f.IsDeleted == false && f.Facility.IsDFZ == false).ToListAsync();

         }
         catch (Exception)
         {
            throw;
         }

      }

      public async Task<FacilityAccess> GetFacilityAccessByUserAndFacilityId(Guid userAccountId, Guid key)
      {
         try
         {
            return await context.FacilityAccesses.FirstOrDefaultAsync(f => f.UserAccountId == userAccountId && f.Oid == key && f.IsDeleted == false);

         }
         catch (Exception)
         {
            throw;
         }
      }
      public async Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByLoginRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize)
      {
         try
         {
            if (string.IsNullOrEmpty(searchNameOrCellPhone))
            {
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && !x.IsApproved && !x.ForgotPassword, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);
            }
            else
            {
               searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
               string normalizedCellphone = searchNameOrCellPhone.Trim();
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && !x.IsApproved && !x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
               || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone), page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);

            }
            //    return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int GetFacilityAccessesByLoginRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone)
      {


         if (string.IsNullOrEmpty(searchNameOrCellPhone))
         {
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && !x.IsApproved && !x.ForgotPassword).Count();
         }
         else
         {
            searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
            string normalizedCellphone = searchNameOrCellPhone.Trim();
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && !x.IsApproved && !x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
                || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone)).Count();

         }
      }
      public async Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByApprovedRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize)
      {
         try
         {
            if (string.IsNullOrEmpty(searchNameOrCellPhone))
            {
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.IsApproved && !x.ForgotPassword, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);
            }
            else
            {
               searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
               string normalizedCellphone = searchNameOrCellPhone.Trim();
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.IsApproved && !x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
               || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone), page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);

            }
            //    return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int GetFacilityAccessesByApprovedRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone)
      {


         if (string.IsNullOrEmpty(searchNameOrCellPhone))
         {
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.IsApproved && !x.ForgotPassword).Count();
         }
         else
         {
            searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
            string normalizedCellphone = searchNameOrCellPhone.Trim();
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.IsApproved && !x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
                || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone)).Count();

         }
      }

      public async Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByRecoveryRequestFacility(int facilityId, string searchNameOrCellPhone, int page, int pageSize)
      {
         try
         {
            if (string.IsNullOrEmpty(searchNameOrCellPhone))
            {
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.ForgotPassword, page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);
            }
            else
            {
               searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
               string normalizedCellphone = searchNameOrCellPhone.Trim();
               return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
               || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
               || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone), page, pageSize, orderBy: d => d.OrderByDescending(y => y.DateCreated), p => p.UserAccount, p => p.Facility);

            }
            //    return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int GetFacilityAccessesByRecoveryRequestFacilityTotalCount(int facilityId, string searchNameOrCellPhone)
      {
         if (string.IsNullOrEmpty(searchNameOrCellPhone))
         {
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.ForgotPassword).Count();
         }
         else
         {
            searchNameOrCellPhone = searchNameOrCellPhone.ToLower();
            string normalizedCellphone = searchNameOrCellPhone.Trim();
            return context.FacilityAccesses.Where(x => x.IsDeleted == false && x.IsSynced == false && x.FacilityId == facilityId && x.IsIgnored == false && x.ForgotPassword && (x.UserAccount.FirstName + " " + x.UserAccount.Surname).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + " " + x.UserAccount.Cellphone).ToLower().Contains(searchNameOrCellPhone)
                || (x.UserAccount.CountryCode + x.UserAccount.Cellphone).Contains(normalizedCellphone) || x.UserAccount.Cellphone.Contains(normalizedCellphone)
                || x.UserAccount.Cellphone.ToLower().Contains(searchNameOrCellPhone)).Count();

         }
      }

      /// <summary>
      /// The method is used to get a FacilityAccess by key.
      /// </summary>
      /// <param name="key">Primary key of the table FacilityAccess.</param>
      /// <returns>Returns a FacilityAccess if the key is matched.</returns>
      public async Task<FacilityAccess> GetFacilityAccessByKey(Guid key)
      {
         try
         {
            return await LoadWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.Oid == key, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get a UserAccount by key.
      /// </summary>
      /// <param name="userAccountId">Primary key of the table UserAccount.</param>
      /// <returns>Returns a UserAccount if the key is matched.</returns>
      public async Task<FacilityAccess> GetFacilityAccessByUserAccountId(Guid userAccountId)
      {
         try
         {
            return await LoadWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.UserAccountId == userAccountId, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is used to get all facility access by UserAccount.
      /// </summary>
      /// <param name="userAccountId">Primary key of the table UserAccount.</param>
      /// <returns>Returns a UserAccount if the key is matched.</returns>
      public async Task<IEnumerable<FacilityAccess>> GetAllFacilityAccessesByUserAccountId(Guid userAccountId)
      {
         try
         {
            return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsIgnored == false && x.ForgotPassword == true && x.IsSynced == false && x.UserAccountId == userAccountId, p => p.UserAccount, p => p.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

        /// <summary>
        /// The method is used to get all facility access by UserAccount.
        /// </summary>
        /// <param name="userAccountId">Primary key of the table UserAccount.</param>
        /// <returns>Returns a UserAccount if the key is matched.</returns>
        public async Task<IEnumerable<FacilityAccess>> GetFacilityAccessesByUserAccountId(Guid userAccountId)
        {
            try
            {
                return await LoadListWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsApproved == true && x.UserAccountId == userAccountId, p => p.UserAccount, p => p.Facility);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a UserAccount by user id and facility id.
        /// </summary>
        /// <param name="userAccountId">Primary key of the table UserAccount.</param>
        /// <param name="facilityId">Primary key of the table Facility.</param>
        /// <returns>Returns a UserAccount if the key is matched.</returns>
        public async Task<FacilityAccess> GetFacilityAccessByUserIdWithFacilityId(Guid userAccountId, int facilityId)
      {
         try
         {
            return await LoadWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.UserAccountId == userAccountId && x.FacilityId == facilityId, p => p.UserAccount, f => f.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// The method is to check duplicate Facility Access Request by UserId to get a UserAccount by key.
      /// </summary>
      /// <param name="userAccountId">Primary key of the table UserAccount.</param>
      /// <param name="facilityId">Primary key of the table Facility.</param>
      /// <returns>Returns a UserAccount if the key is matched.</returns>
      public async Task<FacilityAccess> CheckDuplicateFacilityAccessRequestByUserId(Guid userAccountId, int facilityId)
      {
         try
         {
            return await LoadWithChildAsync<FacilityAccess>(x => x.IsDeleted == false && x.IsSynced == false && x.UserAccountId == userAccountId && x.FacilityId == facilityId && x.IsIgnored == false && x.IsApproved == false, p => p.UserAccount, f => f.Facility);
         }
         catch (Exception)
         {
            throw;
         }
      }
    }
}