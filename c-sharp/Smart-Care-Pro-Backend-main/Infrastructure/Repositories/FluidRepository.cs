using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by    : Stephan
 * Date created  : 29.01.2023
 * Modified by   : Bella  
 * Last modified : 13.08.2023
 * Reviewed by   : 
 * Date Reviewed : 
 */
namespace Infrastructure.Repositories
{
    public class FluidRepository : Repository<Fluid>, IFluidRepository
    {
        public FluidRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Fluid by key.
        /// </summary>
        /// <param name="key">Primary key of the table Fluids.</param>
        /// <returns>Returns a Fluid if the key is matched.</returns>
        public async Task<Fluid> GetFluidByKey(Guid key)
        {
            try
            {
                var fluid = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (fluid != null)
                {
                    fluid.ClinicianName = await context.UserAccounts.Where(x => x.Oid == fluid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    fluid.FacilityName = await context.Facilities.Where(x => x.Oid == fluid.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    fluid.EncounterDate = await context.Encounters.Where(x => x.Oid == fluid.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return fluid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of Fluids.
        /// </summary>
        /// <returns>Returns a list of all Fluids.</returns>
        public async Task<IEnumerable<Fluid>> GetFluids()
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
        /// The method is used to get a Fluid by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a Fluid if the ClientID is matched.</returns>
        public async Task<IEnumerable<Fluid>> GetFluidByClient(Guid clientId)
        {
            try
            {
                return await context.Fluids.Include(x => x.FluidIntakes).Include(x => x.FluidOutputs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                fluid => fluid.EncounterId,
                    encounter => encounter.Oid,
                    (fluid, encounter) => new Fluid
                    {
                        EncounterId = fluid.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        ClientId = fluid.ClientId,
                        CreatedBy = fluid.CreatedBy,
                        CreatedIn = fluid.CreatedIn,
                        DateCreated = fluid.DateCreated,
                        DateModified = fluid.DateModified,
                        DoctorsOrder = fluid.DoctorsOrder,
                        EncounterType = fluid.EncounterType,
                        FluidIntakes = fluid.FluidIntakes.ToList(),
                        FluidOutputs = fluid.FluidOutputs.ToList(),
                        InteractionId = fluid.InteractionId,
                        IsDeleted = fluid.IsDeleted,
                        IsSynced = fluid.IsSynced,
                        ModifiedBy = fluid.ModifiedBy,
                        ModifiedIn = fluid.ModifiedIn,
                        RecordDate = fluid.RecordDate,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluid.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Fluid>> GetFluidByClientLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.Fluids.Include(x => x.FluidIntakes).Include(x => x.FluidOutputs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId && p.DateCreated >= Last24Hours).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                fluid => fluid.EncounterId,
                    encounter => encounter.Oid,
                    (fluid, encounter) => new Fluid
                    {
                        EncounterId = fluid.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        ClientId = fluid.ClientId,
                        CreatedBy = fluid.CreatedBy,
                        CreatedIn = fluid.CreatedIn,
                        DateCreated = fluid.DateCreated,
                        DateModified = fluid.DateModified,
                        DoctorsOrder = fluid.DoctorsOrder,
                        EncounterType = fluid.EncounterType,
                        FluidIntakes = fluid.FluidIntakes.ToList(),
                        FluidOutputs = fluid.FluidOutputs.ToList(),
                        InteractionId = fluid.InteractionId,
                        IsDeleted = fluid.IsDeleted,
                        IsSynced = fluid.IsSynced,
                        ModifiedBy = fluid.ModifiedBy,
                        ModifiedIn = fluid.ModifiedIn,
                        RecordDate = fluid.RecordDate,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluid.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Fluid>> GetFluidByClient(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var fluidAsQuerable = context.Fluids.Include(x => x.FluidIntakes).Include(x => x.FluidOutputs).AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                fluid => fluid.EncounterId,
                    encounter => encounter.Oid,
                    (fluid, encounter) => new Fluid
                    {
                        EncounterId = fluid.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        ClientId = fluid.ClientId,
                        CreatedBy = fluid.CreatedBy,
                        CreatedIn = fluid.CreatedIn,
                        DateCreated = fluid.DateCreated,
                        DateModified = fluid.DateModified,
                        DoctorsOrder = fluid.DoctorsOrder,
                        EncounterType = fluid.EncounterType,
                        FluidIntakes = fluid.FluidIntakes.ToList(),
                        FluidOutputs = fluid.FluidOutputs.ToList(),
                        InteractionId = fluid.InteractionId,
                        IsDeleted = fluid.IsDeleted,
                        IsSynced = fluid.IsSynced,
                        ModifiedBy = fluid.ModifiedBy,
                        ModifiedIn = fluid.ModifiedIn,
                        RecordDate = fluid.RecordDate,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == fluid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == fluid.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).AsQueryable();

                if (encounterType == null)
                    return await fluidAsQuerable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await fluidAsQuerable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetFluidByClientTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Fluids.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Fluids.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a Fluid by Encounter.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a Fluid if the EncounterID is matched.</returns>
        public async Task<IEnumerable<Fluid>> GetFluidByEncounter(Guid encounterId)
        {
            try
            {
                return await context.Fluids.Include(x => x.FluidIntakes).Include(x => x.FluidOutputs).AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
                  .Join(
                      context.Encounters.AsNoTracking(),
                  fluid => fluid.EncounterId,
                      encounter => encounter.Oid,
                      (fluid, encounter) => new Fluid
                      {
                          EncounterId = fluid.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = fluid.ClientId,
                          CreatedBy = fluid.CreatedBy,
                          CreatedIn = fluid.CreatedIn,
                          DateCreated = fluid.DateCreated,
                          DateModified = fluid.DateModified,
                          DoctorsOrder = fluid.DoctorsOrder,
                          EncounterType = fluid.EncounterType,
                          FluidIntakes = fluid.FluidIntakes.ToList(),
                          FluidOutputs = fluid.FluidOutputs.ToList(),
                          InteractionId = fluid.InteractionId,
                          IsDeleted = fluid.IsDeleted,
                          IsSynced = fluid.IsSynced,
                          ModifiedBy = fluid.ModifiedBy,
                          ModifiedIn = fluid.ModifiedIn,
                          RecordDate = fluid.RecordDate,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == fluid.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == fluid.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      }).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}