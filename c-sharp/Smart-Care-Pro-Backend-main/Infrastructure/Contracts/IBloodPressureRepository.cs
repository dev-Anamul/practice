using Domain.Entities;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : 
 * Last modified : 
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Contracts
{
    public interface IBloodPressureRepository : IRepository<BloodPressure>
    {
        BloodPressure UpdateBloodPressure(BloodPressure bloodPressure);
    }
}