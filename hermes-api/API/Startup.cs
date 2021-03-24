using System;
using System.Text;
using Hermes.API.Controllers;
using Hermes.Core;
using Hermes.Shell;
using Hermes.Shell.Read;
using Hermes.Shell.Write;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => c.AddDefaultPolicy(build => {
                build.WithOrigins("http://localhost:3000", Configuration["UIServer"]);
                build.AllowCredentials();
                build.AllowAnyHeader();
                build.AllowAnyMethod();
            }));
            InjectDependencies(services);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    var secret = Encoding.ASCII.GetBytes(Configuration["Authentication:Secret"]);
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("GoogleToken", options =>
                {
                    options.IncludeErrorDetails = true;
                    options.Authority = Configuration["Authentication:Google:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Google:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:Google:Audience"],
                        ValidateLifetime = true
                    };
                });
            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }
            app.UseCors();
            app.Use(async (context, next) =>
            {
                try {
                    await next();
                } catch(Exception ex){
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Exception: ");
                    Console.ResetColor();
                    Console.WriteLine(ex.Message);
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                        Error = ex.Message
                    }));
                }
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QueriesHub>("/api/hubs/queries");
            });
        }

        void InjectDependencies(IServiceCollection services)
        {
            // Init DB
            services.AddSingleton<SQLConnection>(new SQLConnection(Configuration["SQLConnection"]));
            services.AddSingleton<ConnectionFactory>(new ConnectionFactory {
                HostName = Configuration["Queue:HostName"],
                UserName = Configuration["Queue:UserName"],
                Password = Configuration["Queue:Password"]
            });
            // Inject Queries
            services.AddSingleton<IArticleQueries, ArticleQuery>();
            services.AddSingleton<IArticleTemplateQueries, ArticleTemplateQuery>();
            services.AddSingleton<IRoomQueries, RoomQuery>();
            services.AddSingleton<IUserQueries, UserQuery>();
            services.AddSingleton<ILanguageQueries, LanguageQuery>();
            services.AddSingleton<ITopicQueries, TopicQuery>();
            services.AddSingleton<IForumPostQueries, ForumPostQuery>();
            // Inject Interpreters
            services.AddSingleton<DomainInterpreter>();
        }
    }
}
