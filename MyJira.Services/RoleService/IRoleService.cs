using MyJira.Infastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.RoleService
{
    public interface IRoleService
    {
        Task<OperationResult<string>> CreateRole(string roleName);
        Task<OperationResult<string>> SetRoleToUser(int memberId, string  roleName);
        Task<OperationResult<bool>> IfUserIsInRole(int memberId, string roleName);
    }
}
