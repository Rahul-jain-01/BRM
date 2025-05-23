using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Application.DTOs
{
    public class ReferralTreeDto
    {
        public int UserId { get; set; }
        public int? coApplicantId{get;set;}
        public List<ReferralUserDto> Level1 { get; set; } = new();
        public List<ReferralUserDto> Level2 { get; set; } = new();
        public List<ReferralUserDto> Level3 { get; set; } = new();
    }
}
