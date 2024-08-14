using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class BloodPressureRepository : Repository<BloodPressure>, IBloodPressureRepository
    {
        private readonly DataContext context;

        public BloodPressureRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public BloodPressure UpdateBloodPressure(BloodPressure bloodPressure)
        {
            try
            {
                var existingInDb = context.BloodPressures
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(bloodPressure.PartographId) &&
                        i.BloodPressureTime.Equals(bloodPressure.BloodPressureTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new BloodPressure()
                    {
                        PartographId = bloodPressure.PartographId,
                        SystolicPressure = bloodPressure.SystolicPressure,
                        DiastolicPressure = bloodPressure.DiastolicPressure,
                        BloodPressureTime = bloodPressure.BloodPressureTime,
                        IsSynced = false,
                        IsDeleted = false,

                    };
                    context.BloodPressures.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.SystolicPressure != bloodPressure.SystolicPressure || existingInDb.DiastolicPressure != bloodPressure.DiastolicPressure)
                    {

                        existingInDb.SystolicPressure = bloodPressure.SystolicPressure;
                        existingInDb.DiastolicPressure = bloodPressure.DiastolicPressure;
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