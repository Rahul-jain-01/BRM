using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Application.Interfaces
{
    public interface ICommissionService
    {
        Task<decimal> CalculateCommissionAsync(int userId);
    }
}
