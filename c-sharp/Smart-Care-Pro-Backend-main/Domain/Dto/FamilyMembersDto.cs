using Domain.Entities;
using Utilities.Constants;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
   public class FamilyMembersDto
   {
      public List<FamilyMember> FamilyMembers { get; set; }
      public Guid EncounterID { get; set; }
      public Guid ClientID { get; set; }
      public Guid? CreatedBy { get; set; }
      public int? CreatedIn { get; set; }
      public Guid? ModifiedBy { get; set; }
      public int? ModifiedIn { get; set; }
      public Enums.EncounterType EncounterType { get; set; }
   }
}