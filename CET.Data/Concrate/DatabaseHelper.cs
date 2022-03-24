using CET.Data.Abstract;
using CET.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Data.Concrate
{
    public class DatabaseHelper : IDatabaseHelper
    {
        CETDBContext db;
        public DatabaseHelper(CETDBContext db)
        {
            this.db = db;
        }

        public async Task DeleteAccountAsync(long ID)
        {
            var deleteAccount = await GetAccountAsync(ID);

            if (deleteAccount != null)
                db.Remove(deleteAccount);

            db.SaveChanges();
        }

        public async Task DeleteEstimateAsync(long ID)
        {
            var deleteEstimate = await GetEstimateAsync(ID);

            if (deleteEstimate != null)
                db.Remove(deleteEstimate);

            db.SaveChanges();
        }

        public async Task<Account> GetAccountAsync(long ID)
        {
            var account = await db.FindAsync<Account>(ID);

            return account;
        }

        public Account GetAccount(string userName, string passWord)
        {
            var account = db.Accounts.AsQueryable().FirstOrDefault(x => x.LoginEmail.Equals(userName) && x.PasswordHash.Equals(passWord));
            return account;
        }

        

        public List<Account> GetAllAccountsWitPaging(int page = 1, int size = 20)
        {
            //if size=0 ; means no Paging
            if (size == 0)
                return db.Accounts.AsQueryable().ToList();
            else
                return db.Accounts.AsQueryable().Skip((page - 1) * size).Take(size).ToList();
        }

        public async Task<Estimate> GetEstimateAsync(long ID)
        {
            var estimate = await db.FindAsync<Estimate>(ID);

            return estimate;
        }

        public List<Estimate> GetUserEstimates(long UserID, int page = 1, int size = 20)
        {
            //if size=0 ; means no Paging
            if (size == 0)
                return db.Estimates.AsQueryable().Where(x=>x.AccountId==UserID).ToList();
            else
                return db.Estimates.AsQueryable().Where(x => x.AccountId == UserID).Skip((page - 1) * size).Take(size).ToList();
        }

        /// <summary>
        /// ID=0 için ==>
        /// if BaseID =0 ==>Base Account, BaseID >0 ==>Sub Account
        ///
        /// ID>0 için ==>
	    /// if BaseID ==ID ==>Base Account, BaseID !=ID ==>Sub Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> insertOrUpdateAccountAsync(Account account)
        {
            if (account.ID == 0) //insert
            {
                bool isBaseAccount = (account.BaseID == 0);

                await db.Accounts.AddAsync(account);
                if (isBaseAccount) // this means this is base account otherwise means sub account
                {
                    account.BaseID = account.ID;
                    db.Accounts.Update(account);
                }

            }
            else //Update
            {
                db.Accounts.Update(account);
            }
            await db.SaveChangesAsync();

            return account;
        }

        public async Task<Estimate> insertOrUpdateEstimateAsync(Estimate estimate)
        {
            if (estimate.ID == 0) //insert
            {
                await db.Estimates.AddAsync(estimate);

            }
            else //Update
            {
                db.Estimates.Update(estimate);
            }
            await db.SaveChangesAsync();

            return estimate;
        }
    }
}
