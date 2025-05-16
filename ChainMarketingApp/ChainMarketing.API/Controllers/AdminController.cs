using ChainMarketing.Application.DTOs;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChainMarketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserRepository _userRepository;


        public AdminController(IAdminService adminService,IUserRepository userRepository)
        {
            _adminService = adminService;
            _userRepository = userRepository;
        }

        [HttpGet("user-referral-code/{email}")]

        public async Task<IActionResult> GetUserReferralCode([FromRoute] string email) { 
            var result=await _adminService.GetUserReferralCodeByEmailAsync(email);

            return Ok(result);
        }
        [HttpGet("referral-tree/{email}")]
        public async Task<IActionResult> GetReferralTree([FromRoute] string email)
        {
            var result = await _adminService.GetReferralTreeByEmailAsync(email);
            return Ok(result);
        }
        [HttpPut("mark-paid/{email}")]
        public async Task<IActionResult> MarkUserAsPaid([FromRoute]string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var userInfo = await _userRepository.GetByIdAsync(user.Id);
            if (user == null) return NotFound();

            user.IsPaid = true;
            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetUserDetails();

            //domian to DTO
            var userDTO = new List<UserDto>();

            foreach (var item in users)
            {
                userDTO.Add(new UserDto()
                {
                    Id= item.Id,
                    Email=item.Email,
                    ReferralCode=item.ReferralCode,
                    ReferredById=item.ReferredById,
                    HasCoApplicant=item.HasCoApplicant,
                    IsPaid=item.IsPaid,
                    CreatedAt=item.CreatedAt,
                });
            }
            return Ok(userDTO);

        }


    }
}
