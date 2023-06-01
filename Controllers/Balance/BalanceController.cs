using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskNowSoftware.Core.Filters;
using TaskNowSoftware.IRepository;
using TaskNowSoftware.Models.Response;

namespace TaskNowSoftware.Controllers.Balance
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IMapper _mapper;
        private readonly IBalanceRepository _balanceRepository;

        public BalanceController(ILogger<BalanceController> logger, IMapper mapper, IBalanceRepository balanceRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _balanceRepository = balanceRepository;
        }

        /// <summary>
        /// Get user balance
        /// </summary>
        /// <returns>Balance against authorized user</returns> 
        [Authorize]
        [HttpGet("GetBalance")]
        public async Task<ActionResult> GetBalance()
        {
            try
            {
                int _userId = 0;
                var items = HttpContext.Items["User"];
                if (items != null)
                {
                    _userId = Convert.ToInt32(items);
                }
                if (_userId > 0)
                {
                    var status = HttpContext.User;
                    GetBalanceResponseModel _balanceRes = await _balanceRepository.GetBalance(_userId);
                    var result = _mapper.Map<dynamic>(_balanceRes);
                    return Ok(result);
                }
                else
                {
                    return StatusCode(400, "User Id not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Something went wrong {nameof(GetBalance)}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
