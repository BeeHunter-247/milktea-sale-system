﻿using PRN222.Milktea.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountModel> Login(string email, string password);
        Task<IEnumerable<AccountModelAdmin>> GetAllAccountsExceptAdminAsync(int currentAdminId);
        Task BanAccountAsync(int accountId);
        Task<int> GetTotalRegisteredAccountsAsync();
        Task<int> GetTotalBannedAccountsAsync();

        Task UnlockAccountAsync(int accountId);
    }
}
