using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRServer210107.Hubs;

namespace SignalRServer210107
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5500", "null")
                       .AllowAnyMethod()
                       .AllowAnyHeader()                       
                       .AllowCredentials();
            }));

            services.AddSignalR();
        }

        // HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

          

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });           

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");


            app.Use(async (context, next) =>
            {
                if (next != null)
                {
                    await next.Invoke();
                }
                var hubContext = context.RequestServices
                                        .GetRequiredService<IHubContext<GenericHub>>();
                string stamp = $"> {DateTime.Now.ToString("G")}: ";
                string method = $" [{context.Request.Method}]";
                string path = $"  {context.Request.Path}{context.Request.QueryString} ";

                string status = $" {context.Response.StatusCode} {(HttpStatusCode)context.Response.StatusCode} ";

                IPAddress ip = context.Request.HttpContext.Connection.RemoteIpAddress;
                if (ip != null)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ip = Dns.GetHostEntry(ip)
                        .AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }
                }
                string addr = $"{ip?.ToString() ?? ""}";
                await hubContext.Clients.All.SendAsync("pipelineMessage", stamp, method, path, addr, status);               
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<GenericHub>("/generichub");
            });


           


        }
    }
}
