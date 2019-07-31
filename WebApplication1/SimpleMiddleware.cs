using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace App04
{
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;
        public SimpleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            if (context.Request.QueryString.ToString().Contains("About"))
            {
                await ReturnIndexPage(context);
                return;
            }
            
            if (context.Request.QueryString.ToString().Contains("Resume"))
            {
                await ReturnPdfPage(context);
                return;
            }
            await _next.Invoke(context);
        }
        private static async Task ReturnPdfPage(HttpContext context)
        {
            var file = new FileInfo(@"wwwroot\Resume.pdf");
            byte[] buffer;
            if (file.Exists)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/pdf";
                buffer = File.ReadAllBytes(file.FullName);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/pdf";
                buffer = Encoding.UTF8.GetBytes("Unable to find the requested file");
            }

            using (var stream = context.Response.Body)
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }

            context.Response.ContentLength = buffer.Length;
        }
        private static async Task ReturnIndexPage(HttpContext context)
        {
            var file = new FileInfo(@"wwwroot\About.html");
            byte[] buffer;
            if (file.Exists)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/html";
                buffer = File.ReadAllBytes(file.FullName);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "text/plain";
                buffer = Encoding.UTF8.GetBytes("Unable to find the requested file");
            }

            using (var stream = context.Response.Body)
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }

            context.Response.ContentLength = buffer.Length;
        }
    }
}