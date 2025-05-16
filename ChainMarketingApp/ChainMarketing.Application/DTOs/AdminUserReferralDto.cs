using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Application.DTOs
{
    public class AdminUserReferralDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string ReferralCode { get; set; }
    }
}
