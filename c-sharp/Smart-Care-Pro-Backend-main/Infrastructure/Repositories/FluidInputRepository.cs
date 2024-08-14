using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by    : Stephan
 * Date created  : 09.02.2023
 * Modified by   : Bella
 * Last modified : 13.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class FluidInputRepository : Repository<FluidIntake>, IFluidInputRepository
    {
        public FluidInputRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a FluidInputs by key.
        /// </summary>
        /// <param name="key">Primary key of the table FluidInputs.</param>
        /// <returns>Returns a FluidInputs if the key is matched.</returns>
        public async Task<FluidIntake> GetFluidInputByKey(Guid key)
        {
            try
            {
                var fluidIntake = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (fluidIntake != null)
                {
                    fluidIntake.ClinicianName = await context.UserAccounts.Where(x => x.Oid == fluidIntake.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    fluidIntake.FacilityName = await context.Facilities.Where(x => x.Oid == fluidIntake.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    fluidIntake.EncounterDate = await context.Encounters.Where(x => x.Oid == fluidIntake.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();

                }
                return fluidIntake;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a FluidInputs if encounterId is matched.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a FluidInputs if the encounterId is matched.</returns>
        public async Task<IEnumerable<FluidOutput>> GetFluidOutputByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.FluidsOutputs.Where(p => p.IsDeleted == false && p.EncounterId == EncounterId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                 fluidsOutputs => fluidsOutputs.EncounterId,
                    encounter => encounter.Oid,
                    (fluidsOutputs, encounter) => new FluidOutput
                    {
                        EncounterId = fluidsOutputs.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        IsDeleted = fluidsOutputs.IsDeleted,
                        InteractionId = fluidsOutputs.InteractionId,
                        FluidId = fluidsOutputs.FluidId,
                        IsSynced = fluidsOutputs.IsSynced,
                        ModifiedBy = fluidsOutputs.ModifiedBy,
                        DateCreated = fluidsOutputs.DateCreated,
                        DateModified = fluidsOutputs.DateModified,
                        CreatedBy = fluidsOutputs.CreatedBy,
                        CreatedIn = fluidsOutputs.CreatedIn,
                        EncounterType = fluidsOutputs.EncounterType,
                        ModifiedIn = fluidsOutputs.ModifiedIn,
                        OutputAmount = fluidsOutputs.OutputAmount,
                        OutputTime = fluidsOutputs.OutputTime,
                        OutputType = fluidsOutputs.OutputType,
                        Route = fluidsOutputs.Route,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluidsOutputs.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluidsOutputs.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FluidInputs.
        /// </summary>
        /// <returns>Returns a list of all FluidInputs.</returns>
        public async Task<IEnumerable<FluidIntake>> GetFluidInputs()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a FluidInputs by FluidId.
        /// </summary>
        /// <param name="FluidID"></param>
        /// <returns>Returns a FluidInputs if the FluidId is matched.</returns>
        public async Task<IEnumerable<FluidIntake>> GetFluidInputByFluid(Guid FluidId)
        {
            try
            {
                return await context.FluidIntakes.Where(b => b.IsDeleted == false && b.FluidId == FluidId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                 fluidIntake => fluidIntake.EncounterId,
                    encounter => encounter.Oid,
                    (fluidIntake, encounter) => new FluidIntake
                    {
                        EncounterId = fluidIntake.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        CreatedBy = fluidIntake.CreatedBy,
                        CreatedIn = fluidIntake.CreatedIn,
                        DateCreated = fluidIntake.DateCreated,
                        DateModified = fluidIntake.DateModified,
                        EncounterType = fluidIntake.EncounterType,
                        Fluid = fluidIntake.Fluid,
                        FluidId = FluidId,
                        IntakeAmount = fluidIntake.IntakeAmount,
                        IntakeTime = fluidIntake.IntakeTime,
                        IntakeType = fluidIntake.IntakeType,
                        InteractionId = fluidIntake.InteractionId,
                        IsDeleted = fluidIntake.IsDeleted,
                        IsSynced = fluidIntake.IsSynced,
                        ModifiedBy = fluidIntake.ModifiedBy,
                        ModifiedIn = fluidIntake.ModifiedIn,
                        Route = fluidIntake.Route,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluidIntake.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluidIntake.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                   
                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// The method is used to get a FluidInputs by Encounter.
        /// </summary>
        /// <param name="EncounterId"></param>
        /// <returns>Returns a FluidInputs if the EncounterID is matched.</returns>
        public async Task<IEnumerable<FluidIntake>> GetFluidInputByEncounter(Guid EncounterId)
        {
            try
            {
                return await context.FluidIntakes.Where(p => p.IsDeleted == false && p.FluidId == EncounterId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                 fluidIntake => fluidIntake.EncounterId,
                    encounter => encounter.Oid,
                    (fluidIntake, encounter) => new FluidIntake
                    {
                        EncounterId = fluidIntake.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        CreatedBy = fluidIntake.CreatedBy,
                        CreatedIn = fluidIntake.CreatedIn,
                        DateCreated = fluidIntake.DateCreated,
                        DateModified = fluidIntake.DateModified,
                        EncounterType = fluidIntake.EncounterType,
                        FluidId = fluidIntake.FluidId,
                        IntakeAmount = fluidIntake.IntakeAmount,
                        IntakeTime = fluidIntake.IntakeTime,
                        IntakeType = fluidIntake.IntakeType,
                        InteractionId = fluidIntake.InteractionId,
                        IsDeleted = fluidIntake.IsDeleted,
                        IsSynced = fluidIntake.IsSynced,
                        ModifiedBy = fluidIntake.ModifiedBy,
                        ModifiedIn = fluidIntake.ModifiedIn,
                        Route = fluidIntake.Route,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluidIntake.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluidIntake.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}