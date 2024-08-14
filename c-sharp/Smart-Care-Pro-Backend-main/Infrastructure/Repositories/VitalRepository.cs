using Domain.Entities;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using static Utilities.Constants.Enums;

/*
*Created by: Stephan
* Date created: 29.04.2023
* Modified by: Stephan
* Last modified: 13.08.2023
* Reviewed by:
*Date reviewed:
*/

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IVitalRepository interface.
    /// </summary>
    public class VitalRepository : Repository<Vital>, IVitalRepository
    {
        public VitalRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a vital by key.
        /// </summary>
        /// <param name="key">Primary key of the table Vitals.</param>
        /// <returns>Returns a vital if the key is matched.</returns>
        public async Task<Vital> GetVitalByKey(Guid key)
        {
            try
            {
                return await context.Vitals.AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         vital => vital.EncounterId,
         encounter => encounter.Oid,
         (vital, encounter) => new Vital
         {
             AbdominalCircumference = vital.AbdominalCircumference,
             EncounterId = vital.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = vital.DateCreated,
             BMI = vital.BMI,
             ClientId = vital.ClientId,
             Comment = vital.Comment,
             CreatedBy = vital.CreatedBy,
             CreatedIn = vital.CreatedIn,
             DateModified = vital.DateModified,
             Client = vital.Client,
             Diastolic = vital.Diastolic,
             DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
             EncounterType = vital.EncounterType,
             HCScore = vital.HCScore,
             HeadCircumference = vital.HeadCircumference,
             Height = vital.Height,
             IsDeleted = vital.IsDeleted,
             IsSynced = vital.IsSynced,
             ModifiedBy = vital.ModifiedBy,
             ModifiedIn = vital.ModifiedIn,
             MUAC = vital.MUAC,
             MUACScore = vital.MUACScore,
             Oid = vital.Oid,
             OxygenSaturation = vital.OxygenSaturation,
             PulseRate = vital.PulseRate,
             RandomBloodSugar = vital.RandomBloodSugar,
             RespiratoryRate = vital.RespiratoryRate,
             Systolic = vital.Systolic,
             SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
             Temperature = vital.Temperature,
             VitalsDate = vital.VitalsDate,
             Weight = vital.Weight,
             ClinicianName = context.UserAccounts.Where(x => x.Oid == vital.CreatedBy).Select(x => x.FirstName + " " + x.Surname).FirstOrDefault() ?? "",
             FacilityName = context.Facilities.Where(x => x.Oid == vital.CreatedIn).Select(x => x.Description).FirstOrDefault() ?? "",

         }).Where(x => x.IsDeleted == false && x.Oid == key).FirstOrDefaultAsync();
                //return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of vitals.
        /// </summary>
        /// <returns>Returns a list of all vitals.</returns>
        public async Task<IEnumerable<Vital>> GetVitals(Guid clientId)
        {
            try
            {
                return await context.Vitals.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
             vital => vital.EncounterId,
             encounter => encounter.Oid,
             (vital, encounter) => new Vital
             {
                 AbdominalCircumference = vital.AbdominalCircumference,
                 EncounterId = vital.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 DateCreated = vital.DateCreated,
                 BMI = vital.BMI,
                 ClientId = vital.ClientId,
                 Comment = vital.Comment,
                 CreatedBy = vital.CreatedBy,
                 CreatedIn = vital.CreatedIn,
                 DateModified = vital.DateModified,
                 //Client = vital.Client,
                 Diastolic = vital.Diastolic,
                 DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
                 EncounterType = vital.EncounterType,
                 HCScore = vital.HCScore,
                 HeadCircumference = vital.HeadCircumference,
                 Height = vital.Height,
                 IsDeleted = vital.IsDeleted,
                 IsSynced = vital.IsSynced,
                 ModifiedBy = vital.ModifiedBy,
                 ModifiedIn = vital.ModifiedIn,
                 MUAC = vital.MUAC,
                 MUACScore = vital.MUACScore,
                 Oid = vital.Oid,
                 OxygenSaturation = vital.OxygenSaturation,
                 PulseRate = vital.PulseRate,
                 RandomBloodSugar = vital.RandomBloodSugar,
                 RespiratoryRate = vital.RespiratoryRate,
                 Systolic = vital.Systolic,
                 SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
                 Temperature = vital.Temperature,
                 VitalsDate = vital.VitalsDate,
                 Weight = vital.Weight,

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Vital>> GetVitals(Guid clientId, int page, int pageSize, EncounterType? encounterType)
        {
            try
            {
                var vitals = context.Vitals.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         vital => vital.EncounterId,
         encounter => encounter.Oid,
         (vital, encounter) => new Vital
         {
             AbdominalCircumference = vital.AbdominalCircumference,
             EncounterId = vital.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = vital.DateCreated,
             BMI = vital.BMI,
             ClientId = vital.ClientId,
             Comment = vital.Comment,
             CreatedBy = vital.CreatedBy,
             CreatedIn = vital.CreatedIn,
             DateModified = vital.DateModified,
             //Client = vital.Client,
             Diastolic = vital.Diastolic,
             DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
             EncounterType = vital.EncounterType,
             HCScore = vital.HCScore,
             HeadCircumference = vital.HeadCircumference,
             Height = vital.Height,
             IsDeleted = vital.IsDeleted,
             IsSynced = vital.IsSynced,
             ModifiedBy = vital.ModifiedBy,
             ModifiedIn = vital.ModifiedIn,
             MUAC = vital.MUAC,
             MUACScore = vital.MUACScore,
             Oid = vital.Oid,
             OxygenSaturation = vital.OxygenSaturation,
             PulseRate = vital.PulseRate,
             RandomBloodSugar = vital.RandomBloodSugar,
             RespiratoryRate = vital.RespiratoryRate,
             Systolic = vital.Systolic,
             SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
             Temperature = vital.Temperature,
             VitalsDate = vital.VitalsDate,
             Weight = vital.Weight,

         }).AsQueryable();

                if (encounterType == null)
                    return await vitals.OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();
                else
                    return await vitals.Where(p => p.EncounterType == encounterType).OrderByDescending(x => x.EncounterDate).Skip(page).Take(pageSize).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetVitalsTotalCount(Guid clientID, EncounterType? encounterType)
        {
            if (encounterType == null)
                return context.Vitals.Where(x => x.IsDeleted == false && x.ClientId == clientID).Count();
            else
                return context.Vitals.Where(x => x.IsDeleted == false && x.ClientId == clientID && x.EncounterType == encounterType).Count();
        }
        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<IEnumerable<Vital>> GetVitalsByClient(Guid clientId)
        {
            try
            {
                return await context.Vitals.Where(p => p.IsDeleted == false && p.ClientId == clientId).AsNoTracking()
         .Join(
             context.Encounters.AsNoTracking(),
             vital => vital.EncounterId,
             encounter => encounter.Oid,
             (vital, encounter) => new Vital
             {
                 AbdominalCircumference = vital.AbdominalCircumference,
                 EncounterId = vital.EncounterId,
                 EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
                 DateCreated = vital.DateCreated,
                 BMI = vital.BMI,
                 ClientId = vital.ClientId,
                 Comment = vital.Comment,
                 CreatedBy = vital.CreatedBy,
                 CreatedIn = vital.CreatedIn,
                 DateModified = vital.DateModified,
                 //Client = vital.Client,
                 Diastolic = vital.Diastolic,
                 DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
                 EncounterType = vital.EncounterType,
                 HCScore = vital.HCScore,
                 HeadCircumference = vital.HeadCircumference,
                 Height = vital.Height,
                 IsDeleted = vital.IsDeleted,
                 IsSynced = vital.IsSynced,
                 ModifiedBy = vital.ModifiedBy,
                 ModifiedIn = vital.ModifiedIn,
                 MUAC = vital.MUAC,
                 MUACScore = vital.MUACScore,
                 Oid = vital.Oid,
                 OxygenSaturation = vital.OxygenSaturation,
                 PulseRate = vital.PulseRate,
                 RandomBloodSugar = vital.RandomBloodSugar,
                 RespiratoryRate = vital.RespiratoryRate,
                 Systolic = vital.Systolic,
                 SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
                 Temperature = vital.Temperature,
                 VitalsDate = vital.VitalsDate,
                 Weight = vital.Weight,

             }).OrderByDescending(x => x.EncounterDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by key.
        /// </summary>
        /// <param name="key">Primary key of the table Clients.</param>
        /// <returns>Returns a client if the key is matched.</returns>
        public async Task<Vital> GetLatestVitalByClient(Guid clientId)
        {
            return await context.Vitals.Where(x => x.ClientId == clientId && x.IsDeleted == false).Include(c => c.Client).AsNoTracking()
     .Join(
         context.Encounters.AsNoTracking(),
         vital => vital.EncounterId,
         encounter => encounter.Oid,
         (vital, encounter) => new Vital
         {
             AbdominalCircumference = vital.AbdominalCircumference,
             EncounterId = vital.EncounterId,
             EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
             DateCreated = vital.DateCreated,
             BMI = vital.BMI,
             ClientId = vital.ClientId,
             Comment = vital.Comment,
             CreatedBy = vital.CreatedBy,
             CreatedIn = vital.CreatedIn,
             DateModified = vital.DateModified,
             //Client = vital.Client,
             Diastolic = vital.Diastolic,
             DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
             EncounterType = vital.EncounterType,
             HCScore = vital.HCScore,
             HeadCircumference = vital.HeadCircumference,
             Height = vital.Height,
             IsDeleted = vital.IsDeleted,
             IsSynced = vital.IsSynced,
             ModifiedBy = vital.ModifiedBy,
             ModifiedIn = vital.ModifiedIn,
             MUAC = vital.MUAC,
             MUACScore = vital.MUACScore,
             Oid = vital.Oid,
             OxygenSaturation = vital.OxygenSaturation,
             PulseRate = vital.PulseRate,
             RandomBloodSugar = vital.RandomBloodSugar,
             RespiratoryRate = vital.RespiratoryRate,
             Systolic = vital.Systolic,
             SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
             Temperature = vital.Temperature,
             VitalsDate = vital.VitalsDate,
             Weight = vital.Weight,

         }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();
            // return await LoadWithChildWithOrderByAsync<Vital>(c => c.ClientId == clientId && c.IsDeleted == false, orderBy: x => x.OrderByDescending(y => y.DateCreated), p => p.Client);
        }
        public async Task<Vital> GetLatestVitalByClientAndEncounterType(Guid clientId, EncounterType encounterType)
        {
            return await context.Vitals.Where(x => x.ClientId == clientId && x.EncounterType == encounterType && x.IsDeleted == false).Include(c => c.Client).AsNoTracking()
   .Join(
       context.Encounters.AsNoTracking(),
       vital => vital.EncounterId,
       encounter => encounter.Oid,
       (vital, encounter) => new Vital
       {
           AbdominalCircumference = vital.AbdominalCircumference,
           EncounterId = vital.EncounterId,
           EncounterDate = encounter.OPDVisitDate ?? encounter.IPDAdmissionDate ?? encounter.DateCreated,
           DateCreated = vital.DateCreated,
           BMI = vital.BMI,
           ClientId = vital.ClientId,
           Comment = vital.Comment,
           CreatedBy = vital.CreatedBy,
           CreatedIn = vital.CreatedIn,
           DateModified = vital.DateModified,
           Client = vital.Client,
           Diastolic = vital.Diastolic,
           DiastolicIfUnrecordable = vital.DiastolicIfUnrecordable,
           EncounterType = vital.EncounterType,
           HCScore = vital.HCScore,
           HeadCircumference = vital.HeadCircumference,
           Height = vital.Height,
           IsDeleted = vital.IsDeleted,
           IsSynced = vital.IsSynced,
           ModifiedBy = vital.ModifiedBy,
           ModifiedIn = vital.ModifiedIn,
           MUAC = vital.MUAC,
           MUACScore = vital.MUACScore,
           Oid = vital.Oid,
           OxygenSaturation = vital.OxygenSaturation,
           PulseRate = vital.PulseRate,
           RandomBloodSugar = vital.RandomBloodSugar,
           RespiratoryRate = vital.RespiratoryRate,
           Systolic = vital.Systolic,
           SystolicIfUnrecordable = vital.SystolicIfUnrecordable,
           Temperature = vital.Temperature,
           VitalsDate = vital.VitalsDate,
           Weight = vital.Weight,

       }).OrderByDescending(x => x.EncounterDate).FirstOrDefaultAsync();
            // return await LoadWithChildWithOrderByAsync<Vital>(c => c.ClientId == clientId && c.EncounterType == encounterType && c.IsDeleted == false, orderBy: x => x.OrderByDescending(y => y.DateCreated), p => p.Client);
        }
    }
}