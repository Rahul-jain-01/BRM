using System.Security.Claims;
using ChainMarketing.Application.Services;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Interfaces;
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
        private readonly IUserRepository _userRepository;

        public ReferralPathController(ReferralPathService referralPathService,IUserRepository userRepository)
        {
            _referralPathService = referralPathService;
            _userRepository = userRepository;
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
        //[Authorize(Roles ="Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllReferralPaths(int id)
        {
            var paths = await _referralPathService.GetReferralTreeFromPathTableAsync(id);
            // Debug log
            Console.WriteLine("Tree fetched for user: " + id);
            Console.WriteLine("Level 1 Count: " + paths.Level1.Count);
            Console.WriteLine("Level 2 Count: " + paths.Level2.Count);
            Console.WriteLine("Level 3 Count: " + paths.Level3.Count);
            return Ok(paths);
        }

        [HttpGet("count/{id}")]

        public async Task<int> CountOfLevel1([FromRoute]int id)
        {
            return await _referralPathService.GetActualLevel1ReferralCountAsync(id);
        }

        [HttpPost("assign-coapplicant")]
        public async Task<IActionResult> AssignCoApplicant([FromBody] CoApplicantRequest request)
        {
            await _userRepository.AssignCoApplicantAsync(request.UserId, request.SelectedReferralId);
            return Ok();
        }

        public class CoApplicantRequest
        {
            public int UserId { get; set; }
            public int? SelectedReferralId { get; set; }
        }

    }
}
