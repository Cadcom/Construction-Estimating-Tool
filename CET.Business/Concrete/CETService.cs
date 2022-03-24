using CET.Business.Abstract;
using CET.Data.Abstract;
using CET.Shared.Entities;
using CET.Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CET.Business.Concrete
{
    public class CETService : ICETService
    {
        IDatabaseHelper databaseHelper;
        public CETService(IDatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task DeleteAccountAsync(long ID)
        {
            await databaseHelper.DeleteAccountAsync(ID);
        }

        public async Task DeleteEstimateAsync(long ID)
        {
            await databaseHelper.DeleteEstimateAsync(ID);
        }

        public Task<Account> GetAccountAsync(long ID)
        {
            return databaseHelper.GetAccountAsync(ID);
        }

        public Account GetAccountAsync(string userName, string passWord)
        {
            Account account = databaseHelper.GetAccount(userName, passWord);

            if (account == null)
                return null;

            // authentication successful so generate jwt token with Authorize
            account.PasswordHash = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("mySecretKeymySecretKeymySecretKeymySecretKey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.BaseID.ToString()), //This is important for Sub Accounts
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            byte roles = account.Roles;

            if (roles >= Roles.Read) {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, Role.Read));
                roles -= Roles.Read;

            }
            if (roles >= Roles.Write)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, Role.Write));
                roles -= Roles.Write;

            }
            if (roles >= Roles.Delete)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, Role.Delete));
            

            var token = tokenHandler.CreateToken(tokenDescriptor);
            account.Token = tokenHandler.WriteToken(token);

            return account;
        }

        public List<Account> GetAllAccountsWitPaging(int page = 1, int size = 20)
        {
            return databaseHelper.GetAllAccountsWitPaging(page, size);
        }

        public async Task<Estimate> GetEstimateAsync(long ID)
        {
            return await databaseHelper.GetEstimateAsync(ID);
        }

        public List<Estimate> GetUserEstimates(long UserID, int page = 1, int size = 20)
        {
            return databaseHelper.GetUserEstimates(UserID,page, size);
        }

        public async Task<Account> insertOrUpdateAccountAsync(Account account)
        {
            return await databaseHelper.insertOrUpdateAccountAsync(account);
        }

        public async Task<Estimate> insertOrUpdateEstimateAsync(Estimate estimate)
        {
            return await databaseHelper.insertOrUpdateEstimateAsync(estimate);
        }
    }
}
