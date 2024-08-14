using Domain.Entities;

/*
 * Created by   : Stephan
 * Date created : 29-01-2023
 * Modified by  : Stephan
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IUserAccountRepository : IRepository<UserAccount>
    {
        /// <summary>
        /// The method is used to get a user account by username.
        /// </summary>
        /// <param name="username">Username of a user.</param>
        /// <returns>Returns a user account if the username is matched.
        public Task<UserAccount> GetUserAccountByUsername(string UserName);

        /// <summary>
        /// The method is used to get a user account by key.
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts.</param>
        /// <returns>Returns a user account if the key is matched.</returns>
        public Task<UserAccount> GetUserAccountByKey(Guid key);

        /// <summary>
        /// The method is used to get a user account by first name.
        /// </summary>
        /// <param name="firstName">First name of a user.</param>
        /// <returns>Returns a user account if the first name is matched.</returns>
        public Task<UserAccount> GetUserAccountByFirstName(string firstName);

        /// <summary>
        /// The method is used to get a user account by surname.
        /// </summary>
        /// <param name="surname">Surname of a user.</param>
        /// <returns>Returns a user account if the surname is matched.</returns>
        public Task<UserAccount> GetUserAccountBySurname(string surname);

        /// <summary>
        /// The method is used to get a user account by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number is matched.</returns>
        public Task<UserAccount> GetUserAccountByCellphone(string cellphone);

        /// <summary>
        /// The method is used to get a user account by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <param name="CountryCode">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number is matched.</returns>
        public Task<UserAccount> GetUserAccountByCellphoneNCountryCode(string cellphone, string CountryCode);

        public Task<UserAccount> GetUserAccountByNRC(string nrc);

        /// <summary>
        /// The method is used to get a user account by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <param name="CountryCode">Cellphone number of a user.</param>
        /// <param name="UserId">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number is matched.</returns>
        public Task<UserAccount> GetUserAccountByCellphoneNCountryCode(string cellphone, string CountryCode, Guid UserId);

        public Task<UserAccount> GetUserAccountByNRC(string nrc, Guid UserId);

        /// <summary>
        /// The method is used to get a user account by cellphone number or UserId.
        /// </summary>
        /// <param name="Username or Cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number or UserId is matched.</returns>
        public UserAccount GetbyCellPhoneOrUsername(string Username, string Cellphone);

        /// <summary>
        /// The method is used to get the list of user accounts.
        /// </summary>
        /// <returns>Returns a list of all user accounts.</returns>
        public Task<IEnumerable<UserAccount>> GetUserAccounts();

         /// <summary>
         /// The method is used to get the user.
         /// </summary>
         /// <returns>Returns a user account.</returns>
         public Task<UserAccount> GetUserNRC(string nrc);
    }
}