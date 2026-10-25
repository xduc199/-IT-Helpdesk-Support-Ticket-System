using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace API.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
    {
        var path = context.Request.Path.ToString();

        // ❌ CHẶN LOG NHỮNG ENDPOINT KHÔNG CẦN THIẾT
        if (path.StartsWith("/swagger") ||
            path.StartsWith("/openapi") ||
            path.Contains("/AuditLogs"))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = Guid.TryParse(userIdStr, out var uid) ? uid : null,
                Action = $"{context.Request.Method} {context.Response.StatusCode}",
                TicketId = null,
                CreatedAt = DateTime.UtcNow,
                Detail = $"{path} | {stopwatch.ElapsedMilliseconds}ms"
            };

            try
            {
                db.AuditLogs.Add(log);
                await db.SaveChangesAsync();
            }
            catch
            {
                // ❌ KHÔNG ĐƯỢC làm crash API vì audit fail
            }
        }
    }
}