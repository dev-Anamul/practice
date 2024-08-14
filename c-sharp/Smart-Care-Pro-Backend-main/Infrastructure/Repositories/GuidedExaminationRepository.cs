using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tomas
 * Date created : 05.03.2023
 * Modified by  : Bella
 * Last modified: 13.08.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class GuidedExaminationRepository : Repository<GuidedExamination>, IGuidedExaminationRepository
    {
        public GuidedExaminationRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a birth record by ClientID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a GuidedExamination if the ClientID is matched.</returns>
        public async Task<IEnumerable<GuidedExamination>> ReadGuidedExaminationByClient(Guid clientId)
        {
            try
            {
                return await context.GuidedExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                .Join(
                    context.Encounters.AsNoTracking(),
                guidedExaminations => guidedExaminations.EncounterId,
                    encounter => encounter.Oid,
                    (guidedExaminations, encounter) => new GuidedExamination
                    {
                        EncounterId = guidedExaminations.EncounterId,
                        EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                        CreatedIn = guidedExaminations.CreatedIn,
                        CreatedBy = guidedExaminations.CreatedBy,
                        ClientId = guidedExaminations.ClientId,
                        DateModified = guidedExaminations.DateModified,
                        InteractionId = guidedExaminations.InteractionId,
                        AbnormalDischarge = guidedExaminations.AbnormalDischarge,
                        Warts = guidedExaminations.Warts,
                        CervicalConsistency = guidedExaminations.CervicalConsistency,
                        CervicalDischarge = guidedExaminations.CervicalDischarge,
                        CervixColour = guidedExaminations.CervixColour,
                        DateCreated = guidedExaminations.DateCreated,
                        EncounterType = guidedExaminations.EncounterType,
                        FibroidPalpable = guidedExaminations.FibroidPalpable,
                        IsDeleted = guidedExaminations.IsDeleted,
                        IsSynced = guidedExaminations.IsSynced,
                        LiverPalpable = guidedExaminations.LiverPalpable,
                        Masses = guidedExaminations.Masses,
                        ModifiedBy = guidedExaminations.ModifiedBy,
                        ModifiedIn = guidedExaminations.ModifiedIn,
                        Normal = guidedExaminations.Normal,
                        OtherAbnormalities = guidedExaminations.OtherAbnormalities,
                        OtherFindings = guidedExaminations.OtherFindings,
                        Scars = guidedExaminations.Scars,
                        Size = guidedExaminations.Size,
                        Sores = guidedExaminations.Sores,
                        TenderAdnexa = guidedExaminations.TenderAdnexa,
                        UterusPosition = guidedExaminations.UterusPosition,
                        Tenderness = guidedExaminations.Tenderness,
                        VaginalMuscleTone = guidedExaminations.VaginalMuscleTone,
                        ClinicianName = context.UserAccounts.Where(x => x.Oid == guidedExaminations.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                        FacilityName = context.Facilities.Where(x => x.Oid == guidedExaminations.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                    }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of GuidedExamination by EncounterID.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Returns a list of all GuidedExamination by EncounterID.</returns>
        public async Task<IEnumerable<GuidedExamination>> ReadGuidedExaminationByEncounter(Guid encounterId)
        {
            try
            {
                return await context.GuidedExaminations.AsNoTracking().Where(p => p.IsDeleted == false && p.EncounterId == encounterId).AsNoTracking()
                  .Join(
                      context.Encounters.AsNoTracking(),
                  guidedExaminations => guidedExaminations.EncounterId,
                      encounter => encounter.Oid,
                      (guidedExaminations, encounter) => new GuidedExamination
                      {
                          EncounterId = guidedExaminations.EncounterId,
                          EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                          CreatedIn = guidedExaminations.CreatedIn,
                          CreatedBy = guidedExaminations.CreatedBy,
                          ClientId = guidedExaminations.ClientId,
                          DateModified = guidedExaminations.DateModified,
                          InteractionId = guidedExaminations.InteractionId,
                          AbnormalDischarge = guidedExaminations.AbnormalDischarge,
                          Warts = guidedExaminations.Warts,
                          CervicalConsistency = guidedExaminations.CervicalConsistency,
                          CervicalDischarge = guidedExaminations.CervicalDischarge,
                          CervixColour = guidedExaminations.CervixColour,
                          DateCreated = guidedExaminations.DateCreated,
                          EncounterType = guidedExaminations.EncounterType,
                          FibroidPalpable = guidedExaminations.FibroidPalpable,
                          IsDeleted = guidedExaminations.IsDeleted,
                          IsSynced = guidedExaminations.IsSynced,
                          LiverPalpable = guidedExaminations.LiverPalpable,
                          Masses = guidedExaminations.Masses,
                          ModifiedBy = guidedExaminations.ModifiedBy,
                          ModifiedIn = guidedExaminations.ModifiedIn,
                          Normal = guidedExaminations.Normal,
                          OtherAbnormalities = guidedExaminations.OtherAbnormalities,
                          OtherFindings = guidedExaminations.OtherFindings,
                          Scars = guidedExaminations.Scars,
                          Size = guidedExaminations.Size,
                          Sores = guidedExaminations.Sores,
                          TenderAdnexa = guidedExaminations.TenderAdnexa,
                          UterusPosition = guidedExaminations.UterusPosition,
                          Tenderness = guidedExaminations.Tenderness,
                          VaginalMuscleTone = guidedExaminations.VaginalMuscleTone,
                          ClinicianName = context.UserAccounts.Where(x => x.Oid == guidedExaminations.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                          FacilityName = context.Facilities.Where(x => x.Oid == guidedExaminations.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                      }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a GuidedExaminationByKey by key.
        /// </summary>
        /// <param name="key">Primary key of the table GuidedExaminationByKey.</param>
        /// <returns>Returns a GuidedExaminationByKey if the key is matched.</returns>
        public async Task<GuidedExamination> ReadGuidedExaminationByKey(Guid key)
        {
            try
            {
                var guidedExamination = await FirstOrDefaultAsync(p => p.InteractionId == key && p.IsDeleted == false);

                if (guidedExamination != null)
                {
                    guidedExamination.ClinicianName = await context.UserAccounts.Where(x => x.Oid == guidedExamination.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    guidedExamination.FacilityName = await context.Facilities.Where(x => x.Oid == guidedExamination.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    guidedExamination.EncounterDate = await context.Encounters.Where(x => x.Oid == guidedExamination.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return guidedExamination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a GuidedExaminationByKey by key.
        /// </summary>
        /// <returns>Returns a GuidedExaminationByKey if the key is matched.</returns>
        public async Task<IEnumerable<GuidedExamination>> ReadGuidedExaminations()
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
    }
}