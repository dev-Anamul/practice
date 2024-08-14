using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Domain.DataModelForReporting;
using Domain.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using Utilities.Constants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

/*
 * Created by   : Brian
 * Date created : 05.11.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date Reviewed:
 */
namespace Api.Controllers
{
    /// <summary>
    /// Report controller.
    /// </summary>

    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    [Authorize]

    public class ReportController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ReportController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        [Route(RouteConstants.ReadARTReports)]
        public async Task<IActionResult> ReadARTReports(string Key)
        {
            try
            {
                List<ARTPsychoSocialCounselling> ARTPsychoSocialCounselling = new();
                DateTime startDate = Convert.ToDateTime(Key.Split(",")[0]);
                DateTime endDate = Convert.ToDateTime(Key.Split(",")[1]);
                int? FacilityId = Convert.ToInt32(Key.Split(",")[2]);

                string connectionString = configuration.GetConnectionString("DBLocation");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_102_ART_PSYCHO_SOCIAL_COUNSELLING", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                        cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new ARTPsychoSocialCounselling
                                {
                                    MaleCount = (int)reader["MaleCount"],
                                    FemaleCount = (int)reader["FemaleCount"],
                                    Total = (int)reader["Total"],
                                    MalePositiveCount = (int)reader["MalePositiveCount"],
                                    FemalePositiveCount = (int)reader["FemalePositiveCount"],
                                    MaleNegativeCount = (int)reader["MaleNegativeCount"],
                                    FemaleNegativeCount = (int)reader["FemaleNegativeCount"],
                                    MalePEPCount = (int)reader["MalePEPCount"],
                                    FemalePEPCount = (int)reader["FemalePEPCount"],
                                    MaleLinkageToCare = (int)reader["MaleLinkageToCare"],
                                    FemaleLinkageToCare = (int)reader["FemaleLinkageToCare"],
                                    TotalLinkageToCare = (int)reader["TotalLinkageToCare"]
                                };

                                ARTPsychoSocialCounselling.Add(result);
                            }
                        }
                    }
                }

                return Ok(ARTPsychoSocialCounselling);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadOPDAttendance)]
        public async Task<IActionResult> ReadOPDAttendance(string Key)
        {
            try
            {
                List<OPDAttendance> OPDAttendanceList = new();
                DateTime startDate = Convert.ToDateTime(Key.Split(",")[0]);
                DateTime endDate = Convert.ToDateTime(Key.Split(",")[1]);
                int? FacilityId = Convert.ToInt32(Key.Split(",")[2]);

                string connectionString = configuration.GetConnectionString("DBLocation");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_101_OPD_Attendance", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                        cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new OPDAttendance
                                {
                                    AdultMale = (int)reader["AdultMale"],
                                    AdultFemale = (int)reader["AdultFemale"],
                                    ChildrenMale = (int)reader["ChildrenMale"],
                                    ChildrenFemale = (int)reader["ChildrenFemale"],
                                    TotalAdults = (int)reader["TotalAdults"],
                                    TotalChildren = (int)reader["TotalChildren"],
                                    TotalMale = (int)reader["TotalMale"],
                                    TotalFemale = (int)reader["TotalFemale"],
                                    TotalAttendence = (int)reader["TotalAttendence"],
                                    AdultReferral = (int)reader["AdultReferral"],
                                    ChildrenReferral = (int)reader["ChildrenReferral"],
                                    TotalReferral = (int)reader["TotalReferral"]
                                };

                                OPDAttendanceList.Add(result);
                            }
                        }
                    }
                }
                return Ok(OPDAttendanceList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadMCH)]
        public async Task<IActionResult> ReadMCH(string Key)
        {
            try
            {
                List<MCH> MCHList = new();
                DateTime startDate = Convert.ToDateTime(Key.Split(",")[0]);
                DateTime endDate = Convert.ToDateTime(Key.Split(",")[1]);
                int? FacilityId = Convert.ToInt32(Key.Split(",")[2]);

                string connectionString = configuration.GetConnectionString("DBLocation");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_103_MCH", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                        cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new MCH
                                {
                                    FirstBookingCount = (int)reader["FirstBookingCount"],
                                    RevisitCount = (int)reader["RevisitCount"],
                                    TotalVisit = (int)reader["TotalVisit"],
                                    FirstVisitReferralCount = (int)reader["FirstVisitReferralCount"],
                                    RevisitReferralCount = (int)reader["RevisitReferralCount"],
                                    HighRiskCount = (int)reader["HighRiskCount"],
                                };
                                MCHList.Add(result);
                            }
                        }
                    }
                }
                return Ok(MCHList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadDiarrhea)]
        public async Task<IActionResult> ReadDiarrhea(string Key)
        {
            try
            {
                List<Diarrhea> DiarrheaList = new();
                DateTime startDate = Convert.ToDateTime(Key.Split(",")[0]);
                DateTime endDate = Convert.ToDateTime(Key.Split(",")[1]);
                int? FacilityId = Convert.ToInt32(Key.Split(",")[2]);

                string connectionString = configuration.GetConnectionString("DBLocation");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_104_DIARRHEA", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                        cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new Diarrhea
                                {
                                    DiarrheaClientCount = (int)reader["DiarrheaClientCount"],
                                };
                                DiarrheaList.Add(result);
                            }
                        }
                    }
                }
                return Ok(DiarrheaList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadCholera)]
        public async Task<IActionResult> ReadCholera(string Key)
        {
            try
            {
                List<Cholera> DiarrheaList = new();
                DateTime startDate = Convert.ToDateTime(Key.Split(",")[0]);
                DateTime endDate = Convert.ToDateTime(Key.Split(",")[1]);
                int? FacilityId = Convert.ToInt32(Key.Split(",")[2]);

                string connectionString = configuration.GetConnectionString("DBLocation");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_105_CHOLERA", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FacilityId", FacilityId));
                        cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new Cholera
                                {
                                    CholeraClientCount = (int)reader["CholeraClientCount"],
                                };
                                DiarrheaList.Add(result);
                            }
                        }
                    }
                }
                return Ok(DiarrheaList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        //API for React CSV Report

        //[HttpGet]
        //[Route(RouteConstants.ReadARTRegisterReport)]
        //public async Task<IActionResult> ReadARTRegisterReport(DateTime startDate, DateTime endDate, int facilityId)
        //{
        //    string connectionString = configuration.GetConnectionString("DBLocation");
        //    SqlConnection Connection = new SqlConnection(connectionString);

        //    List<object> records = new List<object>();

        //    try
        //    {
        //        using var connection = Connection;
        //        connection.Open();
        //        var parameters = new
        //        {
        //            startDate = startDate.ToString("yyyy-MM-dd"),
        //            endDate = endDate.ToString("yyyy-MM-dd"),
        //            facilityId = facilityId
        //        };

        //        var result = await connection.QueryAsync("dbo.scpro_sp_101_GetARTResponses", parameters, commandType: CommandType.StoredProcedure);

        //        records = result.ToList();

        //        //if (records.Count > 0)
        //        //{
        //        //    var csvBuilder = new StringBuilder();
        //        //    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
        //        //    {
        //        //        csv.WriteRecords(records);
        //        //    }

        //        //    var csvContent = csvBuilder.ToString();

        //        //    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

        //        //    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
        //        //}

        //        return Ok(records);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }

        //}

        [HttpPost]
        [Route(RouteConstants.ReadARTRegisterReport)]
        public async Task<IActionResult> ReadARTRegisterReport(ReportParamsDto reportDto)
        {
            string connectionString = configuration.GetConnectionString("DBLocation");
            SqlConnection Connection = new SqlConnection(connectionString);

            List<object> records = new List<object>();

            try
            {
                using var connection = Connection;
                connection.Open();
                var parameters = new
                {
                    startDate = reportDto.StartDate.ToString("yyyy-MM-dd"),
                    endDate = reportDto.EndDate.ToString("yyyy-MM-dd"),
                    facilityId = reportDto.FacilityId
                };

                var result = await connection.QueryAsync("dbo.scpro_sp_101_GetARTResponses", parameters, commandType: CommandType.StoredProcedure);

                records = result.ToList();

                if (records.Count > 0)
                {
                    var csvBuilder = new StringBuilder();
                    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(records);
                    }

                    var csvContent = csvBuilder.ToString();

                    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

                    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route(RouteConstants.ReadAPRegister)]
        public async Task<IActionResult> ReadAPRegister(ReportParamsDto reportDto)
        {
            string connectionString = configuration.GetConnectionString("DBLocation");
            SqlConnection Connection = new SqlConnection(connectionString);

            List<object> records = new List<object>();

            try
            {
                using var connection = Connection;
                connection.Open();
                var parameters = new
                {
                    startDate = reportDto.StartDate.ToString("yyyy-MM-dd"),
                    endDate = reportDto.EndDate.ToString("yyyy-MM-dd"),
                    facilityId = reportDto.FacilityId
                };

                var result = await connection.QueryAsync("dbo.scpro_sp_107_art_appointment_pending", parameters, commandType: CommandType.StoredProcedure);

                records = result.ToList();

                if (records.Count > 0)
                {
                    var csvBuilder = new StringBuilder();
                    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(records);
                    }

                    var csvContent = csvBuilder.ToString();

                    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

                    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet]
        [Route(RouteConstants.ReadHIAOneA)]
        public async Task<IActionResult> ReadHIAOneA(string key)
        {
            return Ok(ReadHIAOneA(key));
        }

        [HttpGet]
        [Route(RouteConstants.ReadHIAOneB)]
        public async Task<IActionResult> ReadHIAOneB (string key)
        {
            return Ok(ReadHIAOneB(key));
        }

        [HttpGet]
        [Route(RouteConstants.ReadHIATwo)]
        public async Task<IActionResult> ReadHIATwo(string key)
        {
            return Ok(ReadHIATwo(key));
        }


        [HttpPost]
        [Route(RouteConstants.ReadLateForPharmacy)]
        public async Task<IActionResult> ReadLateForPharmacy(ReportParamsDto reportDto)
        {
            string connectionString = configuration.GetConnectionString("DBLocation");
            SqlConnection Connection = new SqlConnection(connectionString);

            List<object> records = new List<object>();

            try
            {
                using var connection = Connection;
                connection.Open();
                var parameters = new
                {
                    startDate = reportDto.StartDate.ToString("yyyy-MM-dd"),
                    endDate = reportDto.EndDate.ToString("yyyy-MM-dd"),
                    facilityId = reportDto.FacilityId
                };

                var result = await connection.QueryAsync("dbo.scpro_106_sp_late_for_pharmacy", parameters, commandType: CommandType.StoredProcedure);

                records = result.ToList();

                if (records.Count > 0)
                {
                    var csvBuilder = new StringBuilder();
                    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(records);
                    }

                    var csvContent = csvBuilder.ToString();

                    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

                    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route(RouteConstants.ReadOpdRegister)]
        public async Task<IActionResult> ReadOpdRegister(ReportParamsDto reportDto)
        {
            string connectionString = configuration.GetConnectionString("DBLocation");
            SqlConnection Connection = new SqlConnection(connectionString);

            List<object> records = new List<object>();

            try
            {
                using var connection = Connection;
                connection.Open();
                var parameters = new
                {
                    startDate = reportDto.StartDate.ToString("yyyy-MM-dd"),
                    endDate = reportDto.EndDate.ToString("yyyy-MM-dd"),
                    facilityId = reportDto.FacilityId
                };

                var result = await connection.QueryAsync("dbo.scpro_104_sp_opd_register", parameters, commandType: CommandType.StoredProcedure);

                records = result.ToList();

                if (records.Count > 0)
                {
                    var csvBuilder = new StringBuilder();
                    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(records);
                    }

                    var csvContent = csvBuilder.ToString();

                    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

                    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route(RouteConstants.ReadNdRegister)]
        public async Task<IActionResult> ReadNdRegister(ReportParamsDto reportDto)
        {
            string connectionString = configuration.GetConnectionString("DBLocation");
            SqlConnection Connection = new SqlConnection(connectionString);

            List<object> records = new List<object>();

            try
            {
                using var connection = Connection;
                connection.Open();
                var parameters = new
                {
                    startDate = reportDto.StartDate.ToString("yyyy-MM-dd"),
                    endDate = reportDto.EndDate.ToString("yyyy-MM-dd"),
                    facilityId = reportDto.FacilityId
                };

                var result = await connection.QueryAsync("dbo.scpro_105_sp_nd_register", parameters, commandType: CommandType.StoredProcedure);

                records = result.ToList();

                if (records.Count > 0)
                {
                    var csvBuilder = new StringBuilder();
                    using (var csv = new CsvWriter(new StringWriter(csvBuilder), new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csv.WriteRecords(records);
                    }

                    var csvContent = csvBuilder.ToString();

                    var fileName = $"ARTRegisterDataExport_{DateTime.Now.ToString("dd/MM/yyyyHHmmss")}.csv";

                    return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default; // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}