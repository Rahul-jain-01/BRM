using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Application.DTOs
{
    public class ReferralUserDto
    {
        public int Id { get; set; }
        public bool IsCoApplicant { get; set; }
        public bool HasCoApplicant { get; set; }
    }
}
