using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models.ModelProcedure;
using System.Data;

namespace QuanLyHopDongVaKySo_API.Services.ExecProcedureService
{
    public class ExecProcedureServices : IExecProcedureServices
    {
        private readonly ProjectDbContext _context;
        private readonly IConfiguration _configuration;

        public ExecProcedureServices(ProjectDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<CountCustomerAdded>> CountCustomerAdded()
        {
            DateTime endDate = DateTime.Today.AddDays(1);
            DateTime startDate = DateTime.Today.AddMonths(-3).AddDays(-1);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate);
                parameters.Add("@EndDate", endDate);

                var result = connection.Query<CountCustomerAdded>("CountCustomersByTypeAndDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
    }
}
