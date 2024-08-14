using Domain.Entities;

/*
 * Created by   : Lion
 * Date created : 02.01.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
   public interface IFetalHeartRatesRepository : IRepository<FetalHeartRate>
   {
      /// <summary>
      /// The method is used to update fetal heart rate.
      /// </summary>
      /// <param name="fetalHeartRate"></param>
      /// <returns></returns>
      FetalHeartRate UpdateFatalRate(FetalHeartRate fetalHeartRate);
   }
}