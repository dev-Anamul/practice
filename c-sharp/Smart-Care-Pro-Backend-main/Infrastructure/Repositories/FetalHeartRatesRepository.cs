using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Lion
 * Date created : 02.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
   public class FetalHeartRatesRepository : Repository<FetalHeartRate>, IFetalHeartRatesRepository
   {
      private readonly DataContext context;

      public FetalHeartRatesRepository(DataContext context) : base(context)
      {
         this.context = context;
      }

      public FetalHeartRate UpdateFatalRate(FetalHeartRate fetalHeartRate)
      {
         try
         {
            var existingInDb = context.FetalHeartRates
                .FirstOrDefault(i =>
                    i.PartographId.Equals(fetalHeartRate.PartographId) &&
                    i.FetalRateTime.Equals(fetalHeartRate.FetalRateTime)
                );
            if (existingInDb == null)
            {
               existingInDb = new FetalHeartRate()
               {
                  PartographId = fetalHeartRate.PartographId,
                  FetalRateTime = fetalHeartRate.FetalRateTime,
                  FetalRate = fetalHeartRate.FetalRate,
                  IsSynced = false,
                  IsDeleted = false,
               };
               context.FetalHeartRates.Add(existingInDb);
            }
            else
            {
               existingInDb.FetalRate = fetalHeartRate.FetalRate;
               existingInDb.IsDeleted = false;
               existingInDb.IsSynced = false;
               context.Entry(existingInDb).State = EntityState.Modified;
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