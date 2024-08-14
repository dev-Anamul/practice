using Domain.Entities;
using Infrastructure.Contracts;
using Serilog; 
/*
 * Created by   : Brian
 * Date created : 06.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IUserAccountRepository interface.
    /// </summary>
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a user account by username.
        /// </summary>
        /// <param name="UserName">Username of a user.</param>
        /// <returns>Returns a user account if the username is matched.
        public async Task<UserAccount> GetUserAccountByUsername(string UserName)
        {
            try
            {
                var user = new UserAccount();

                return await LoadWithChildAsync<UserAccount>(u => u.Username.ToLower().Trim() == UserName.ToLower().Trim() && u.IsDeleted == false, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByUsername", "UserAccountRepository.cs", "erro Occured");
               
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by key.
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts.</param>
        /// <returns>Returns a user account if the key is matched.</returns>
        public async Task<UserAccount> GetUserAccountByKey(Guid key)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Oid == key && u.IsDeleted == false);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByKey", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by first name.
        /// </summary>
        /// <param name="firstName">First name of a user.</param>
        /// <returns>Returns a user account if the first name is matched.</returns>
        public async Task<UserAccount> GetUserAccountByFirstName(string firstName)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.FirstName.ToLower().Trim() == firstName.ToLower().Trim() && u.IsDeleted == false);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByFirstName", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by surname.
        /// </summary>
        /// <param name="surname">Surname of a user.</param>
        /// <returns>Returns a user account if the surname is matched.</returns>
        public async Task<UserAccount> GetUserAccountBySurname(string surname)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Surname.ToLower().Trim() == surname.ToLower().Trim() && u.IsDeleted == false);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountBySurname", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number is matched.</returns>
        public async Task<UserAccount> GetUserAccountByCellphone(string cellphone)
        {
            try
            {
                return await LoadWithChildAsync<UserAccount>(u => u.Cellphone.ToLower().Trim() == cellphone.ToLower().Trim() && u.IsDeleted == false, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByCellphone", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by cellphone number and Country Code.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <param name="CountryCode">Country Code of a user.</param>
        /// <returns>Returns a user account if the cellphone number and Country Code is matched.</returns>
        public async Task<UserAccount> GetUserAccountByCellphoneNCountryCode(string cellphone, string CountryCode)
        {
            try
            {
                return await LoadWithChildAsync<UserAccount>(u => u.Cellphone.ToLower().Trim() == cellphone.ToLower().Trim() && u.CountryCode.ToLower().Trim() == CountryCode.ToLower().Trim() && u.IsDeleted == false, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByCellphoneNCountryCode", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a user account by cellphone number,Country Code and userId .
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <param name="CountryCode">Country Code of a user.</param>
        /// <param name="UserId">userId of a user.</param>
        /// <returns>Returns a user account if the cellphone number,Country Code and userId .</returns>
        public async Task<UserAccount> GetUserAccountByCellphoneNCountryCode(string cellphone, string CountryCode, Guid UserId)
        {
            try
            {
                return await LoadWithChildAsync<UserAccount>(u => u.Cellphone.ToLower().Trim() == cellphone.ToLower().Trim() && u.CountryCode.ToLower().Trim() == CountryCode.ToLower().Trim() && u.IsDeleted == false && u.Oid != UserId, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByCellphoneNCountryCode", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }
        /// <summary>
        /// The method is used to get a user account by cellphone number or UserId.
        /// </summary>
        /// <param name="Username or Cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number or UserId is matched.</returns>
        //public UserAccount GetbyCellPhoneOrUserId(string Cellphone = "")
        public UserAccount GetbyCellPhoneOrUsername(string Username, string Cellphone)
        {
            try
            {
                if (Username != null || Cellphone != null)
                {
                    var result = context.UserAccounts.FirstOrDefault(x => x.Username == Username || x.Cellphone == Cellphone);

                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetbyCellPhoneOrUsername", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of user accounts.
        /// </summary>
        /// <returns>Returns a list of all user accounts.</returns>
        public async Task<IEnumerable<UserAccount>> GetUserAccounts()
        {
            try
            {
                return await QueryAsync(u => u.IsDeleted == false);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccounts", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

        public async Task<UserAccount> GetUserAccountByNRC(string nrc)
        {
            try
            {
                return await LoadWithChildAsync<UserAccount>(u => u.NRC == nrc && u.IsDeleted == false, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByNRC", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }

       public async Task<UserAccount> GetUserNRC(string nrc)
       {
            try
            {
               return await FirstOrDefaultAsync(c => c.NRC == nrc && c.IsDeleted == false);
            }
            catch (Exception)
            {
               throw;
            }
       }

        public async Task<UserAccount> GetUserAccountByNRC(string nrc, Guid userId)
        {
            try
            {
                return await LoadWithChildAsync<UserAccount>(u => u.NRC == nrc && u.IsDeleted == false && u.Oid != userId, p => p.FacilityAccesses);
            }
            catch (Exception)
            {
                Serilog.Log.Error("{LogDate}{Location} {MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "UserAccountRepository", "GetUserAccountByNRC", "UserAccountRepository.cs", "erro Occured");
                throw;
            }
        }
    }
}