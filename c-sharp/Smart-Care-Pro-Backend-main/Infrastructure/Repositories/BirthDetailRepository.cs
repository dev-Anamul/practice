using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

/*
 * Created by   : Lion
 * Date created : 29-01-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class BirthDetailRepository : Repository<BirthDetail>, IBirthDetailRepository
    {
        public BirthDetailRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get birth BirthDetail by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthDetails.</param>
        /// <returns>Returns a birth BirthDetail if the key is matched.</returns>
        public async Task<BirthDetail> GetBirthDetailByKey(Guid key)
        {
            try
            {
                var birthDetail = await FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);
                if (birthDetail != null)
                {
                    birthDetail.ClinicianName = await context.UserAccounts.Where(x => x.Oid == birthDetail.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    birthDetail.FacilityName = await context.Facilities.Where(x => x.Oid == birthDetail.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    birthDetail.EncounterDate = await context.Encounters.Where(x => x.Oid == birthDetail.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return birthDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of birth histories.
        /// </summary>
        /// <returns>Returns a list of all birth histories.</returns>
        public async Task<IEnumerable<BirthDetail>> GetBirthDetails()
        {
            try
            {
                return await context.BirthDetails.AsNoTracking().Join(
                        context.Encounters.AsNoTracking(),
                        birthDetails => birthDetails.EncounterId,
                        encounter => encounter.Oid,
                        (birthDetails, encounter) => new BirthDetail
                        {
                            InteractionId = birthDetails.InteractionId,
                            IsSuccessfulDelivery = birthDetails.IsSuccessfulDelivery,
                            Remarks = birthDetails.Remarks,
                            Gender = birthDetails.Gender,
                            Weight = birthDetails.Weight,
                            TypeOfDelivery = birthDetails.TypeOfDelivery,
                            BirthDate = birthDetails.BirthDate,
                            BirthTime = birthDetails.BirthTime,
                            PartographId = birthDetails.PartographId,

                            // Include other properties as needed

                            EncounterId = birthDetails.EncounterId,
                            EncounterType = birthDetails.EncounterType,
                            CreatedIn = birthDetails.CreatedIn,
                            DateCreated = birthDetails.DateCreated,
                            CreatedBy = birthDetails.CreatedBy,
                            ModifiedIn = birthDetails.ModifiedIn,
                            DateModified = birthDetails.DateModified,
                            ModifiedBy = birthDetails.ModifiedBy,
                            IsDeleted = birthDetails.IsDeleted,
                            IsSynced = birthDetails.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == birthDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == birthDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    )
                    .OrderByDescending(b => b.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of FluidOutput by encounter id.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FluidOutput>> GetFluidOutputByEncounter(Guid encounterId)
        {

            try
            {
                return await context.FluidsOutputs.AsNoTracking().Join(
                        context.Encounters.AsNoTracking(),
                        fluidOutput => fluidOutput.EncounterId,
                        encounter => encounter.Oid,
                        (fluidOutput, encounter) => new FluidOutput
                        {
                            InteractionId = fluidOutput.InteractionId,
                            OutputTime = fluidOutput.OutputTime,
                            OutputType = fluidOutput.OutputType,
                            OutputAmount = fluidOutput.OutputAmount,
                            Route = fluidOutput.Route,
                            FluidId = fluidOutput.FluidId,
                            Fluid = fluidOutput.Fluid,

                            // Include other properties as needed

                            EncounterId = fluidOutput.EncounterId,
                            EncounterType = fluidOutput.EncounterType,
                            CreatedIn = fluidOutput.CreatedIn,
                            DateCreated = fluidOutput.DateCreated,
                            CreatedBy = fluidOutput.CreatedBy,
                            ModifiedIn = fluidOutput.ModifiedIn,
                            DateModified = fluidOutput.DateModified,
                            ModifiedBy = fluidOutput.ModifiedBy,
                            IsDeleted = fluidOutput.IsDeleted,
                            IsSynced = fluidOutput.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == fluidOutput.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == fluidOutput.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    )
                    .Where(x => x.EncounterId == encounterId && x.IsDeleted == false)
                    .OrderByDescending(b => b.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// The method is used to get the list of Birth Detail by encounter id.
        /// </summary>
        /// <param name="encounterid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BirthDetail>> GetBirthDetailByEncounter(Guid encounterId)
        {
            try
            {
                return await context.BirthDetails.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        birthDetails => birthDetails.EncounterId,
                        encounter => encounter.Oid,
                        (birthDetails, encounter) => new BirthDetail
                        {
                            InteractionId = birthDetails.InteractionId,
                            IsSuccessfulDelivery = birthDetails.IsSuccessfulDelivery,
                            Remarks = birthDetails.Remarks,
                            Gender = birthDetails.Gender,
                            Weight = birthDetails.Weight,
                            TypeOfDelivery = birthDetails.TypeOfDelivery,
                            BirthDate = birthDetails.BirthDate,
                            BirthTime = birthDetails.BirthTime,
                            PartographId = birthDetails.PartographId,

                            // Include other properties as needed

                            EncounterId = birthDetails.EncounterId,
                            EncounterType = birthDetails.EncounterType,
                            CreatedIn = birthDetails.CreatedIn,
                            DateCreated = birthDetails.DateCreated,
                            CreatedBy = birthDetails.CreatedBy,
                            ModifiedIn = birthDetails.ModifiedIn,
                            DateModified = birthDetails.DateModified,
                            ModifiedBy = birthDetails.ModifiedBy,
                            IsDeleted = birthDetails.IsDeleted,
                            IsSynced = birthDetails.IsSynced,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == birthDetails.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == birthDetails.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                        }
                    )

                    .OrderByDescending(b => b.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}