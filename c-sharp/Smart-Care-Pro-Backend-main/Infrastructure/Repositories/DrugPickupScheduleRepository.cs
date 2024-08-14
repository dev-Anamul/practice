using Domain.Entities;
using Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DrugPickupScheduleRepository : Repository<DrugPickUpSchedule>, IDrugPickupScheduleRepository
    {
        private readonly DataContext context;

        public DrugPickupScheduleRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
