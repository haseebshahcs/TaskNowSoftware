using Microsoft.AspNetCore.Mvc;
using TaskNowSoftware.Models.Response;

namespace TaskNowSoftware.IRepository
{
    public interface IBalanceRepository
    {
        Task<GetBalanceResponseModel> GetBalance(int userId);
    }
}
