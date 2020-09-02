using GitCommunity.Shared.Exceptions;
using GitCommunity.Shared.Models;
using GitCommunity.Shared.Utility;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GitCommunity.Shared.Service
{
    public class Accounts
    {

        private const int cachedExpiration = 120;
        public const string baseUrl = @"https://api.github.com";
        private readonly ServiceCaller _serviceCaller;
        private readonly IMemoryCache _cache;
        public Accounts(IMemoryCache memoryCache)
        {
            _serviceCaller = new ServiceCaller();
            _cache = memoryCache;
        }


        private async Task<User> GetUser(string userName)
        {
            var raw = await _serviceCaller.Invoke(baseUrl + "/users/" + userName, httpMethod: HttpMethod.Get);
            return JsonConvert.DeserializeObject<User>(raw);
        }


        private async Task<User> GetSertToCached(string name)
        {
            if (!_cache.TryGetValue(name, out User userOut))
            {
                if (userOut == null)
                {
                    userOut = await GetUser(name);
                    userOut.Login = name;
                    userOut.Source = "Github";
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                           .SetSize(1)
                                           .SetSlidingExpiration(TimeSpan.FromSeconds(cachedExpiration));

                _cache.Set(name, userOut, cacheEntryOptions);

            }
            else {
                userOut.Source = "Cached";
            }

            return userOut;
        }

        public async Task<List<User>> GetUserInformation(string[] usernames)
        {
            List<User> users = new List<User>();

            foreach (var item in usernames.GroupBy(g => g).ToList())
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    //put some try catch here
                    User user = await GetSertToCached(item.Key);

                    user.GenerateNumberOfFollowerPerRepositories();
                    users.Add(user);
                }
            }

            return users.OrderBy(o => o.Name).ToList();
        }
    }
}
