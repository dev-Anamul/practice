using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System;

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
    /// Implementation of WardRepository class.
    /// </summary>
    public class LogRepository: ILogRepository
    {

        private readonly string _connectionString;
        public LogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBLocation");
        }

        public async Task<Tuple<List<Log>, int>> GetAll(int pageSize, int pageNumber)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_get_alllogs", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters to the SqlCommand
                    cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int) { Value = pageSize });
                    cmd.Parameters.Add(new SqlParameter("@pageNumber", SqlDbType.Int) { Value = pageNumber });

                    var response = new List<Log>();
                    await sql.OpenAsync();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    var table = ds.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        response.Add(MapToValue(row));
                    }
                    return new Tuple<List<Log>, int>(response, (int)ds.Tables[1].Rows[0]["totalRows"]) ;
                }
            }
        }

        private Log MapToValue(DataRow reader)
        {
            return new Log()
            {
                Id = (int)reader["Id"],
                Message = reader["Message"].ToString(),
                MessageTemplate = reader["MessageTemplate"].ToString(),
                Level = reader["Level"].ToString(),
                Exception = reader["Exception"].ToString(),
                Properties = reader["Properties"].ToString(),
                CreateDate = (DateTime) reader["TimeStamp"]
            };
        }
    }
}