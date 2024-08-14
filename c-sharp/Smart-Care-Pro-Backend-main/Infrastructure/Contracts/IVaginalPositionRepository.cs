using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Brian
 * Date created : 18.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IVaginalPositionRepository : IRepository<VaginalPosition>
    {
        /// <summary>
        /// The method is used to get a VeginalPosition by key.
        /// </summary>
        /// <param name="key">Primary key of the table VeginalPositions.</param>
        /// <returns>Returns a VeginalPosition if the key is matched.</returns>
        public Task<VaginalPosition> GetVaginalPositionByKey(Guid key);

        /// <summary>
        /// The method is used to get the list of VeginalPosition.
        /// </summary>
        /// <returns>Returns a list of all VeginalPosition.</returns>
        public Task<IEnumerable<VaginalPosition>> GetVaginalPositions();

        /// <summary>
        /// The method is used to get a birth record by ClientId.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>Returns a VeginalPosition if the ClientId is matched.</returns>
        public Task<IEnumerable<VaginalPosition>> GetVaginalPositionByClient(Guid clientId);

        /// <summary>
        /// The method is used to get the list of VeginalPosition by EncounterId.
        /// </summary>
        /// <returns>Returns a list of all VeginalPosition by EncounterId.</returns>
        public Task<IEnumerable<VaginalPosition>> GetVaginalPositionByEncounter(Guid encounterId);
    }
}