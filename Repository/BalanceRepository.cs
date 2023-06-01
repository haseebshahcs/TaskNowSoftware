using Dapper;
using TaskNowSoftware.DbContext;
using TaskNowSoftware.IRepository;
using TaskNowSoftware.Models.Response;

namespace TaskNowSoftware.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly DapperContext _context;
        public BalanceRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<GetBalanceResponseModel> GetBalance(int userId)
        {
            GetBalanceResponseModel obj = new();
            try
            {
                string _query = $@"SELECT TOP 1 BALANCE FROM dbo.USER_BALANCE WHERE USER_ID = @UserId";

                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    var result = await connection.QueryAsync<double>(_query, new { UserId = userId });

                    if (result != null && result.Any())
                    {
                        var _balance = result.First();
                        obj.Balance = _balance;
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
    }
}
