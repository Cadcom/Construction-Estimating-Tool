using CET.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Business.Abstract
{
    public interface ICETService
    {
        public Task<Account> insertOrUpdateAccountAsync(Account account);
        public Task DeleteAccountAsync(long ID);
        public Task<Account> GetAccountAsync(long ID);
        public Account GetAccountAsync(string userName, string passWord);
        public List<Account> GetAllAccountsWitPaging(int page = 1, int size = 20);

        public Task<Estimate> insertOrUpdateEstimateAsync(Estimate estimate);
        public Task DeleteEstimateAsync(long ID);
        public Task<Estimate> GetEstimateAsync(long ID);
        public List<Estimate> GetUserEstimates(long UserID, int page = 1, int size = 20);
    }
}
