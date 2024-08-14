using Domain.Entities;
using static Utilities.Constants.Enums;

/*
 * Created by   : Lion
 * Date created : 15.04.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class IdentifiedReasonDto : EncounterBaseModel
   {
      public DateTime? DateCreated { get; set; }

      public DateTime? DateModified { get; set; }

      public Guid IdentifiedReasonInteractionID { get; set; }

      public int TBSuspectingReasonID { get; set; }

      public TBSuspectingReason TBSuspectingReason { get; set; }

      public Guid ClientID { get; set; }

      public Guid OPDVisitID { get; set; }

      public EncounterType EncounterType { get; set; }

      public int[] TBSuspectingReasonList { get; set; }
   }
}