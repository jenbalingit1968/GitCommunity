using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GitCommunity.Api.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) => _next = next;


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try { await _next(httpContext).ConfigureAwait(false); }
            catch (Shared.Exceptions.InternalServerExeception exception) { await HandleExceptionAsync(httpContext, exception.Message, HttpStatusCode.InternalServerError).ConfigureAwait(false); }
            catch (Shared.Exceptions.BadRequestException  exception) { await HandleExceptionAsync(httpContext, exception.Message, HttpStatusCode.Conflict).ConfigureAwait(false); }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, string exceptionMessage, HttpStatusCode httpStatusCode)
        {
            var response = httpContext.Response;
            response.Clear();
            response.StatusCode = (int)httpStatusCode;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage = exceptionMessage })).ConfigureAwait(false);
        }
    }

    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
