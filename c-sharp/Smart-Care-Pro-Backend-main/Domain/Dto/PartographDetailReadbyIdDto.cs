using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class PartographDetailReadbyIdDto
    {
        public Guid PartographID { get; set; }
        public List<string[]> FetalHeartRateData { get; set; }
        public List<string[]> ContractionsData { get; set; }
        public List<string[]> LiquorData { get; set; }
        public List<string[]> MouldingData { get; set; }
        public List<string[]> CervixData { get; set; }
        public List<string[]> DescentData { get; set; }
        public List<string[]> OxytocinData { get; set; }
        public List<string[]> DropsData { get; set; }
        public List<string[]> MedicineData { get; set; }
        public List<string[]> BloodPressureData { get; set; }
        public List<string[]> PulseData { get; set; }
        public List<string[]> TemparatureData { get; set; }
        public List<string[]> ProteinData { get; set; }
        public List<string[]> AcetoneData { get; set; }
        public List<string[]> VolumeData { get; set; }
    }
}
