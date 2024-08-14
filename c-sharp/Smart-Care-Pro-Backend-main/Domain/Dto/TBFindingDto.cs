using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 08.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class TBFindingDto : EncounterBaseModel
   {
      public int[] TBFindingList { get; set; }

      public Guid ClientID { get; set; }
   }
}