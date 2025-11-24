using MyJira.Repository.MemberRepository;
using MyJira.Services.AccountService;
using MyJira.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.Helper
{
    public static class UserProfileHelper
    {
        
        public static UserProfile GetUserProfile(ClaimsPrincipal User)
        {
            var userProfile = new UserProfile();

            var memberId = User.FindFirstValue("MemberId");
            var memberName = User.FindFirstValue("MemberName");
            userProfile.Name = memberName;
            if (!string.IsNullOrWhiteSpace(memberId))
            {
                userProfile.MemberId = int.Parse(memberId);
            }

            return userProfile;
        }
    }
}
