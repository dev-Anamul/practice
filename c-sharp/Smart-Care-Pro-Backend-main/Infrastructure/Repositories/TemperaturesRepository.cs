using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TemperaturesRepository : Repository<Temperature>, ITemperaturesRepository
    {
        private readonly DataContext context;

        public TemperaturesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public Temperature UpdateTemperature(Temperature temperature)
        {
            try
            {
                var existingInDb = context.Temperatures
                    .FirstOrDefault(i =>
                        i.PartographId.Equals(temperature.PartographId) &&
                        i.TemperatureTime.Equals(temperature.TemperatureTime)
                    );

                if (existingInDb == null)
                {
                    existingInDb = new Temperature()
                    {
                        PartographId = temperature.PartographId,
                        TemperaturesDetails = temperature.TemperaturesDetails,
                        TemperatureTime = temperature.TemperatureTime,
                        IsSynced = false,
                        IsDeleted = false,
                    };
                    context.Temperatures.Add(existingInDb);
                }
                else
                {
                    if (existingInDb.TemperaturesDetails != temperature.TemperaturesDetails)
                    {
                        existingInDb.TemperaturesDetails = temperature.TemperaturesDetails;
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