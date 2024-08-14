using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 09.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IContraceptiveRepository : IRepository<Contraceptive>
    {
        /// <summary>
        /// The method is used to get a contraceptive by contraceptive name.
        /// </summary>
        /// <param name="contraceptiveName">Name of a contraceptive.</param>
        /// <returns>Returns a contraceptive if the contraceptive name is matched.</returns>
        public Task<Contraceptive> GetContraceptiveByName(string contraceptiveName);

        /// <summary>
        /// The method is used to get a contraceptive by key.
        /// </summary>
        /// <param name="key">Primary key of the table Contraceptives.</param>
        /// <returns>Returns a contraceptive if the key is matched.</returns>
        public Task<Contraceptive> GetContraceptiveByKey(int key);

        /// <summary>
        /// The method is used to get the list of contraceptives.
        /// </summary>
        /// <returns>Returns a list of all contraceptives.</returns>
        public Task<IEnumerable<Contraceptive>> GetContraceptives();
    }
}