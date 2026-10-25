using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new { StatusCode = context.Response.StatusCode, Message = "Internal Server Error" };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}