using ExcepionHandling.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExcepionHandling
{
    public class GlobalExceptionHandlingMiddleware
    {

        private readonly RequestDelegate next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleInternalServerError(context, ex);
            }
        }

        public async Task HandleNotFoundException(HttpContext context, NotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(new ProblemDetails
            {
                Title = "Not Found",
                Detail = ex.Message,
                Status = context.Response.StatusCode
            }.ToString());
        }

        public async Task HandleInternalServerError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ProblemDetails 
            {
                Title = "Internal server error",
                Detail = ex.Message,
                Status = context.Response.StatusCode    
            }.ToString());
        }


    }
}
