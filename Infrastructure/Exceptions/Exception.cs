using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Exceptions
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        IHostingEnvironment _env;
        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(RouletteException))
            {
                var json = new JsonErrorResponse
                {
                    Messages = ((RouletteException)context.Exception).Messages
                };

                if (_env.IsEnvironment("Local")
                || _env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }

                context.Result = new ObjectResult(json);
                context.HttpContext.Response.StatusCode = ((RouletteException)context.Exception).HttpStatusCode;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "An error occur.Try it again." }
                };

                if (_env.IsEnvironment("Local")
                || _env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }
                context.Result = new ObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }
        public class JsonErrorResponse
        {
            public string[] Messages { get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
