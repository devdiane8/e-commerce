using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Errors;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace API.Midlleware;

public class ExecptionMiddleware(IHostEnvironment environment, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExeptionAsync(context, ex, environment);
        }
    }

    private static Task HandleExeptionAsync(HttpContext context, Exception ex, IHostEnvironment environment)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = environment.IsDevelopment()
        ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
        : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal server error");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(response, options);
        return context.Response.WriteAsync(json);
    }
}
