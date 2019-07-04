using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LashmerAdmin.Data;
using LashmerAdmin.Helper;
using LashmerAdmin.Helper.Auth;
using LashmerAdmin.Models;
using LashmerAdmin.Models.DataModels;
using LashmerAdmin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.IdentityModel.Tokens;

namespace LashmerAdmin
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey))
            ;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
	    public void ConfigureServices(IServiceCollection services)
	    {
		    //services.Configure<CookiePolicyOptions>(options =>
		    //{
		    //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
		    //    options.CheckConsentNeeded = context => true;
		    //    options.MinimumSameSitePolicy = SameSiteMode.None;
		    //});

		    services.AddDbContext<ApplicationDbContext>(options =>
			    options.UseSqlServer(
				    Configuration.GetConnectionString("DefaultConnection")));

		    services.AddTransient<IEmailSender, LashmerEmailSender>();
		    services.AddSingleton<IJwtFactory, JwtFactory>();
		    ConfigAuthentication(services);

		    //services.AddIdentity<ApplicationUser, IdentityRole>()
		    //    .AddEntityFrameworkStores<ApplicationDbContext>()
		    //    .AddDefaultTokenProviders();

		    // add identity
		    services.AddIdentityCore<ApplicationUser>()
			    .AddRoles<IdentityRole>()
			    .AddSignInManager()
			    .AddEntityFrameworkStores<ApplicationDbContext>()
			    .AddDefaultTokenProviders();

		    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		    var corsSetting = Configuration.GetSection("Cors").Get<Cors>();
		    if (corsSetting != null)
		    {
			    var origins = corsSetting.AllowedOrigins.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

			    services.AddCors(x => x.AddDefaultPolicy(builder =>
			    {
				    builder.WithOrigins(origins)
					    .AllowAnyHeader()
					    .AllowAnyMethod();
			    }));
		    }

		    services.AddSingleton(CreateLogger(HostEnvironment, Configuration));
	    }

	    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
	        app.UseCors();
			app.UseAuthentication();
      //app.UseMvc();
      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
      //app.UseSpa(spa =>
      //{
      //    spa.Options.SourcePath = "ClientApp";
      //    if (env.IsDevelopment())
      //    {
      //        spa.UseAngularCliServer(npmScript: "start");
      //    }
      //});

      CreateUserRoles(provider).Wait();
        }

        private void ConfigAuthentication(IServiceCollection services)
        {
            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser",
                    policy => policy.RequireClaim(Constants.JwtClaimIdentifiers.Rol,
                        Constants.JwtClaims.ApiAccess));
            });
        }

        private ILogger CreateLogger(IHostingEnvironment env, IConfiguration configuration)
        {
            var environment = env.EnvironmentName;
            var logpath = env.ContentRootPath ?? "";
            var assemblyTitle = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);

            var config = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName ?? assemblyTitle)
                .Enrich.WithProperty("Environment", string.IsNullOrEmpty(environment) ? "PROD" : environment)
                //.Destructure.UsingAttributes()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.File(Path.Combine(logpath, $"Logs\\{env.ApplicationName}-{DateTime.Now.Date:yyyy-MM-dd}.log"),
                    retainedFileCountLimit: null,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {SourceContext} - ({MachineName}|{HttpRequestId}|{UserName}) {Message}{NewLine}{Exception}");

            return config.CreateLogger();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Adding Admin Role
            var roleCheck = await roleManager.RoleExistsAsync("Admin").ConfigureAwait(false);
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await roleManager.CreateAsync(new IdentityRole("Admin")).ConfigureAwait(false);
            }

            //Adding Sales Role
            roleCheck = await roleManager.RoleExistsAsync("Sales").ConfigureAwait(false);
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await roleManager.CreateAsync(new IdentityRole("Sales")).ConfigureAwait(false);
            }

            //Assign Admin role to the God User
            //login id for Admin management
            var user = await userManager.FindByEmailAsync("okfeng100@gmail.com").ConfigureAwait(false);
            if (user != null && !await userManager.IsInRoleAsync(user, "Admin").ConfigureAwait(false))
            {
                await userManager.AddToRoleAsync(user, "Admin").ConfigureAwait(false);
            }
        }
    }
}
