using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 30.03.2023
 * Modified by  : Bella
 * Last modified: 26.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IARTDrugRepository : IRepository<ARTDrug>
   {
      /// <summary>
      /// The method is used to get an ArtDrug by art drug name.
      /// </summary>
      /// <param name="artDrug">Name of an ArtDrug.</param>
      /// <returns>Returns an ArtDrug if the Art Drug name is matched.</returns>
      public Task<ARTDrug> GetARTDrugByName(string artDrug);

      /// <summary>
      /// The method is used to get an ArtDrug by key.
      /// </summary>
      /// <param name="key">Primary key of the table ArtDrugs.</param>
      /// <returns>Returns an ArtDrug if the key is matched.</returns>
      public Task<ARTDrug> GetARTDrugByKey(int key);

      /// <summary>
      /// The method is used to get the list of ArtDrugs.
      /// </summary>
      /// <returns>Returns a list of all ArtDrugs.</returns>
      public Task<IEnumerable<ARTDrug>> GetARTDrugs();

      /// <summary>
      /// The method is used to get a ARTDrug by ARTDrugClassId.
      /// </summary>
      /// <param name="artDrugClassId"></param>
      /// <returns>Returns a ARTDrug if the ARTDrugClassId is matched.</returns>
      public Task<IEnumerable<ARTDrug>> GetARTDrugByARTDrugClass(int artDrugClassId);
   }
}