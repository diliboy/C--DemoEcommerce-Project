using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //declare default variables
            string message = "Sorry, Internal server error occured";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //check if response here is too many requests - 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    message = "Too many requests made.";
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is Unauthorized - 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    message = "You are not authorized to access.";
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is forbidden - 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    message = "You are not allowed/required to access.";
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //log original exceptions / File, Debugger, console
                LogException.LogExceptions(ex);

                //check if exception is timeout - 408 requst timeout
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout... try again!";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                //if Exception is caught
                //If none of above then do default
                await ModifyHeader(context, title, message, statusCode);
                
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            //display scary-free message to client
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails() { 
                Detail = message,
                Status = statusCode,
                Title = title
            }),CancellationToken.None);
            return;
        }
    } 
}

        
