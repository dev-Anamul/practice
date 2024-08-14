using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class IdentifiedTBFindingDto : EncounterBaseModel
   {
      public DateTime? DateCreated { get; set; }

      public DateTime? DateModified { get; set; }

      public Guid IdentifiedTBFindingInteractionID { get; set; }

      public int TBFindingID { get; set; }

      public TBFinding TBFinding { get; set; }

      public Guid ClientID { get; set; }

      public EncounterType EncounterType { get; set; }
   }
}