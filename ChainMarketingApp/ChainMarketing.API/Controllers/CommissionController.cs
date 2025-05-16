using ChainMarketing.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChainMarketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;

        public CommissionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCommission(int userId)
        {
            var total = await _commissionService.CalculateCommissionAsync(userId);
            return Ok(new { UserId = userId, TotalCommission = total });
        }
    }
}
