using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace NextOptimization.Business.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate NextRequestDelegate;

        public ErrorHandlingMiddleware(RequestDelegate requestDelegate)
        {
            NextRequestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await ProceedWithServiceCall(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse($"{ex.GetAllMessages()}")));
                return;
            }
        }

        private async Task ProceedWithServiceCall(HttpContext context)
        {
            context.Request.EnableBuffering();
            string bodyString = await ReadBodyString(context);
            string path = ReadPath(context);

            try
            {
                await NextRequestDelegate(context);
            }
            catch (ApiException ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)ex.StatusCode;
                await response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse($"{ex.GetAllMessages()}")));
                return;
            }
        }

        private static string ReadPath(HttpContext context)
        {
            return context.Request.Method + " " + context.Request.Path + context.Request.QueryString;
        }

        private static async Task<string> ReadBodyString(HttpContext context)
        {
            string bodyString = string.Empty;

            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                bodyString = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            return bodyString;
        }
    }
}