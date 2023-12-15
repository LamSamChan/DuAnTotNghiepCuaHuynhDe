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

        public async Task<List<CountCustomerAdded>> CountCustomerAddedByDate(DateTime startDate,DateTime endDate)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate.ToString("yyyy/MM/dd"));
                parameters.Add("@EndDate", endDate.ToString("yyyy/MM/dd"));

                var result = connection.Query<CountCustomerAdded>("CountCustomersByTypeAndDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        public async Task<List<CountCustomerAdded>> CountCustomerAddedByWeek(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@MonthYear", year+"-"+month);

                var result = connection.Query<CountCustomerAdded>("CountCustomersByWeekAndMonth", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        public async Task<List<CountCustomerAdded>> CountCustomerAddedByMonth(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceMonth", year+"-"+month+"-01");

                var result = connection.Query<CountCustomerAdded>("CountMonthsInLastSixMonths", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }





        public async Task<List<CountPendingContractCreadted>> CountPContractByDate(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate.ToString("yyyy/MM/dd"));
                parameters.Add("@EndDate", endDate.ToString("yyyy/MM/dd"));

                var result = connection.Query<CountPendingContractCreadted>("SumPendingContractByDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountPContractByWeek(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@MonthYear", year + "-" + month);

                var result = connection.Query<CountPendingContractCreadted>("CountPendingContractByWeekAndMonth", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountPContractByMonth(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceMonth", year + "-" + month + "-01");

                var result = connection.Query<CountPendingContractCreadted>("CountPContractMonthsInLastSixMonths", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }





        public async Task<List<CountPendingContractCreadted>> CountPContractWaitCusByDate(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate.ToString("yyyy/MM/dd"));
                parameters.Add("@EndDate", endDate.ToString("yyyy/MM/dd"));

                var result = connection.Query<CountPendingContractCreadted>("SumPendingContractWaitCusByDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountPContractWaitCusByWeek(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@MonthYear", year + "-" + month);

                var result = connection.Query<CountPendingContractCreadted>("CountPendingContractWaitCusByWeekAndMonth", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountPContractWaitCusByMonth(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceMonth", year + "-" + month + "-01");

                var result = connection.Query<CountPendingContractCreadted>("CountPContractWaitCusMonthsInLastSixMonths", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }




        public async Task<List<CountPendingContractCreadted>> CountDContractByDate(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate.ToString("yyyy/MM/dd"));
                parameters.Add("@EndDate", endDate.ToString("yyyy/MM/dd"));

                var result = connection.Query<CountPendingContractCreadted>("SumDoneContractByDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountDContractByWeek(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@MonthYear", year + "-" + month);

                var result = connection.Query<CountPendingContractCreadted>("CountDoneContractByWeekAndMonth", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountDContractByMonth(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceMonth", year + "-" + month + "-01");

                var result = connection.Query<CountPendingContractCreadted>("CountDContractMonthsInLastSixMonths", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }




        public async Task<List<CountPendingContractCreadted>> CountUnEffectByDate(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate.ToString("yyyy/MM/dd"));
                parameters.Add("@EndDate", endDate.ToString("yyyy/MM/dd"));

                var result = connection.Query<CountPendingContractCreadted>("SumUnEffectContractByDate", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountUnEffectByWeek(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@MonthYear", year + "-" + month);

                var result = connection.Query<CountPendingContractCreadted>("CountUnEffectContractByWeekAndMonth", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<List<CountPendingContractCreadted>> CountUnEffectByMonth(DateTime monthData)
        {
            string year = monthData.Year.ToString();
            string month = monthData.Month.ToString();
            if (int.Parse(month) < 10)
            {
                month = "0" + month;
            }
            using (var connection = new SqlConnection(_configuration.GetConnectionString("connection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceMonth", year + "-" + month + "-01");

                var result = connection.Query<CountPendingContractCreadted>("CountUnEffectContractMonthsInLastSixMonths", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
    }
}
