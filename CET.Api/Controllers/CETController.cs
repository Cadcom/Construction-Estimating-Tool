using CET.Business.Abstract;
using CET.Shared.Entities;
using CET.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CET.Api.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class CETController : Controller
    {
        ICETService service;
        public CETController(ICETService service)
        {
            this.service = service;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateUser appUser)
        {
            var user = service.GetAccountAsync(appUser.Username, appUser.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// Insert or Update Account Method. If account.ID is 0 than this is an add property else this is an update
        /// property. So finds the account from database and update it.
        /// 
        /// account.ID=0 için ==>
        /// if BaseID ==0 ==>this is Base Account, BaseID >0 ==>this is Sub Account
        ///
        /// account.ID>0 için ==>
        /// if BaseID ==ID ==>this is Base Account, BaseID !=ID ==>this is Sub Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>

        [Authorize(Roles = Role.Write)]
        [HttpPost]
        [Route("InsertOrUpdateAccount")]
        public async Task<IActionResult> InsertOrUpdateAccountAsync(Account account)
        {
            if (ModelState.IsValid)
            {                
                account = await service.insertOrUpdateAccountAsync(account);
                return Json(account);
            }
            else return BadRequest(ModelState);
        }


        /// <summary>
        /// Insert or Update Estimate Method. If estimate.ID is 0 than this is an add property else this is an update
        /// property. So finds the estimate from database and update it.
        /// </summary>
        /// <param name="estimate"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Write)]
        [HttpPost]
        [Route("InsertOrUpdateEstimate")]
        public async Task<IActionResult> InsertOrUpdateEstimateAsync(Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                estimate = await service.insertOrUpdateEstimateAsync(estimate);
                return Json(estimate);
            }
            else return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes the account by given Account.BaseID !Important
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAccount")]
        [Authorize(Roles = Role.Delete)]
        public async Task DeleteAccountAsync(long ID)
        {
            await service.DeleteAccountAsync(ID);
        }

        /// <summary>
        /// Deletes the Estimate by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteEstimate")]
        [Authorize(Roles = Role.Delete)]
        public async Task DeleteEstimateAsync(long id)
        {
            await service.DeleteEstimateAsync(id);
        }

        /// <summary>
        /// Get Estimate which given Account.BaseID !Important
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEstimateByID")]
        [Authorize(Roles = Role.Read)]
        [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]

        public async Task<IActionResult> GetEstimateByIDAsync(long id)
        {
            var result = await service.GetEstimateAsync(id);
            return Json(result);
        }


        /// <summary>
        /// Get Estimate list which given Account.BaseID !Important
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEstimateByID")]
        [Authorize(Roles = Role.Read)]
        [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id", "page", "size" })]

        public IActionResult GetUserEstimatesByIDAsync(long id, int page = 1, int size = 20)
        {
            var result = service.GetUserEstimates(id, page, size);
            return Json(result);
        }

    }
}
