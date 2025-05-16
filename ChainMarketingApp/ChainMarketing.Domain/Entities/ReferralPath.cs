using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Domain.Entities
{
    public class ReferralPath
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReferrerId { get; set; }

        public int Level { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public User Referrer { get; set; }

    }
}
