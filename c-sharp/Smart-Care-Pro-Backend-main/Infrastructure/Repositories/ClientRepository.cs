using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System.Globalization;
using static Utilities.Constants.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

/*
 * Created by: Phoenix(1)
 * Date created: 12.09.2022
 * Modified by: Sphinx(1)
 * Last modified: 25.10.2022
 */

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IClientRepository interface.
    /// </summary>
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {

        }

         /// <summary>
         /// The method is used to get a client by firstname, surname, sex and dob.
         /// </summary>
         /// <param name="Firstname"></param>
         /// <param name="Surname"></param>
         /// <param name="Sex"></param>
         /// <param name="DOB"></param>
         /// <returns>Returns a client if firstname, surname, sex, dob is matched.</returns>
         public async Task<IEnumerable<Client>> GetClientByClientBasicInfo(string? Firstname, string? Surname, string Sex, string? DOB)
         {
            try
            {
               Sex sex = (Sex)Convert.ToInt32(Sex);

               var clients = await QueryAsync(c => c.Sex == sex && c.IsDeleted == false);

               if (!string.IsNullOrEmpty(DOB))
               {
                  DateTime dob = DateTime.ParseExact(DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                  clients = clients.Where(c => DateTime.Compare(c.DOB.Date, dob) == 0);
               }

               if (!string.IsNullOrEmpty(Surname))
               {
                  clients = clients.Where(c => c.Surname != null && c.Surname.ToLower().Trim() == Surname.ToLower());
               }

               if (!string.IsNullOrEmpty(Firstname))
               {
                  clients = clients.Where(c => c.FirstName != null && c.FirstName.ToLower().Trim() == Firstname.ToLower());
               }

               return clients;
            }
            catch (Exception)
            {
               throw;
            }
         }

         /// <summary>
         /// The method is used to get a client by firstname, surname, sex.
         /// </summary>
         /// <param name="Firstname"></param>
         /// <param name="Surname"></param>
         /// <param name="Sex"></param>
         /// <returns>Returns a client if firstname, surname, sex is matched.</returns>
         /// <exception cref="ArgumentException"></exception>
         public async Task<IEnumerable<Client>> GetClientByClientNoDob(string? Firstname, string? Surname, string Sex)
         {
            try
            {
               if (string.IsNullOrEmpty(Firstname) && string.IsNullOrEmpty(Surname))
               {
                  throw new ArgumentException("You must provide either Firstname or Surname.");
               }

               Sex sex = (Sex)Convert.ToInt32(Sex);

               var clients = await QueryAsync(c => c.Sex == sex && c.IsDeleted == false);

               if (!string.IsNullOrEmpty(Surname))
               {
                  clients = clients.Where(c => c.Surname != null && c.Surname.ToLower().Trim() == Surname.ToLower());
               }

               if (!string.IsNullOrEmpty(Firstname))
               {
                  clients = clients.Where(c => c.FirstName != null && c.FirstName.ToLower().Trim() == Firstname.ToLower());
               }

               return clients;
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
         public async Task<Client> GetClientByKey(Guid key)
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
        /// The method is used to get a client by cellphone.
        /// </summary>
        /// <param name="Cellphone">Cellphone of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public async Task<IEnumerable<Client>> GetClientByCellPhone(string Cellphone, string? CountryCode)
        {
            try
            {
                if (string.IsNullOrEmpty(CountryCode))
                    return await QueryAsync(c => c.IsDeleted == false && c.Cellphone.Trim() == Cellphone.Trim());
                else
                    return await QueryAsync(c => c.IsDeleted == false &&( c.Cellphone.Trim() == Cellphone.Trim()&&c.CellphoneCountryCode.Trim()==CountryCode.Trim()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by cellphone.
        /// </summary>
        /// <param name="Cellphone">Cellphone of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public async Task<IEnumerable<Client>> GetClientByNUPN(string NUPN)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.NUPN.Trim() == NUPN.Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by facilityId.
        /// </summary>
        /// <param name="facilityId">facilityId of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public async Task<IEnumerable<Client>> GetClientByFacility(int facilityId)
        {
            try
            {
               return await QueryAsync(c => c.ModifiedIn == facilityId || c.CreatedIn == facilityId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by facilityId.
        /// </summary>
        /// <param name="facilityId">facilityId of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public async Task<IEnumerable<Client>> GetClientByFacility(int facilityId, int page, int pageSize)
        {
            try
            {
                return await LoadListWithChildAsync<Client>(c => c.IsDeleted == false && (c.ModifiedIn == facilityId || c.CreatedIn == facilityId), page, pageSize, 
                    orderBy: d => d.OrderByDescending(y => y.DateCreated), 
                    h => h.HomeLanguage, 
                    o => o.Occupation,
                    e=>e.EducationLevel,
                    c => c.Country,
                    d => d.District);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by Nupn and Gender.
        /// </summary>
        /// <param name="Cellphone">Cellphone of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public async Task<Client> GetClientByNUPNAndGender(string NUPN)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
               // DateTime minDOBDate = currentDate.AddYears(-5);
                int ageThreshold = 10;
                return await LoadWithChildAsync<Client>(c => c.IsDeleted == false && c.NUPN.Trim() == NUPN.Trim() && c.Sex == Sex.Female &&( currentDate.Year - c.DOB.Year > ageThreshold), c => c.Country);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetClientListByNRC(string NRC)
        {
            try
            {
                return await QueryAsync(c => c.NRC.Trim() == NRC.Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of clients.
        /// </summary>
        /// <returns>Returns a list of all clients.</returns>
        public async Task<IEnumerable<Client>> GetClients()
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

        /// <summary>
        /// The method is used to get a client by client name.
        /// </summary>
        /// <param name="clientName">Name of a client.</param>
        /// <returns>Returns a county if the client name is matched.</returns>
        public async Task<Client> GetClientNRC(string nrc)
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
  
    }
}