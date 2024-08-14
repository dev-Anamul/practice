using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Domain.Dto
{
    public class ContraceptiveHistoryDTO
    {
        [Column(TypeName = "Tinyint")]
        public int? UsedFor { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int ContraceptiveId { get; set; }
    }
}
