using ChainMarketing.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChainMarketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralController : ControllerBase
    {
        private readonly IReferralTreeService _referralTreeService;

        public ReferralController(IReferralTreeService referralTreeService)
        {
            _referralTreeService = referralTreeService;
        }

        [HttpGet("tree/{userId}")]
        public async Task<IActionResult> GetReferralTree(int userId)
        {
            try
            {
                var referralTree = await _referralTreeService.GetReferralTreeAsync(userId);
                return Ok(referralTree);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
