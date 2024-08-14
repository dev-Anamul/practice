using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.X509Certificates;
using static Utilities.Constants.Enums;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Bithy
 * Date created : 25.12.2022
 * Modified by  : Rezwana
 * Last modified: 27.12.2022
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IChiefComplaintRepository interface.
    /// </summary>
    public class ChiefComplaintRepository : Repository<ChiefComplaint>, IChiefComplaintRepository
    {
        public ChiefComplaintRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a chief complaint by key.
        /// </summary>
        /// <param name="key">Primary key of the table ChiefComplaints.</param>
        /// <returns>Returns a chief complaint if the key is matched.</returns>
        public async Task<ChiefComplaint> GetChiefComplaintByKey(Guid key)
        {
            try
            {
                var data = await context.ChiefComplaints.AsNoTracking().FirstOrDefaultAsync(c => c.InteractionId == key && c.IsDeleted == false);
               
                if (data != null)
                {
                    data.ClinicianName = await context.UserAccounts.Where(x => x.Oid == data.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    data.FacilityName = await context.Facilities.Where(x => x.Oid == data.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    data.EncounterDate = await context.Encounters.Where(x => x.Oid == data.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of chief complaints.
        /// </summary>
        /// <returns>Returns a list of all chief complaints.</returns>
        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaints()
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,

                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        /// The method is used to get a OPDVisit by key.
        /// </summary>
        /// <param name="key">Primary key of the table OPDVisit.</param>
        /// <returns>Returns a OPDVisit if the key is matched.</returns>
        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByOpdVisit(Guid OPDVisitID)
        {

            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.EncounterId == OPDVisitID && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByOpdVisitEncounterType(Guid OPDVisitID, EncounterType encouterType)
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.EncounterId == OPDVisitID && x.EncounterType == encouterType && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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

        public async Task<ChiefComplaint> GetChiefComplaintByOpdVisit(Guid EncounterID)
        {
            try
            {
                //return await LastOrDefaultAsync(c => c.EncounterID == EncounterID && c.IsDeleted == false);
                var data = await context.ChiefComplaints.AsNoTracking().FirstOrDefaultAsync(c => c.EncounterId == EncounterID);

                if (data != null)
                {
                    data.ClinicianName = await context.UserAccounts.Where(x => x.Oid == data.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefaultAsync() ?? "";
                    data.FacilityName = await context.Facilities.Where(x => x.Oid == data.CreatedIn).Select(x => x.Description).FirstOrDefaultAsync() ?? "";
                    data.EncounterDate = await context.Encounters.Where(x => x.Oid == data.EncounterId).Select(x => x.OPDVisitDate ?? x.IPDAdmissionDate ?? x.DateCreated).FirstOrDefaultAsync();
                }

                return data;
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
        public async Task<Tuple<int, IEnumerable<ChiefComplaint>>> GetChiefComplaintsByClient(Guid clientID, int pageNo, int pageSize)
        {

            //var data = await QueryAsync(c => c.IsDeleted == false && c.ClientId == clientID);
            var data = await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

                          }
                      )

                      .OrderByDescending(b => b.EncounterDate)
                      .ToListAsync();
            return new Tuple<int, IEnumerable<ChiefComplaint>>(data.Count(), data.Skip(pageSize * (pageNo - 1))
               .Take(pageSize));
        }

        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClient(Guid clientID)
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClient(Guid clientID, EncounterType encouterType)
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.EncounterType == encouterType && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        public async Task<IEnumerable<ChiefComplaint>> GetChiefComplaintsByClientPagging(Guid clientID, int page, int pageSize, EncounterType? encouterType)
        {
            try
            {
                var chiefComplaintAsQueryable = context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
                          }
                      ).AsQueryable();
                if (encouterType == null)
                    return await chiefComplaintAsQueryable.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await chiefComplaintAsQueryable.Where(x => x.EncounterType == encouterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetChiefComplaintsByClientPaggingTotalCount(Guid clientID, Utilities.Constants.Enums.EncounterType? encouterType)
        {
            if (encouterType == null)
                return context.ChiefComplaints.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.ChiefComplaints.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encouterType).Count();

        }
        public async Task<IEnumerable<ChiefComplaint>> GetPEPChiefComplaintsByClient(Guid clientID)
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.EncounterType == Utilities.Constants.Enums.EncounterType.PEP && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        public async Task<IEnumerable<ChiefComplaint>> GetPEPChiefComplaintsByClientLast24Hours(Guid clientID)
        {
            try
            {
                DateTime Last24Hours = DateTime.Now.AddHours(-24);

                return await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.DateCreated >= Last24Hours && x.EncounterType == Utilities.Constants.Enums.EncounterType.PEP && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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

        public async Task<IEnumerable<ChiefComplaint>> GetPrEPChiefComplaintsByClient(Guid clientID)
        {
            try
            {
                return await context.ChiefComplaints.AsNoTracking().Where(x => x.ClientId == clientID && x.EncounterType == Utilities.Constants.Enums.EncounterType.PrEP && x.IsDeleted == false).Join(
                          context.Encounters.AsNoTracking(),
                          chiefComplaint => chiefComplaint.EncounterId,
                          encounter => encounter.Oid,
                          (chiefComplaint, encounter) => new ChiefComplaint
                          {
                              InteractionId = chiefComplaint.InteractionId,
                              ChiefComplaints = chiefComplaint.ChiefComplaints,
                              HistoryOfChiefComplaint = chiefComplaint.HistoryOfChiefComplaint,
                              HistorySummary = chiefComplaint.HistorySummary,
                              ExaminationSummary = chiefComplaint.ExaminationSummary,
                              HIVStatus = chiefComplaint.HIVStatus,
                              LastHIVTestDate = chiefComplaint.LastHIVTestDate,
                              TestingLocation = chiefComplaint.TestingLocation,
                              PotentialHIVExposureDate = chiefComplaint.PotentialHIVExposureDate,
                              RecencyType = chiefComplaint.RecencyType,
                              RecencyTestDate = chiefComplaint.RecencyTestDate,
                              ChildExposureStatus = chiefComplaint.ChildExposureStatus,
                              IsChildGivenARV = chiefComplaint.IsChildGivenARV,
                              IsMotherGivenARV = chiefComplaint.IsMotherGivenARV,
                              NATTestDate = chiefComplaint.NATTestDate,
                              NATResult = chiefComplaint.NATResult,
                              TBScreenings = chiefComplaint.TBScreenings,
                              ClientId = chiefComplaint.ClientId,
                              ExposureList = chiefComplaint.ExposureList,
                              keyPopulationDemographics = chiefComplaint.keyPopulationDemographics,
                              hIVRiskScreenings = chiefComplaint.hIVRiskScreenings,
                              // Include other properties as needed
                              EncounterId = chiefComplaint.EncounterId,
                              EncounterType = chiefComplaint.EncounterType,
                              CreatedIn = chiefComplaint.CreatedIn,
                              DateCreated = chiefComplaint.DateCreated,
                              CreatedBy = chiefComplaint.CreatedBy,
                              ModifiedIn = chiefComplaint.ModifiedIn,
                              DateModified = chiefComplaint.DateModified,
                              ModifiedBy = chiefComplaint.ModifiedBy,
                              IsDeleted = chiefComplaint.IsDeleted,
                              IsSynced = chiefComplaint.IsSynced,
                              EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                              ClinicianName = context.UserAccounts.Where(x => x.Oid == chiefComplaint.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
                              FacilityName = context.Facilities.Where(x => x.Oid == chiefComplaint.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",
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
        /// The method is used to get a Client by with pagination
        /// </summary>
        /// <param name="clientID">Primary key of the table Client.</param>
        /// <param name="pageNo">Primary key of the table Client.</param>
        /// <param name="pageSize">Primary key of the table Client.</param>
        /// <returns>Returns a Client if the key is matched.</returns>
        public async Task<Tuple<int, IEnumerable<ChiefComplaint>>> GetChiefComplaintsWithPagination(Guid clientID, int pageNo, int pageSize)
        {

            var rows = await QueryAsync(c => c.IsDeleted == false && c.ClientId == clientID);
            var data = context.ChiefComplaints.Where(c => c.IsDeleted == false && c.ClientId == clientID).Skip(pageSize * (pageNo - 1))
               .Take(pageSize).AsEnumerable();
            return new Tuple<int, IEnumerable<ChiefComplaint>>(rows.Count(), data);
        }
    }
}