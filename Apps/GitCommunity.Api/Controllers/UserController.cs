using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SharedData = GitCommunity.Shared;
namespace GitCommunity.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SharedData.Service.Accounts accounts;

        public UserController(IMemoryCache cache)
        {
            accounts = new SharedData.Service.Accounts(cache);
        }

        [HttpGet]
        public string Get()
        {
            return "welcome to git community!!";
        }

        [HttpPost]
        public async Task<List<SharedData.Models.User>> RetrieveUsers(string [] username)
        {
            
            return await accounts.GetUserInformation(username);
        }
        
        
    }
}
