using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IPrEPStoppingReasonRepository : IRepository<StoppingReason>
    {
        /// <summary>
        /// The method is used to get a prEP Stopping Reason by key.
        /// </summary>
        /// <param name="key">Primary key of the table PrEPStoppingReasons.</param>
        /// <returns>Returns a prEP Stopping Reason if the key is matched.</returns>
        public Task<StoppingReason> GetPrEPStoppingReasonByKey(int key);

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public Task<IEnumerable<StoppingReason>> GetPrEPStoppingReasons();

        /// <summary>
        /// The method is used to get a prepStoppingReason by prepStoppingReason name.
        /// </summary>
        /// <param name="prepStoppingReasonName">Name of a prepStoppingReason.</param>
        /// <returns>Returns a county if the prepStoppingReason name is matched.</returns>
        public Task<StoppingReason> GetPrEPStoppingReasonByName(string prepStoppingReasonName);
    }
}