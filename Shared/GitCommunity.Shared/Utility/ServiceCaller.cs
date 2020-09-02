using GitCommunity.Shared.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GitCommunity.Shared.Utility
{
    internal class ServiceCaller
    {
        public async Task<string> Invoke(string url, HttpMethod httpMethod, string parameter = "")
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage();


            var httpResponseMessage = new HttpResponseMessage();

            request.RequestUri = new Uri(url);
            request.Method = httpMethod;

            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

            if (request.Method != HttpMethod.Get)
                request.Content = new StringContent(parameter, Encoding.UTF8, "application/json");

            var task = await httpClient.SendAsync(request);
            if (task.StatusCode == HttpStatusCode.InternalServerError)
            {

                var isResponse = JsonConvert.DeserializeObject<Models.ServerResponse>(await task.Content.ReadAsStringAsync());

                throw new InternalServerExeception(isResponse.Message);
            }
            else if (task.StatusCode == HttpStatusCode.BadRequest)
            {
                var brResponse = JsonConvert.DeserializeObject<Models.ServerResponse>(await task.Content.ReadAsStringAsync());

                throw new BadRequestException(brResponse.Message);
            }
            else
            {

                return await task.Content.ReadAsStringAsync();
            }
        }

    }

}
