using System.Security.Claims;
using ChainMarketing.Application.Services;
using ChainMarketing.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChainMarketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralPathController : ControllerBase
    {
        private readonly ReferralPathService _referralPathService;

        public ReferralPathController(ReferralPathService referralPathService)
        {
            _referralPathService = referralPathService;
        }

        // Endpoint to add a referral path
        [HttpPost("add")]
        public async Task<IActionResult> AddReferralPathAsync([FromBody] ReferralPath referralPath)
        {
            try
            {
                await _referralPathService.AddReferralPathAsync(referralPath.UserId, referralPath.ReferrerId);
                return Ok("Referral path added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint to get all referral paths for a referrer
        [HttpGet("referrals/{referrerId}")]
        public async Task<ActionResult<List<ReferralPath>>> GetReferralPathsForReferrerAsync(int referrerId)
        {
            try
            {
                var referralPaths = await _referralPathService.GetReferralPathsForReferrerAsync(referrerId);
                return Ok(referralPaths);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint to get a single referral path for a user
        //[HttpGet("{userId}")]
        //public async Task<ActionResult<ReferralPath>> GetReferralPathForUserAsync(int userId)
        //{
        //    try
        //    {
        //        var referralPath = await _referralPathService.GetReferralPathsForReferrerAsync(userId);
        //        return Ok(referralPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        // GET: api/ReferralPath/all
        [Authorize(Roles ="Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllReferralPaths(int id)
        {
            var paths = await _referralPathService.GetReferralTreeFromPathTableAsync(id);
            return Ok(paths);
        }
    }
}
