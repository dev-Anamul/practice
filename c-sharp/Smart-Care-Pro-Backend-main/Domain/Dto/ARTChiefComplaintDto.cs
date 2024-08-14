using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using static Utilities.Constants.Enums;

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
    public class ARTChiefComplaintDto : EncounterBaseModel
    {
        public int ChiefComplaintID { get; set; }

        /// <summary>
        /// Chief complaints of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(1000)]
        [Display(Name = "Chief Complaints")]
        public string ChiefComplaints { get; set; }

        /// <summary>
        /// History of chief complaints of the client.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(1000)]
        [Display(Name = "History Of Chief Complaints")]
        public string HistoryOfChiefComplaint { get; set; }

        public string TestingLocation { get; set; }

        public HIVTestResult HIVStatus { get; set; }

        public DateTime LastHIVTestDate { get; set; }

        public List<QuestionsDto> QuestionsList { get; set; }

        public int[] KeyPopulations { get; set; }

        public Guid EncounterID { get; set; }

        public Guid ClientID { get; set; }
    }
}