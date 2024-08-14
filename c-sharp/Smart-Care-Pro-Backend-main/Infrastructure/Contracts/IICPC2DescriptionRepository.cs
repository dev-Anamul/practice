using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 04.01.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IICPC2DescriptionRepository : IRepository<ICPC2Description>
   {
      /// <summary>
      /// The method is used to get an ICPC2 description by ICPC2 name.
      /// </summary>
      /// <param name="iCPC2">Name of an ICPC2.</param>
      /// <returns>Returns an ICPC2 description if the ICPC2 name is matched.</returns>
      public Task<ICPC2Description> GetICPC2DescriptionByName(string iCPC2);

      /// <summary>
      /// The method is used to get an ICPC2 description by key.
      /// </summary>
      /// <param name="key">Primary key of the table ICPC2Descriptions.</param>
      /// <returns>Returns an ICPC2 description if the key is matched.</returns>
      public Task<ICPC2Description> GetICPC2DescriptionByKey(int key);

      /// <summary>
      /// The method is used to get the list of ICPC2Descriptions.
      /// </summary>
      /// <returns>Returns a list of all ICPC2Descriptions.</returns>
      public Task<IEnumerable<ICPC2Description>> GetICPC2Descriptions();

      /// <summary>
      /// The method is used to get an ICPC2 description by AnatomicAxisID.
      /// </summary>
      /// <param name="anatomicAxisId">Primary key of the table AnatomicAxes.</param>
      /// <returns>Returns an ICPC2 description if the AnatomicAxisID is matched.</returns>
      public Task<IEnumerable<ICPC2Description>> GetICPC2DescriptionByAnatomicAxis(int anatomicAxisId);

      /// <summary>
      /// The method is used to get an ICPC2 description by PathologyAxisID.
      /// </summary>
      /// <param name="pathologyAxisId">Primary key of the table PathologyAxes.</param>
      /// <param name="anatomicAxisId">Primary key of the table AnatomicAxis.</param>
      /// <returns>Returns an ICPC2 description if the PathologyAxisID is matched.</returns>
      public Task<IEnumerable<ICPC2Description>> GetICPC2DescriptionByPathologyAxis(int pathologyAxisId, int anatomicAxisId);
   }
}