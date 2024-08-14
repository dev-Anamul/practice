using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.Constants.Enums;

namespace Infrastructure.Repositories
{
    public class ScreeningRepository : Repository<Screening>, IScreeningRepository
    {
        public ScreeningRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Screening>> GetScreening()
        {
            try
            {
                return await context.Screenings.AsNoTracking().Where(p => p.IsDeleted == false)
                    .Join(
                        context.Encounters.AsNoTracking(),
                        screening => screening.EncounterId,
                        encounter => encounter.Oid,
                        (screening, encounter) => new Screening
                        {
                            InteractionId = screening.InteractionId,
                            CreatedBy = screening.CreatedBy,
                            EncounterId = screening.EncounterId,
                            CreatedIn = screening.CreatedIn,
                            DateCreated = screening.DateCreated,
                            DateModified = screening.DateModified,
                            EDIComments = screening.EDIComments,
                            EDIIsAtypicalVessels = screening.EDIIsAtypicalVessels,
                            EDIIsEctopy = screening.EDIIsEctopy,
                            EDIIsLesionCoversMoreThreeQuaters = screening.EDIIsLesionCoversMoreThreeQuaters,
                            EDIIsLesionExtendedIntoCervicalOs = screening.EDIIsLesionExtendedIntoCervicalOs,
                            EDIIsLesionSeen = screening.EDIIsLesionSeen,
                            EDIIsMosiacism = screening.EDIIsMosiacism,
                            EDIIsPunctations = screening.EDIIsPunctations,
                            EDIIsQueryICC = screening.EDIIsQueryICC,
                            EDIStateType = screening.EDIStateType,
                            EDITestResult = screening.EDITestResult,
                            EDIWhyNotDone = screening.EDIWhyNotDone,
                            EncounterType = screening.EncounterType,
                            HPVGenoTypeComment = screening.HPVGenoTypeFound,
                            HPVGenoTypeFound = screening.HPVGenoTypeFound,
                            HPVTestResult = screening.HPVTestResult,
                            HPVTestType = screening.HPVTestType,
                            IsDeleted = screening.IsDeleted,
                            IsVIADone = screening.IsVIADone,
                            IsEDIDone = screening.IsEDIDone,
                            IsEDITransformationZoneSeen = screening.IsEDITransformationZoneSeen,
                            IsPapSmearDone = screening.IsPapSmearDone,
                            IsSynced = screening.IsSynced,
                            IsVIATransformationZoneSeen = screening.IsVIATransformationZoneSeen,
                            ModifiedBy = screening.ModifiedBy,
                            ModifiedIn = screening.ModifiedIn,
                            PapSmearComment = screening.PapSmearComment,
                            PapSmearGrade = screening.PapSmearGrade,
                            PapSmearTestResult = screening.PapSmearTestResult,
                            VIAComments = screening.VIAComments,
                            VIAIsAtypicalVessels = screening.VIAIsAtypicalVessels,
                            VIAIsEctopy = screening.VIAIsEctopy,
                            VIAIsLesionCovers = screening.VIAIsLesionCovers,
                            VIAIsLesionExtendedIntoCervicalOs = screening.VIAIsLesionExtendedIntoCervicalOs,
                            VIAIsLesionSeen = screening.VIAIsLesionSeen,
                            VIAIsMosiacism = screening.VIAIsMosiacism,
                            VIAIsPunctations = screening.VIAIsPunctations,
                            VIAIsQueryICC = screening.VIAIsQueryICC,
                            VIAStateType = screening.VIAStateType,
                            VIATestResult = screening.VIATestResult,
                            VIAWhyNotDone = screening.VIAWhyNotDone,
                            ClientId = screening.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == screening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == screening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                            // Add other properties as needed
                        })

                    .OrderByDescending(x => x.EncounterDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Screening> GetScreeningByKey(Guid key)
        {
            try
            {
                Screening screening = await context.Screenings.AsNoTracking().FirstOrDefaultAsync(b => b.InteractionId == key && b.IsDeleted == false);

                if (screening is not null)
                {
                    screening.EncounterDate = await context.Encounters.Where(x => x.Oid == screening.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                    screening.ClinicianName = await context.UserAccounts.Where(x => x.Oid == screening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    screening.FacilityName = await context.Facilities.Where(x => x.Oid == screening.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                }

                return screening;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// The method is used to get a Screening by clientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Returns a  ScreeningbyClienId if the clientId is matched.</returns>

        public async Task<IEnumerable<Screening>> GetScreeningbyClienId(Guid clientId)
        {
            try
            {
                return await context.Screenings.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
                    .Join(
                        context.Encounters.AsNoTracking(),
                        screening => screening.EncounterId,
                        encounter => encounter.Oid,
                        (screening, encounter) => new Screening
                        {
                            InteractionId = screening.InteractionId,
                            CreatedBy = screening.CreatedBy,
                            EncounterId = screening.EncounterId,
                            CreatedIn = screening.CreatedIn,
                            DateCreated = screening.DateCreated,
                            DateModified = screening.DateModified,
                            EDIComments = screening.EDIComments,
                            EDIIsAtypicalVessels = screening.EDIIsAtypicalVessels,
                            EDIIsEctopy = screening.EDIIsEctopy,
                            EDIIsLesionCoversMoreThreeQuaters = screening.EDIIsLesionCoversMoreThreeQuaters,
                            EDIIsLesionExtendedIntoCervicalOs = screening.EDIIsLesionExtendedIntoCervicalOs,
                            EDIIsLesionSeen = screening.EDIIsLesionSeen,
                            EDIIsMosiacism = screening.EDIIsMosiacism,
                            EDIIsPunctations = screening.EDIIsPunctations,
                            EDIIsQueryICC = screening.EDIIsQueryICC,
                            EDIStateType = screening.EDIStateType,
                            EDITestResult = screening.EDITestResult,
                            EDIWhyNotDone = screening.EDIWhyNotDone,
                            EncounterType = screening.EncounterType,
                            HPVGenoTypeComment = screening.HPVGenoTypeFound,
                            HPVGenoTypeFound = screening.HPVGenoTypeFound,
                            HPVTestResult = screening.HPVTestResult,
                            HPVTestType = screening.HPVTestType,
                            IsDeleted = screening.IsDeleted,
                            IsVIADone = screening.IsVIADone,
                            IsEDIDone = screening.IsEDIDone,
                            IsEDITransformationZoneSeen = screening.IsEDITransformationZoneSeen,
                            IsPapSmearDone = screening.IsPapSmearDone,
                            IsSynced = screening.IsSynced,
                            IsVIATransformationZoneSeen = screening.IsVIATransformationZoneSeen,
                            ModifiedBy = screening.ModifiedBy,
                            ModifiedIn = screening.ModifiedIn,
                            PapSmearComment = screening.PapSmearComment,
                            PapSmearGrade = screening.PapSmearGrade,
                            PapSmearTestResult = screening.PapSmearTestResult,
                            VIAComments = screening.VIAComments,
                            VIAIsAtypicalVessels = screening.VIAIsAtypicalVessels,
                            VIAIsEctopy = screening.VIAIsEctopy,
                            VIAIsLesionCovers = screening.VIAIsLesionCovers,
                            VIAIsLesionExtendedIntoCervicalOs = screening.VIAIsLesionExtendedIntoCervicalOs,
                            VIAIsLesionSeen = screening.VIAIsLesionSeen,
                            VIAIsMosiacism = screening.VIAIsMosiacism,
                            VIAIsPunctations = screening.VIAIsPunctations,
                            VIAIsQueryICC = screening.VIAIsQueryICC,
                            VIAStateType = screening.VIAStateType,
                            VIATestResult = screening.VIATestResult,
                            VIAWhyNotDone = screening.VIAWhyNotDone,
                            ClientId = screening.ClientId,
                            EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                            ClinicianName = context.UserAccounts.Where(x => x.Oid == screening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                            FacilityName = context.Facilities.Where(x => x.Oid == screening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",


                            // Add other properties as needed
                        }).OrderByDescending(x => x.EncounterDate).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// </summary>
        /// <returns>Returns a list of all Screening by EncounterID.</returns>
        public async Task<IEnumerable<Screening>> GetScreeningByEncounter(Guid encounterId)
        {
            try
            {
                return await context.Screenings.AsNoTracking().Where(x => x.EncounterId == encounterId && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          screening => screening.EncounterId,
                          encounter => encounter.Oid,
                          (screening, encounter) => new Screening
                          {
                              InteractionId = screening.InteractionId,
                              CreatedBy = screening.CreatedBy,
                              EncounterId = screening.EncounterId,
                              CreatedIn = screening.CreatedIn,
                              DateCreated = screening.DateCreated,
                              DateModified = screening.DateModified,
                              EDIComments = screening.EDIComments,
                              EDIIsAtypicalVessels = screening.EDIIsAtypicalVessels,
                              EDIIsEctopy = screening.EDIIsEctopy,
                              EDIIsLesionCoversMoreThreeQuaters = screening.EDIIsLesionCoversMoreThreeQuaters,
                              EDIIsLesionExtendedIntoCervicalOs = screening.EDIIsLesionExtendedIntoCervicalOs,
                              EDIIsLesionSeen = screening.EDIIsLesionSeen,
                              EDIIsMosiacism = screening.EDIIsMosiacism,
                              EDIIsPunctations = screening.EDIIsPunctations,
                              EDIIsQueryICC = screening.EDIIsQueryICC,
                              EDIStateType = screening.EDIStateType,
                              EDITestResult = screening.EDITestResult,
                              EDIWhyNotDone = screening.EDIWhyNotDone,
                              EncounterType = screening.EncounterType,
                              HPVGenoTypeComment = screening.HPVGenoTypeFound,
                              HPVGenoTypeFound = screening.HPVGenoTypeFound,
                              HPVTestResult = screening.HPVTestResult,
                              HPVTestType = screening.HPVTestType,
                              IsDeleted = screening.IsDeleted,
                              IsVIADone = screening.IsVIADone,
                              IsEDIDone = screening.IsEDIDone,
                              IsEDITransformationZoneSeen = screening.IsEDITransformationZoneSeen,
                              IsPapSmearDone = screening.IsPapSmearDone,
                              IsSynced = screening.IsSynced,
                              IsVIATransformationZoneSeen = screening.IsVIATransformationZoneSeen,
                              ModifiedBy = screening.ModifiedBy,
                              ModifiedIn = screening.ModifiedIn,
                              PapSmearComment = screening.PapSmearComment,
                              PapSmearGrade = screening.PapSmearGrade,
                              PapSmearTestResult = screening.PapSmearTestResult,
                              VIAComments = screening.VIAComments,
                              VIAIsAtypicalVessels = screening.VIAIsAtypicalVessels,
                              VIAIsEctopy = screening.VIAIsEctopy,
                              VIAIsLesionCovers = screening.VIAIsLesionCovers,
                              VIAIsLesionExtendedIntoCervicalOs = screening.VIAIsLesionExtendedIntoCervicalOs,
                              VIAIsLesionSeen = screening.VIAIsLesionSeen,
                              VIAIsMosiacism = screening.VIAIsMosiacism,
                              VIAIsPunctations = screening.VIAIsPunctations,
                              VIAIsQueryICC = screening.VIAIsQueryICC,
                              VIAStateType = screening.VIAStateType,
                              VIATestResult = screening.VIATestResult,
                              VIAWhyNotDone = screening.VIAWhyNotDone,
                              ClientId = screening.ClientId,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == screening.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == screening.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                              // Add other properties as needed
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
