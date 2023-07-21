using LogicaNegocioServicio.Comunes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Middleware
{

    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate _next;

        public LoguearRespuestaHTTPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Capturar la solicitud
            var request = await FormatRequest(context.Request);
            string response;
            var servicio = await FormatServicio(context.Request);
            // Almacenar el cuerpo original de la respuesta
            var originalBodyStream = context.Response.Body;

            try
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // Llamar al siguiente middleware en el pipeline
                await _next(context);

                // Capturar la respuesta
                response = await FormatResponse(context.Response);

                // Hacer algo con la solicitud y la respuesta capturadas, como escribirlas en un archivo de registro
                // ...
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente, por ejemplo, escribirla en un archivo de registro
                LogModel log1 = new LogModel();
                log1.Respuesta = $"Error: {ex.Message}";
                log1.Peticion = request;
                Logger.RegistrarLog(log1);

                // Lanzar la excepción para que sea manejada por el siguiente middleware en el pipeline
                throw;
            }
            finally
            {
                // Restaurar el cuerpo original de la respuesta
                context.Response.Body = originalBodyStream;
            }

            // Loguear la respuesta
            LogModel log = new LogModel();
            log.Respuesta = response;
            log.Servicio= servicio;
            log.Peticion = request;
            Logger.RegistrarLog(log);

            // Escribir la respuesta en el cuerpo original
            await WriteResponseAsync(response, originalBodyStream);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            // Habilitar la memoria en búfer para la solicitud HTTP
            request.EnableBuffering();

            // Leer la solicitud y formatearla como una cadena
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            // Construir la URL completa manualmente
             var queryString = request.QueryString;

            var formattedRequest = $"{queryString} {requestBody}";
            return formattedRequest;
        }

        private async Task<string> FormatServicio(HttpRequest request)
        {
            // Habilitar la memoria en búfer para la solicitud HTTP
            request.EnableBuffering();

            // Leer la solicitud y formatearla como una cadena
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            // Construir la URL completa manualmente
            var scheme = request.Scheme;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            var path = request.Path.ToUriComponent();
           

            var formattedRequest = $"{request.Method} {scheme}://{host}{pathBase}{path}";
            return formattedRequest;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            // Leer la respuesta y formatearla como una cadena
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyString = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            // Agregar el Content-Length a la cadena formateada
            var contentLength = response.ContentLength;
            var formattedResponse = $"{responseBodyString}";

            if (contentLength.HasValue)
            {
                formattedResponse += $" (Content-Length: {contentLength})";
            }

            return formattedResponse;
        }

        private async Task WriteResponseAsync(string response, Stream stream)
        {
            var buffer = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }

}

