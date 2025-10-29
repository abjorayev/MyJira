using Microsoft.AspNetCore.Identity;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Helper;
using MyJira.Repository.MemberRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemberRepository _memberRepository;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            IMemberRepository memberRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _memberRepository = memberRepository;
        }

        public async Task<OperationResult<string>> CreateRole(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return OperationResult<string>.Fail("Role already exists");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
                return OperationResult<string>.Fail("Failed to create role");

            return OperationResult<string>.Ok("Role created successfully");
        }

        public async Task<OperationResult<bool>> IfUserIsInRole(int memberId, string roleName)
        {
            var user =  _userManager.Users.FirstOrDefault(x => x.MemberId == memberId);
            if (user == null)
                return OperationResult<bool>.Fail("User not found");
            var hasRole = await _userManager.IsInRoleAsync(user, roleName);
            return OperationResult<bool>.Ok(false);
        }

        public async Task<OperationResult<string>> SetRoleToUser(int memberId, string roleName)
        {
            
            var user =  _userManager.Users.FirstOrDefault(x => x.MemberId == memberId);
            if (user == null)
                return OperationResult<string>.Fail("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return OperationResult<string>.Fail("Role does not exist");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return OperationResult<string>.Fail("Failed to assign role");

            return OperationResult<string>.Ok("Role assigned successfully");
        }
    }
}
