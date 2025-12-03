using BlogPersonal.Domain.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogPersonal.API.Middlewares
{
    /// <summary>
    /// Middleware global para capturar y manejar excepciones no controladas.
    /// Convierte excepciones de dominio a respuestas HTTP apropiadas.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        /// <summary>
        /// Maneja excepciones y genera respuestas HTTP apropiadas según el tipo de excepción.
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = exception.Message,
                timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case InvalidCredentialsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogWarning($"Intento de inicio de sesión con credenciales inválidas");
                    break;

                case UserAlreadyExistsException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning($"Intento de registro con email duplicado: {exception.Message}");
                    break;

                case DuplicateUsernameException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning($"Intento de registro con nombre de usuario duplicado: {exception.Message}");
                    break;

                case DefaultRoleNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError($"Error crítico: rol por defecto no encontrado");
                    response = new
                    {
                        message = "Error interno del servidor. Por favor, contacte al administrador.",
                        timestamp = DateTime.UtcNow
                    };
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogInformation($"Recurso no encontrado: {exception.Message}");
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _logger.LogWarning($"Acceso no autorizado: {exception.Message}");
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning($"Argumento inválido: {exception.Message}");
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError($"Excepción no controlada: {exception.GetType().Name} - {exception.Message}");
                    response = new
                    {
                        message = "Error interno del servidor",
                        timestamp = DateTime.UtcNow
                    };
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
