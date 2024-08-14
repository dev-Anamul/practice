using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 14.08.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */

namespace Infrastructure.Contracts
{
    public interface IClientRepository : IRepository<Client>
    {
        /// <summary>
        /// The method is used to get a client by client's basic information.
        /// </summary>
        /// <param name="Firstname">First name of a client.</param>
        /// <param name="Surname">Surname of a client.</param>
        /// <param name="Sex">Sex of a client.</param>
        /// <param name="DOB">Date of birth of a client.</param>
        /// <returns>Returns a client if the firstname, surname, sex and date of birth is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByClientBasicInfo(string? Firstname, string? Surname, string Sex, string? DOB);

        /// <summary>
        /// The method is used to get a client by client's basic information.
        /// </summary>
        /// <param name="Firstname">First name of a client.</param>
        /// <param name="Surname">Surname of a client.</param>
        /// <param name="Sex">Sex of a client.</param>
        /// <returns>Returns a client if the firstname, surname and sex is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByClientNoDob(string? Firstname, string? Surname, string Sex);

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public Task<Client> GetClientByKey(Guid key);

        /// <summary>
        /// The method is used to get a client by cellphone.
        /// </summary>
        /// <param name="Cellphone">Cellphone of a client.</param>
        /// <returns>Returns a client if the cellphone is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByCellPhone(string Cellphone, string? CountryCode);

        /// <summary>
        /// The method is used to get a client by NRC.
        /// </summary>
        /// <param name="NRC">NRC of a client.</param>
        /// <returns>Returns a client if the NRC is matched.</returns>
        public Task<IEnumerable<Client>> GetClientListByNRC(string NRC);

        /// <summary>
        /// The method is used to get a client by NUPN.
        /// </summary>NUPN of a client.</param>
        /// <returns>Returns a client if the NUPN is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByNUPN(string NUPN);

        /// <summary>
        /// The method is used to get a client by facilityId.
        /// </summary>facilityId of a client.</param>
        /// <returns>Returns a client if the facilityId is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByFacility(int facilityId);


        /// <summary>
        /// The method is used to get a client by facilityId.
        /// </summary>facilityId of a client.</param>
        /// <returns>Returns a client if the facilityId is matched.</returns>
        public Task<IEnumerable<Client>> GetClientByFacility(int facilityId, int page, int pageSize);

        /// <summary>
        /// The method is used to get a client by NUPN and Gender Filter.
        /// </summary>NUPN of a client.</param>
        /// <returns>Returns a client if the NUPN is matched.</returns>
        public Task<Client> GetClientByNUPNAndGender(string NUPN);

        /// <summary>
        /// The method is used to get the list of clients.
        /// </summary>
        /// <returns>Returns a list of all clients.</returns>
        public Task<IEnumerable<Client>> GetClients();

        /// <summary>
        /// The method is used to get the list of client types.
        /// </summary>
        /// <returns>Returns a list of all client types.</returns>
        public Task<Client> GetClientNRC(string nrc);
    }
}