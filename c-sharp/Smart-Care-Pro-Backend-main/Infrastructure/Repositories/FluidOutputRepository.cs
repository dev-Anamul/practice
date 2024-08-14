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
    public class FluidOutputRepository : Repository<FluidOutput>, IFluidOutputRepository
    {
        public FluidOutputRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a FluidOutput by key.
        /// </summary>
        /// <param name="key">Primary key of the table FluidOutputs.</param>
        /// <returns>Returns a FluidOutput if the key is matched.</returns>
        public async Task<FluidOutput> GetFluidOutputByKey(Guid key)
        {
            try
            {
                var fluidOutput = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (fluidOutput != null)
                {
                    fluidOutput.ClinicianName = await context.UserAccounts.Where(x => x.Oid == fluidOutput.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    fluidOutput.FacilityName = await context.Facilities.Where(x => x.Oid == fluidOutput.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    fluidOutput.EncounterDate = await context.Encounters.Where(x => x.Oid == fluidOutput.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return fluidOutput;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FluidOutputs.
        /// </summary>
        /// <returns>Returns a list of all FluidOutputs.</returns>
        public async Task<IEnumerable<FluidOutput>> GetFluidOutputs()
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
        /// The method is used to get a FluidOutput by FluidID.
        /// </summary>
        /// <param name="fluidId"></param>
        /// <returns>Returns a FluidOutputs if the FluidID is matched.</returns>
        public async Task<IEnumerable<FluidOutput>> GetFluidOutputByFluid(Guid fluidId)
        {
            try
            {
                return await context.FluidsOutputs.Where(p => p.IsDeleted == false && p.FluidId == fluidId).AsNoTracking()
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
        /// The method is used to get a FluidOutputs by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a FluidOutput if the EncounterID is matched.</returns>
        public async Task<IEnumerable<FluidOutput>> GetFluidOutputByEncounter(Guid encounterId)
        {
            try
            {
                return await context.FluidsOutputs.Where(p => p.IsDeleted == false && p.FluidId == encounterId).AsNoTracking()
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
    }
}