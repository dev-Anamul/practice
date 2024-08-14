using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
 * Created by   : Stephan
 * Date created : 02.01.2023
 * Modified by  : Bella  
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IGlasgowComaScaleRepository interface.
    /// </summary>
    public class GlasgowComaScaleRepository : Repository<GlasgowComaScale>, IGlasgowComaScaleRepository
    {
        public GlasgowComaScaleRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a glasgow coma scale by key.
        /// </summary>
        /// <param name="key">Primary key of the table GlasgowComaScales.</param>
        /// <returns>Returns a glasgow coma scale if the key is matched.</returns>
        public async Task<GlasgowComaScale> GetGlasgowComaScaleByKey(Guid key)
        {
            try
            {
                var glasgowComaScale = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (glasgowComaScale != null)
                {
                    glasgowComaScale.ClinicianName = await context.UserAccounts.Where(x => x.Oid == glasgowComaScale.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    glasgowComaScale.FacilityName = await context.Facilities.Where(x => x.Oid == glasgowComaScale.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    glasgowComaScale.EncounterDate = await context.Encounters.Where(x => x.Oid == glasgowComaScale.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return glasgowComaScale;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a EncounterId if the key is matched.</returns>
        public async Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByEncounterId(Guid encounterId)
        {
            try
            {
                return await context.GlasgowComaScales.AsNoTracking().Where(g => g.EncounterId == encounterId && g.IsDeleted == false).AsNoTracking()
                 .Join(
                     context.Encounters.AsNoTracking(),
                 glasgowComaScale => glasgowComaScale.EncounterId,
                     encounter => encounter.Oid,
                     (glasgowComaScale, encounter) => new GlasgowComaScale
                     {
                         EncounterId = glasgowComaScale.EncounterId,
                         EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                         ClientId = glasgowComaScale.ClientId,
                         CreatedIn = glasgowComaScale.CreatedIn,
                         CreatedBy = glasgowComaScale.CreatedBy,
                         DateCreated = glasgowComaScale.DateCreated,
                         DateModified = glasgowComaScale.DateModified,
                         EncounterType = glasgowComaScale.EncounterType,
                         EyeScore = glasgowComaScale.EyeScore,
                         GlasgowComaScore = glasgowComaScale.GlasgowComaScore,
                         InteractionId = glasgowComaScale.InteractionId,
                         IsDeleted = glasgowComaScale.IsDeleted,
                         IsSynced = glasgowComaScale.IsSynced,
                         LeftPupilsLightReactionReaction = glasgowComaScale.LeftPupilsLightReactionReaction,
                         LeftPupilsLightReactionSize = glasgowComaScale.LeftPupilsLightReactionReaction,
                         ModifiedBy = glasgowComaScale.ModifiedBy,
                         ModifiedIn = glasgowComaScale.ModifiedIn,
                         MotorScale = glasgowComaScale.MotorScale,
                         Result = glasgowComaScale.Result,
                         RightPupilsLightReactionReaction = glasgowComaScale.RightPupilsLightReactionReaction,
                         RightPupilsLightReactionSize = glasgowComaScale.RightPupilsLightReactionSize,
                         VerbalScore = glasgowComaScale.VerbalScore,
                         ClinicianName = context.UserAccounts.Where(x => x.Oid == glasgowComaScale.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                         FacilityName = context.Facilities.Where(x => x.Oid == glasgowComaScale.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                     }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// The method is used to get the list of glasgow coma scales.
        /// </summary>
        /// <returns>Returns a list of all glasgow coma scales.</returns>
        public async Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScales()
        {
            try
            {
                return await QueryAsync(g => g.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientID(Guid clientId)
        {
            try
            {
                return await context.GlasgowComaScales.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                  .Join(
                      context.Encounters.AsNoTracking(),
                  glasgowComaScale => glasgowComaScale.EncounterId,
                      encounter => encounter.Oid,
                      (glasgowComaScale, encounter) => new GlasgowComaScale
                      {
                          EncounterId = glasgowComaScale.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = glasgowComaScale.ClientId,
                          CreatedIn = glasgowComaScale.CreatedIn,
                          CreatedBy = glasgowComaScale.CreatedBy,
                          DateCreated = glasgowComaScale.DateCreated,
                          DateModified = glasgowComaScale.DateModified,
                          EncounterType = glasgowComaScale.EncounterType,
                          EyeScore = glasgowComaScale.EyeScore,
                          GlasgowComaScore = glasgowComaScale.GlasgowComaScore,
                          InteractionId = glasgowComaScale.InteractionId,
                          IsDeleted = glasgowComaScale.IsDeleted,
                          IsSynced = glasgowComaScale.IsSynced,
                          LeftPupilsLightReactionReaction = glasgowComaScale.LeftPupilsLightReactionReaction,
                          LeftPupilsLightReactionSize = glasgowComaScale.LeftPupilsLightReactionReaction,
                          ModifiedBy = glasgowComaScale.ModifiedBy,
                          ModifiedIn = glasgowComaScale.ModifiedIn,
                          MotorScale = glasgowComaScale.MotorScale,
                          Result = glasgowComaScale.Result,
                          RightPupilsLightReactionReaction = glasgowComaScale.RightPupilsLightReactionReaction,
                          RightPupilsLightReactionSize = glasgowComaScale.RightPupilsLightReactionSize,
                          VerbalScore = glasgowComaScale.VerbalScore,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == glasgowComaScale.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == glasgowComaScale.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientIDLast24Hours(Guid clientId)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.GlasgowComaScales.AsNoTracking().Where(p => p.IsDeleted == false && p.DateCreated >= Last24Hours && p.ClientId == clientId).AsNoTracking()
                  .Join(
                      context.Encounters.AsNoTracking(),
                  glasgowComaScale => glasgowComaScale.EncounterId,
                      encounter => encounter.Oid,
                      (glasgowComaScale, encounter) => new GlasgowComaScale
                      {
                          EncounterId = glasgowComaScale.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          ClientId = glasgowComaScale.ClientId,
                          CreatedIn = glasgowComaScale.CreatedIn,
                          CreatedBy = glasgowComaScale.CreatedBy,
                          DateCreated = glasgowComaScale.DateCreated,
                          DateModified = glasgowComaScale.DateModified,
                          EncounterType = glasgowComaScale.EncounterType,
                          EyeScore = glasgowComaScale.EyeScore,
                          GlasgowComaScore = glasgowComaScale.GlasgowComaScore,
                          InteractionId = glasgowComaScale.InteractionId,
                          IsDeleted = glasgowComaScale.IsDeleted,
                          IsSynced = glasgowComaScale.IsSynced,
                          LeftPupilsLightReactionReaction = glasgowComaScale.LeftPupilsLightReactionReaction,
                          LeftPupilsLightReactionSize = glasgowComaScale.LeftPupilsLightReactionReaction,
                          ModifiedBy = glasgowComaScale.ModifiedBy,
                          ModifiedIn = glasgowComaScale.ModifiedIn,
                          MotorScale = glasgowComaScale.MotorScale,
                          Result = glasgowComaScale.Result,
                          RightPupilsLightReactionReaction = glasgowComaScale.RightPupilsLightReactionReaction,
                          RightPupilsLightReactionSize = glasgowComaScale.RightPupilsLightReactionSize,
                          VerbalScore = glasgowComaScale.VerbalScore,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == glasgowComaScale.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == glasgowComaScale.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<GlasgowComaScale>> GetGlasgowComaScalesByClientID(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var glasgowComaScaleAsQueryable = context.GlasgowComaScales.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                glasgowComaScale => glasgowComaScale.EncounterId,
                    encounter => encounter.Oid,
                    (glasgowComaScale, encounter) => new GlasgowComaScale
                    {
                        EncounterId = glasgowComaScale.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        ClientId = glasgowComaScale.ClientId,
                        CreatedIn = glasgowComaScale.CreatedIn,
                        CreatedBy = glasgowComaScale.CreatedBy,
                        DateCreated = glasgowComaScale.DateCreated,
                        DateModified = glasgowComaScale.DateModified,
                        EncounterType = glasgowComaScale.EncounterType,
                        EyeScore = glasgowComaScale.EyeScore,
                        GlasgowComaScore = glasgowComaScale.GlasgowComaScore,
                        InteractionId = glasgowComaScale.InteractionId,
                        IsDeleted = glasgowComaScale.IsDeleted,
                        IsSynced = glasgowComaScale.IsSynced,
                        LeftPupilsLightReactionReaction = glasgowComaScale.LeftPupilsLightReactionReaction,
                        LeftPupilsLightReactionSize = glasgowComaScale.LeftPupilsLightReactionReaction,
                        ModifiedBy = glasgowComaScale.ModifiedBy,
                        ModifiedIn = glasgowComaScale.ModifiedIn,
                        MotorScale = glasgowComaScale.MotorScale,
                        Result = glasgowComaScale.Result,
                        RightPupilsLightReactionReaction = glasgowComaScale.RightPupilsLightReactionReaction,
                        RightPupilsLightReactionSize = glasgowComaScale.RightPupilsLightReactionSize,
                        VerbalScore = glasgowComaScale.VerbalScore,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == glasgowComaScale.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == glasgowComaScale.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                    }).AsQueryable();

                if (encounterType == null)
                    return await glasgowComaScaleAsQueryable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await glasgowComaScaleAsQueryable.Where(x => x.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetGlasgowComaScalesByClientIDTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.GlasgowComaScales.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.GlasgowComaScales.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
    }
}