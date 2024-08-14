using Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Utilities.Constants;

namespace Domain.Entities
{
    /// <summary>
    /// TestType entity.
    /// </summary>
    public class TestType : BaseModel
    {
        /// <summary>
        /// Primary key of the table Countries.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of a country.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Test Type")]
        [IfNotAlphabet]
        public string Description { get; set; }

        /// <summary>
        /// testSubType of a testType.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<TestSubtype> TestSubtypes { get; set; }
    }
}