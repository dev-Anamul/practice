using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataModelForReporting
{
    public class NdFinal
    {
        public string ConditionDisease { get; set; }
        public string ICD_Code { get; set; }
        public int UnderOne { get; set; }
        public int OneToFour { get; set; }
        public int FiveToFourteen { get; set; }
        public int FfteenAndAbove { get; set; }
        public int DiagnosisTotal { get; set; }
        public int DeathUnderOne { get; set; }
        public int DeathOneToFour { get; set; }
        public int DeathFiveToFourteen { get; set; }
        public int DeathFifteenAndAbove { get; set; }
        public int DeathTotal { get; set; }
    }
}
