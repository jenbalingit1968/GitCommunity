using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GitCommunity.Shared.Models
{
    internal class ServerResponse
    {


        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
