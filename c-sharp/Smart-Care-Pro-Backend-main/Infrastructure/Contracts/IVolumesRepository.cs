using Domain.Entities;

/*
 * Created by   : Brian
 * Date created : 29.04.2023
 * Modified by  : Stephan
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IVolumesRepository : IRepository<Volume>
   {
        /// <summary>
        /// The method is used to get a Volume by ClientId.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns>update a Volumes of the volumes.</returns>
        Volume UpdateVolume(Volume volume);
   }
}