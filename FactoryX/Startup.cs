using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactoryX.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EFCore.DbContextFactory.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using FactoryX.Services;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace FactoryX
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string DefaultConnection = "DefaultConnection"; //DateTime.Now >= DateTime.Parse("2021-12-24") ? "Dconf" : "DefaultConnection"; //"DefaultConnection"; //

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DefaultConnection)));
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();
                       

            // ************************************sql server**********************************************
            // this is like if you had called the AddSqlServerDbContextFactory method.

            var dbLogger = new LoggerFactory(new[]
            {
                new Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });

            services.AddDbContextFactory<EmpresaDbContext>(builder => builder
                .UseSqlServer(Configuration.GetConnectionString("FACTORY_Desarrollo"))
                .UseLoggerFactory(dbLogger));

            //***********************************************************

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                     policy.RequireRole("Admin"));

                options.AddPolicy("RequireUserRole", policy =>
                     policy.RequireRole("User"));

                options.AddPolicy("RequireAvancesRole", policy =>
                     policy.RequireRole("Avances"));

                //options.AddPolicy("CustomSecurityPolicy", policy =>
                //     policy.RequireClaim("IsAdmin", "true"));
            });
                        
            // requires
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //************************* Para el inicio de sesión unico inicio *************************

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache(); //This way ASP.NET Core will use a Memory Cache to store session variables
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1); // It depends on user requirements.
                options.CookieName = ".My.Session"; // Give a cookie name for session which will be visible in request payloads.
            });

            //************************* Para el inicio de sesión unico fin *************************


            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //services.AddDefaultIdentity<IdentityUser>(config =>
            //{
            //    config.SignIn.RequireConfirmedEmail = true;
            //});


            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //***************** Para obtener la IP ******************
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession(); //make sure add this line before UseMvc()

            //app.UseSignalR(x => 
            //{
            //    x.MapHub<ValidaUsrHub>("/validaUsrHub");                
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa");
        }
    }
}
