using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;
using static Utilities.Constants.Enums;

/*
* Created by   : Lion
* Date created : 12.08.2023
* Modified by  : 
* Last modified: 
* Reviewed by  :
* Date reviewed:
*/
namespace Domain.Entities
{
   /// <summary>
   /// Investigation entity.
   /// </summary>
   public class Investigation : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table Investigation.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Order Date of the client.
      /// </summary>        
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "Order Date")]
      [IfFutureDate]
      public DateTime OrderDate { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Display(Name = "OrderNumber")]
      [StringLength(10)]
      public string OrderNumber { get; set; }

      /// <summary>
      /// Order Date of the client.
      /// </summary>        
      [DataType(DataType.DateTime)]
      [Column(TypeName = "smalldatetime")]
      [Display(Name = "SampleCollection Date")]
      public DateTime? SampleCollectionDate { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Quantity")]
      public int Quantity { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Display(Name = "SampleQuantity")]
      public int? SampleQuantity { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Display(Name = "Piority")]
      public Piority Piority { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Display(Name = "ImagingTestDetails")]
      public string ImagingTestDetails { get; set; }

      /// <summary>
      /// Title of Investigation.
      /// </summary>
      [Display(Name = "AdditionalComment")]
      public string AdditionalComment { get; set; }

      /// <summary>
      /// Result received yes or no .
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public bool IsResultReceived { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table UserAccounts.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClinicianId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClinicianId")]
      [JsonIgnore]
      public virtual UserAccount UserAccount { get; set; }

      /// <summary>
      /// Foreign key. Primary key of the table Tests.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public int TestId { get; set; }

      [ForeignKey("TestId")]
      [JsonIgnore]
      public virtual Test Test { get; set; }

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

      /// <summary>
      /// Navigation property.
      /// </summary>
      [JsonIgnore]
      public virtual IEnumerable<Result> Results { get; set; }

      [NotMapped]
      public int? TestTypeId { get; set; }
   }
}