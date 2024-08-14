using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class MedicinesRepository : Repository<Medicine>, IMedicinesRepository
    {
        private readonly DataContext context;

        public MedicinesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Medicine UpdateMedicine(Medicine medicine)
        {
            try
            {
                var existingInDb = context.Medicines
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(medicine.PartographId) &&
                        i.MedicinesTime.Equals(medicine.MedicinesTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Medicine()
                    {
                        PartographId = medicine.PartographId,
                        MedicinesTime = medicine.MedicinesTime,
                        Description = medicine.Description,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Medicines.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.Description != medicine.Description)
                    {
                        existingInDb.Description = medicine.Description;
                        existingInDb.IsDeleted = false;
                        existingInDb.IsSynced = false;
                        context.Entry(existingInDb).State = EntityState.Modified;
                    }
                }

                return existingInDb;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}