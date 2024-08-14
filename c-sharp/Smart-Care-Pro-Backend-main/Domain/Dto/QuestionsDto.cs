using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 18.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class QuestionsDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
    }
}