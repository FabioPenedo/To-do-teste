using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using To_do_teste.src.Context;
using To_do_teste.src.DTOs;
using To_do_teste.src.Exceptions;

namespace To_do_teste.src.Middlewares
{
    public static class GlobalExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, Microsoft.Extensions.Logging.ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        var statusCode = exception switch
                        {
                            NotFoundException => HttpStatusCode.NotFound,
                            BadRequestException => HttpStatusCode.BadRequest,
                            UnauthorizedAccessException => HttpStatusCode.Forbidden,
                            ArgumentException => HttpStatusCode.BadRequest,
                            _ => HttpStatusCode.InternalServerError
                        };

                        context.Response.StatusCode = (int)statusCode;

                        // Garantir que contexto está disponível para o log
                        var userId = ToDoContext.UserId?.ToString() ?? "unknown";
                        var taskId = ToDoContext.TaskId?.ToString() ?? "anonymous";

                        // Log estruturado com contexto completo
                        Log.Error(exception,
                            "exception.handled Exception captured while processing request HttpMethod={Method} Path={Path} StatusCode={StatusCode} ExceptionType={ExceptionType} UserId={UserId} TaskId={TaskId}",
                            context.Request.Method, context.Request.Path, (int)statusCode, exception.GetType().Name, userId, taskId);

                        // Retorna JSON de erro
                        await context.Response.WriteAsync(new ErrorDetails
                        (
                            context.Response.StatusCode,
                            exception.Message
                        ).ToString());
                    }
                });
            });
        }
    }
}
