using Domain.Entities;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface INeonatalBirthOutcomeRepository : IRepository<NeonatalBirthOutcome>
    {
        /// <summary>
        /// The method is used to get a NeonatalBirthOutcome by key.
        /// </summary>
        /// <param name="key">Primary key of the table NeonatalBirthOutcomes.</param>
        /// <returns>Returns a NeonatalBirthOutcome if the key is matched.</returns>
        public Task<NeonatalBirthOutcome> GetNeonatalBirthOutcomeByKey(int key);

        /// <summary>
        /// The method is used to get the list of NeonatalBirthOutcomes.
        /// </summary>
        /// <returns>Returns a list of all NeonatalBirthOutcomes.</returns>
        public Task<IEnumerable<NeonatalBirthOutcome>> GetNeonatalBirthOutcomes();

        /// <summary>
        /// The method is used to get an NeonatalBirthOutcome by NeonatalBirthOutcome Description.
        /// </summary>
        /// <param name="neonatalBirthOutcome">Description of an NeonatalBirthOutcome.</param>
        /// <returns>Returns an NeonatalBirthOutcome if the NeonatalBirthOutcome name is matched.</returns>
        public Task<NeonatalBirthOutcome> GetNeonatalBirthOutcomeByName(string neonatalBirthOutcome);
    }
}