using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IMedicineManufactureRepository : IRepository<MedicineManufacturer>
    {
        public Task<MedicineManufacturer> GetMedicineManufacturerByKey(int key);

        public Task<IEnumerable<MedicineManufacturer>> GetMedicineManufacturer();

        public Task<MedicineManufacturer> GetMedicineManufacturerByName(string medicineManufacturer);

    }
}
