using Microsoft.AspNetCore.Http;
using PersonalAiAssistant.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonalAiAssistant.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotSupportedException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (HttpRequestException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.ServiceUnavailable,
                ex.Message);
        }
        catch (Exception ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;

        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = message
        };

        var json =
            JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}