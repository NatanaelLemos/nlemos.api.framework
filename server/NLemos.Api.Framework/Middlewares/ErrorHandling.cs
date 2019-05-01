using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLemos.Api.Framework.Exceptions;

namespace NLemos.Api.Framework.Middlewares
{
    internal class ErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = Error(exception);

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)error.status;

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Max-Age", "86400");

            await context.Response.WriteAsync(error.message);
        }

        private (HttpStatusCode status, string message) Error(Exception ex)
        {
            if (_logger != null)
            {
                _logger.LogError(FullErrorMessage(ex));
            }

            switch (ex)
            {
                case InvalidModelStateException paramsEx:
                    return (HttpStatusCode.UnprocessableEntity, paramsEx.InnerException.Message);

                case InvalidParametersException paramsEx:
                    return (HttpStatusCode.UnprocessableEntity, paramsEx.InnerException.Message);

                case KeyNotFoundException keyException:
                    return (HttpStatusCode.NotFound, JsonMessage(keyException.Message));

                case ArgumentException argEx:
                    return (HttpStatusCode.InternalServerError, JsonMessage(argEx.Message));

                default:
                    return (HttpStatusCode.InternalServerError, JsonMessage("Internal server error"));
            }
        }

        private string FullErrorMessage(Exception e)
        {
            var msg = "";

            while (e != null)
            {
                msg += e.Message + "\r\n";
                e = e.InnerException;
            }

            return msg;
        }

        private string JsonMessage(string message)
        {
            return "{\"error\": \"" + message + "\"}";
        }
    }
}
