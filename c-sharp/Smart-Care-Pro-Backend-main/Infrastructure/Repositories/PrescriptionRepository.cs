using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

/*
 * Created by   : Tariqul Islam
 * Date created : 12-03-2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="key">Primary key of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public async Task<Prescription> GetPrescriptionByKey(Guid key)
        {
            try
            {
                return await LoadWithChildAsync<Prescription>(p => p.InteractionId == key && p.IsDeleted == false, c => c.Client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="ClientId">ClientId of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public async Task<IEnumerable<Prescription>> GetPrescriptionsByClientId(Guid clientId)
        {
            try
            {
                return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.ClientId == clientId, m => m.Medications);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Prescription by key.
        /// </summary>
        /// <param name="ClientId">ClientId of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public async Task<IEnumerable<Prescription>> GetPrescriptionsForDispenseByClientId(Guid clientId)
        {
            try
            {
                return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.DispensationDate == null && p.ClientId == clientId, m => m.Medications);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a Prescription by today's date.
        /// </summary>
        /// <param>All of today of the table Prescription.</param>
        /// <returns>Returns a Prescription if the key is matched.</returns>
        public async Task<IEnumerable<Prescription>> GetPrescriptionsByToday()
        {
            try
            {
                double validHours = -24;

                var latest = DateTime.Now.AddHours(validHours);

                return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.DateCreated > latest, c => c.Client, m => m.Medications);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Prescription>> GetPharmacyDashBoard(int FacilityId, int skip, int take, string PatientName, string PreCriptionDateSearch)
        {
            try
            {
                if (string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
                {
                    return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId, skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications);
                }
                else if (!string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
                {
                    string[] searchTerms = PatientName.Split(' ');
                    //        return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId && (searchTerms.Any(term =>
                    //p.Client.FirstName.Contains(term) || p.Client.Surname.Contains(term))), skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications, c => c.Client);
                    return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId && (searchTerms.Contains(p.Client.FirstName)|| searchTerms.Contains(p.Client.Surname)), skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications, c => c.Client);
                }
                else if (string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
                {
                    DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId && p.PrescriptionDate.Date == PreCriptionDate, skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications);
                }
                else if (!string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
                {
                    string[] searchTerms = PatientName.Split(' ');
                    DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId && p.PrescriptionDate.Date == PreCriptionDate && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname)), skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications, c => c.Client);
                }
                else
                {
                    return await LoadListWithChildAsync<Prescription>(p => p.IsDeleted == false && p.CreatedIn == FacilityId, skip, take, orderBy: d => d.OrderByDescending(y => y.PrescriptionDate), c => c.Client, m => m.Medications);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetPharmacyDashBoardTotalCount(int FacilityId, string PatientName, string PreCriptionDateSearch)
        {
            if (string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                return context.Prescriptions.Where(x => x.CreatedIn == FacilityId && x.IsDeleted == false).Count();
            }

            else if (!string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                string[] searchTerms = PatientName.Split(' ');
                return context.Prescriptions.Include(x => x.Client).Where(p => p.CreatedIn == FacilityId && p.IsDeleted == false && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname))).Count();
            }

            else if (string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                return context.Prescriptions.Where(p => p.CreatedIn == FacilityId && p.IsDeleted == false && p.PrescriptionDate.Date == PreCriptionDate).Count();
            }

            else if (!string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                string[] searchTerms = PatientName.Split(' ');
                DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                return context.Prescriptions.Include(x => x.Client).Where(p => p.CreatedIn == FacilityId && p.IsDeleted == false && p.PrescriptionDate.Date == PreCriptionDate && (searchTerms.Contains(p.Client.FirstName) || searchTerms.Contains(p.Client.Surname))).Count();
            }
            return 0;
        }
        public int GetPharmacyDashBoardDispensationTotalCount(int FacilityId, string PatientName, string PreCriptionDateSearch)
        {
            if (string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                return context.Prescriptions.Where(x => x.CreatedIn == FacilityId && x.DispensationDate != null && x.IsDeleted == false).Count();
            }

            else if (!string.IsNullOrEmpty(PatientName) && string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                return context.Prescriptions.Include(x => x.Client).Where(p => p.CreatedIn == FacilityId && (p.Client.FirstName.Contains(PatientName) || p.Client.Surname.Contains(PatientName)) && p.DispensationDate != null).Count();
            }

            else if (string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                return context.Prescriptions.Where(p => p.CreatedIn == FacilityId && p.PrescriptionDate.Date == PreCriptionDate && p.DispensationDate != null).Count();
            }

            else if (!string.IsNullOrEmpty(PatientName) && !string.IsNullOrEmpty(PreCriptionDateSearch))
            {
                DateTime PreCriptionDate = DateTime.ParseExact(PreCriptionDateSearch, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                return context.Prescriptions.Include(x => x.Client).Where(p => p.CreatedIn == FacilityId && p.PrescriptionDate.Date == PreCriptionDate && (p.Client.FirstName.Contains(PatientName) || p.Client.Surname.Contains(PatientName)) && p.DispensationDate != null).Count();
            }

            return 0;
        }
        /// <summary>
        /// The method is used to get the list of Prescription  .
        /// </summary>
        /// <returns>Returns a list of all Prescription.</returns>        
        public async Task<IEnumerable<Prescription>> GetPrescription()
        {
            try
            {
                return await QueryAsync(d => d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}