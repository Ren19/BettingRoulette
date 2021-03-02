using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Exceptions
{
    [Serializable]
    public class RouletteException : Exception
    {
        public int HttpStatusCode { get; set; }
        public string[] Messages { get; set; }
        public ServiceResponse Result { get; set; }
        public RouletteException(string[] messages) : base(String.Join('-', messages))
        {
            HttpStatusCode = 400;
            Messages = messages;
        }
        public RouletteException(int statusCode, string[] messages) : base(String.Join('-', messages))
        {
            HttpStatusCode = statusCode;
            Messages = messages;
        }
        public RouletteException(int statusCode, ServiceResponse result) : base(result.Message)
        {
            HttpStatusCode = statusCode;
            Result = result;
        }
        public class ServiceResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
