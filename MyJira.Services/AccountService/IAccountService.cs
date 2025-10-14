using MyJira.Infastructure.Helper;
using MyJira.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Services.AccountService
{
    public interface IAccountService
    {
        Task<OperationResult<string>> Register(RegisterViewModel viewModel);
        Task<OperationResult<string>> Login(LoginViewModel viewModel);
    }
}
