using Domain.Entities;

/*
 * Created by   : Bella
 * Date created : 07-02-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class PainRecordDto : EncounterBaseModel
   {
      public int PainRecordID { get; set; }

      public List<PainScaleDto> PainScalesList { get; set; }

      public Guid EncounterID { get; set; }

      public Guid ClientID { get; set; }
   }
}