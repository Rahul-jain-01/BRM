using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Domain.Entities;

namespace ChainMarketing.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByReferralCodeAsync(string referralCode);
        Task AddAsync(User user);
        Task<List<User>> GetDirectReferralsAsync(int userId);
        Task UpdateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllUserAync();
        Task AssignCoApplicantAsync(int userId, int? selectedReferralId = null);


    }
}
