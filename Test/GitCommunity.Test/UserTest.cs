using Newtonsoft.Json.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using GitCommunity.Shared.Models;
using System.Linq;

namespace GitCommunity.Test
{
    public class UserTest
    {
        private readonly Shared.Service.Accounts account;
        public UserTest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            account = new Shared.Service.Accounts(cache);
        }

        [Test]
        public async Task MatchRecordCountToParameterCount()
        {
            
            string[] usernames = new string[] { "mojombo", "defunkt", "pjhyett" };

            var raw = await account.GetUserInformation(usernames);
            Assert.AreEqual(raw.Count(), usernames.Count());

        }

        [Test]
        public async Task TestForDuplicateUserName()
        {
        
            string[] usernames = new string[] { "mojombo", "mojombo", "defunkt", "pjhyett" };

            var raw = await account.GetUserInformation(usernames);
            Assert.AreEqual(3, raw.Count());

        }

        [Test]
        public async Task TestForNotFoundItem()
        {

            string[] usernames = new string[] { "mojomboa", "mojombo", "defunkt", "pjhyett" };


            var raw = await account.GetUserInformation(usernames);

            var findNotFound = raw.FirstOrDefault(f => f.Message == "Not Found");
            Assert.IsNotNull(findNotFound);

        }

        [Test]
        public async Task TestCahedItem()
        {

            string[] usernames = new string[] { "mojombo" };

            await account.GetUserInformation(usernames);

            var raw = await account.GetUserInformation(usernames);

            var findNotFound = raw.FirstOrDefault(f => f.Source == "Cached");
            Assert.IsNotNull(findNotFound);

        }

        [Test]
        public async Task TestGitHubGet()
        {

            string[] usernames = new string[] { "mojombo" };

            
            var raw = await account.GetUserInformation(usernames);

            var findNotFound = raw.FirstOrDefault(f => f.Source == "Github");
            Assert.IsNotNull(findNotFound);

        }
    }
}
