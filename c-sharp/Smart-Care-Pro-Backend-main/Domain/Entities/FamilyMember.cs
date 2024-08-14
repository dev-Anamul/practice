using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
 * Created by   : Tomas
 * Date created : 15.03.2023
 * Modified by  : Bella
 * Last modified: 12.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// Represents FamilyMember entity in the database.
   /// </summary>
   public class FamilyMember : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table FamilyMembers.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// First name of the family member.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      [Display(Name = "First name")]
      public string FirstName { get; set; }

      /// <summary>
      /// Surname of the family member.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(60)]
      public string Surname { get; set; }

      /// <summary>
      /// Age of the family member.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "SMALLINT")]
      public int Age { get; set; }

      /// <summary>
      /// The field indicates whether HIV Tested or not.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Tested")]
      public bool HIVTested { get; set; }

      /// <summary>
      /// HIV Result of the family member.
      /// </summary>
      [Display(Name = "HIV result")]
      public HIVTestResult HIVResult { get; set; }

      /// <summary>
      /// The field indicates whether the patient is OnArt or not.
      /// </summary>
      [Display(Name = "On ART")]
      public YesNoUnknown? OnART { get; set; }

      /// <summary>
      /// Family member type of the client.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Type of family member")]
      public FamilyMemberType FamilyMemberType { get; set; }

      /// <summary>
      /// Other family member of the client.
      /// </summary>
      [Display(Name = "If other family member")]
      [StringLength(90)]
      public string OtherFamilyMember { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
      /// </summary>
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }

      [NotMapped]
      public List<FamilyMember> FamilyMembers { get; set; }
   }
}