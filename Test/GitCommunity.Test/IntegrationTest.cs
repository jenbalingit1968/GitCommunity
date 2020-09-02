using GitCommunity.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GitCommunity.Test
{
    public class IntegrationTest
    {
        public readonly TestServer testServer;
        public IntegrationTest()
        {
            testServer = new TestServer(new WebHostBuilder()
                    .UseStartup<Startup>());
        }

        [Test]
        public async Task TestValidReponse()
        {
            string[] usernames = new string[] { "mojomboa", "mojombo", "defunkt", "pjhyett" };
            using HttpClient httpClient = testServer.CreateClient();


            //the logic will search to the following username to memory cahed. if the usernames does not exist then it will call the github api. 
            var response = await httpClient.PostAsync("/user", new StringContent(JsonConvert.SerializeObject(usernames), Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            List<Shared.Models.User> result = JsonConvert.DeserializeObject<List<Shared.Models.User>>(responseString);

            Assert.AreNotEqual(0, result.Count);

        }


        [Test]
        public async Task TestIfAllDataAreCached()
        {
            string[] usernames = new string[] { "mojomboa", "mojombo", "defunkt", "pjhyett" };
            using HttpClient httpClient = testServer.CreateClient();

            //first call of service, data must be save to cached;
            await httpClient.PostAsync("/user", new StringContent(JsonConvert.SerializeObject(usernames), Encoding.Default, "application/json"));

            //second call logic will find the data in memorycahed.
            var response = await httpClient.PostAsync("/user", new StringContent(JsonConvert.SerializeObject(usernames), Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();


           //read the reponse of the executed api and convert the value into string format.
            var responseString = await response.Content.ReadAsStringAsync();


            List<Shared.Models.User> result = JsonConvert.DeserializeObject<List<Shared.Models.User>>(responseString);

            //make sure that all data are from cached;
            Assert.AreEqual(0, result.Count(w => w.Source == "Github"));

        }
    }
}
