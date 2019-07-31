using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App04;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
    public class Startup
    {
        public object Response { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<SimpleMiddleware>();
            app.UseFileServer();
            _ = app.UseMvc(routes =>
              {
                  routes.MapRoute(
                      name: "default",
                      template: "{controller=Home}/{action=Index}");
              });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("<html><body>");
                await context.Response.WriteAsync("<button type=\"button\">button</button>");
                await next();
                await context.Response.WriteAsync("</body></html>");
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Bradley Snyder");
            });
        }
    }
}
