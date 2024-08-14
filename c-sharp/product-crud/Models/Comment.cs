using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [MaxLength(100)]
        public string Description { get; set; } = String.Empty;
        public string Post { get; set; } = String.Empty;
        public string User { get; set; } = String.Empty;

        public int MyProperty { get; set; }

    }
}