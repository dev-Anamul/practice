using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;
using System.Xml.Linq;
using Utilities.Constants;

namespace Domain.Dto
{
    public class DotCalendarDto
    {
     

        /// <summary>
        /// Calender Date of the Dot calender.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Calendar Date")]
        public DateTime CalendarDate { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Feedback")]
        public Feedback Feedback { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Dots.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.Town)]
        public Guid DotId { get; set; }

        public string EncounterType { get; set; }
        public Guid CreatedBy { get; set; }
        public int CreatedIn { get; set; }

    }
}
