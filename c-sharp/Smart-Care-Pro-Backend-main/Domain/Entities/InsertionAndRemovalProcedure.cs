using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Brian
 * Date created : 03.05.2023
 * Modified by  :
 * Last modified:
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// FamilyPlan entity.
   /// </summary>
   public class InsertionAndRemovalProcedure : EncounterBaseModel
   {
      /// <summary>
      /// Primary Key of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Implant insertion of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "Implant insertion")]
      public bool ImplantInsertion { get; set; }

      /// <summary>
      /// Implant insertion of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "Implant insertion date")]
      public DateTime? ImplantInsertionDate { get; set; }

      /// <summary>
      /// Implant removal of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "Implant removal")]
      public bool ImplantRemoval { get; set; }

      /// <summary>
      /// Implant removal date of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "Implant removal date")]
      public DateTime? ImplantRemovalDate { get; set; }

      /// <summary>
      /// IUCD insertion of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "IUCD insertion")]
      public bool IUCDInsertion { get; set; }

      /// <summary>
      /// IUCD insertion date of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "IUCD insertion date")]
      public DateTime? IUCDInsertionDate { get; set; }

      /// <summary>
      /// IUCD removal of the table InsertionAndRemovalProcedures.
      /// </summary>
      [Display(Name = "IUCD removal")]
      public bool IUCDRemoval { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public DateTime? IUCDRemovalDate { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}