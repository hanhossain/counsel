using System;
using System.Collections.Generic;
using System.Net;

namespace Counsel.FantasyClient.Exceptions
{
    public class ClientException : Exception
    {
        public ClientException(HttpStatusCode statusCode, List<Dictionary<string, string>> errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public HttpStatusCode StatusCode { get; set; }
        public List<Dictionary<string,string>> Errors { get; set; }
    }
}
