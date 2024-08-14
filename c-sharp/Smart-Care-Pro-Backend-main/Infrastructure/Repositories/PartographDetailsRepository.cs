using Domain.Dto;
using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PartographDetailsRepository : Repository<PartographDetail>, IPartographDetailsRepository
    {
        private readonly DataContext context;

        public PartographDetailsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<PartographDetailReadbyIdDto> GetPartographDetailsbyPartograph(Guid partographId)
        {
            var fetalHeartRatesData = await context.FetalHeartRates
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.FetalRateTime)
                .Select(x => new string[]
                {
                    x.FetalRateTime.ToString(),
                    x.FetalRate.ToString()
                })
                .ToListAsync();

            var liquorData = await context.Liquors
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.LiquorTime)
                .Select(x => new string[]
                {
                    x.LiquorTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var mouldingData = await context.Mouldings
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.MouldingTime)
                .Select(x => new string[]
                {
                    x.MouldingTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var cervixData = await context.Cervixes
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.CervixTime)
                .Select(x => new string[]
                {
                    x.CervixTime.ToString(),
                    x.CervixDetails.ToString()
                })
                .ToListAsync();

            var descentData = await context.DescentOfHeads
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.DescentOfHeadTime)
                .Select(x => new string[]
                {
                    x.DescentOfHeadTime.ToString(),
                    x.DescentOfHeadDetails.ToString()
                })
                .ToListAsync();

            var contractionsData = await context.Contractions
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.ContractionsTime)
                .Select(x => new string[]
                {
                    x.ContractionsTime.ToString(),
                    x.ContractionsDetails.ToString(),
                    x.Duration
                })
                .ToListAsync();

            var oxytocinData = await context.Oxytocins
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.OxytocinTime)
                .Select(x => new string[]
                {
                    x.OxytocinTime.ToString(),
                    x.OxytocinDetails.ToString()
                })
                .ToListAsync();

            var dropsData = await context.Drops
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.DropsTime)
                .Select(x => new string[]
                {
                    x.DropsTime.ToString(),
                    x.DropsDetails.ToString()
                })
                .ToListAsync();

            var medicineData = await context.Medicines
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.MedicinesTime)
                .Select(x => new string[]
                {
                    x.MedicinesTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var bloodPressureData = await context.BloodPressures
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.BloodPressureTime)
                .Select(x => new string[]
                {
                    x.BloodPressureTime.ToString(),
                    x.SystolicPressure.ToString(),
                    x.DiastolicPressure.ToString()
                })
                .ToListAsync();

            var pulseData = await context.Pulses
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.PulseTime)
                .Select(x => new string[]
                {
                    x.PulseTime.ToString(),
                    x.PulseDetails.ToString()
                })
                .ToListAsync();

            var temparatureData = await context.Temperatures
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.TemperatureTime)
                .Select(x => new string[]
                {
                    x.TemperatureTime.ToString(),
                    x.TemperaturesDetails.ToString()
                })
                .ToListAsync();

            var proteinData = await context.Proteins
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.ProteinsTime)
                .Select(x => new string[]
                {
                    x.ProteinsTime.ToString(),
                    x.ProteinsDetails
                })
                .ToListAsync();

            var acetoneData = await context.Acetones
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.AcetoneTime)
                .Select(x => new string[]
                {
                    x.AcetoneTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var volumeData = await context.Volumes
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.VolumesTime)
                .Select(x => new string[]
                {
                    x.VolumesTime.ToString(),
                    x.VolumesDetails
                })
                .ToListAsync();

            var partographDetails = new PartographDetailReadbyIdDto()
            {
                PartographID = partographId,
                FetalHeartRateData = fetalHeartRatesData,
                LiquorData = liquorData,
                MouldingData = mouldingData,
                CervixData = cervixData,
                DescentData = descentData,
                ContractionsData = contractionsData,
                OxytocinData = oxytocinData,
                AcetoneData = acetoneData,
                BloodPressureData = bloodPressureData,
                DropsData = dropsData,
                MedicineData = medicineData,
                ProteinData = proteinData,
                PulseData = pulseData,
                TemparatureData = temparatureData,
                VolumeData = volumeData
            };
            return partographDetails;
        }

        public async Task<PartographDetailReadDto> GetPartographDetailsAsync(Guid partographId)
        {
            var fetalHeartRatesData = await context.FetalHeartRates
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.FetalRateTime)
                .Select(x => new long[]
                {
                    x.FetalRateTime,
                    x.FetalRate
                })
                .ToListAsync();

            var liquorData = await context.Liquors
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.LiquorTime)
                .Select(x => new string[]
                {
                    x.LiquorTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var mouldingData = await context.Mouldings
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.MouldingTime)
                .Select(x => new string[]
                {
                    x.MouldingTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var cervixData = await context.Cervixes
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.CervixTime)
                .Select(x => new long[]
                {
                    x.CervixTime,
                    x.CervixDetails
                })
                .ToListAsync();

            var descentData = await context.DescentOfHeads
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.DescentOfHeadTime)
                .Select(x => new long[]
                {
                    x.DescentOfHeadTime,
                    x.DescentOfHeadDetails
                })
                .ToListAsync();

            var contractionsData = await context.Contractions
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.ContractionsTime)
                .Select(x => new string[]
                {
                    x.ContractionsTime.ToString(),
                    x.ContractionsDetails.ToString(),
                    x.Duration
                })
                .ToListAsync();

            var oxytocinData = await context.Oxytocins
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.OxytocinTime)
                .Select(x => new long[]
                {
                    x.OxytocinTime,
                    x.OxytocinDetails
                })
                .ToListAsync();

            var dropsData = await context.Drops
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.DropsTime)
                .Select(x => new long[]
                {
                    x.DropsTime,
                    x.DropsDetails
                })
                .ToListAsync();

            var medicineData = await context.Medicines
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.MedicinesTime)
                .Select(x => new string[]
                {
                    x.MedicinesTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var bloodPressureData = await context.BloodPressures
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.BloodPressureTime)
                .Select(x => new long[]
                {
                    x.BloodPressureTime,
                    x.SystolicPressure,
                    x.DiastolicPressure
                })
                .ToListAsync();

            var pulseData = await context.Pulses
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.PulseTime)
                .Select(x => new long[]
                {
                    x.PulseTime,
                    x.PulseDetails
                })
                .ToListAsync();

            var temparatureData = await context.Temperatures
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.TemperatureTime)
                .Select(x => new long[]
                {
                    x.TemperatureTime,
                    x.TemperaturesDetails
                })
                .ToListAsync();

            var proteinData = await context.Proteins
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.ProteinsTime)
                .Select(x => new string[]
                {
                    x.ProteinsTime.ToString(),
                    x.ProteinsDetails
                })
                .ToListAsync();

            var acetoneData = await context.Acetones
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.AcetoneTime)
                .Select(x => new string[]
                {
                    x.AcetoneTime.ToString(),
                    x.Description
                })
                .ToListAsync();

            var volumeData = await context.Volumes
                .Where(i => i.PartographId == partographId && i.IsDeleted == false)
                .OrderBy(i => i.VolumesTime)
                .Select(x => new string[]
                {
                    x.VolumesTime.ToString(),
                    x.VolumesDetails
                })
                .ToListAsync();

            var partographDetails = new PartographDetailReadDto()
            {
                PartographID = partographId,
                FetalHeartRateData = fetalHeartRatesData,
                LiquorData = liquorData,
                MouldingData = mouldingData,
                CervixData = cervixData,
                DescentData = descentData,
                ContractionsData = contractionsData,
                OxytocinData = oxytocinData,
                AcetoneData = acetoneData,
                BloodPressureData = bloodPressureData,
                DropsData = dropsData,
                MedicineData = medicineData,
                ProteinData = proteinData,
                PulseData = pulseData,
                TemparatureData = temparatureData,
                VolumeData = volumeData
            };
            return partographDetails;
        }
    }
}