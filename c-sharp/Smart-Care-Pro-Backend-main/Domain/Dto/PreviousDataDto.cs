using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class PreviousDataDto
    {
        [Required]
        public DateTime PreviousSessionDate { get; set; }
        public Guid ClientId { get; set; }
        public string ServiceCode { get; set; }
    }
}
