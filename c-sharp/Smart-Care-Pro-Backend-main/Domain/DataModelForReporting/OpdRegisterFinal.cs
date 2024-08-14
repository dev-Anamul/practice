using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataModelForReporting
{
    public class OpdRegisterFinal
    {
        public string NUPN { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string HivTested { get; set; }
        public string HivTestResult { get; set; }
        public string ReferredForArt { get; set; }
        public string ICD11_Code { get; set; }
        public string Diagnosis { get; set; }
        public string ReferredTo { get; set; }
        public string Remarks { get; set; }
    }
}
