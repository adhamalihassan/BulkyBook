using System.Text;
using BulkyBook.Auth;
using BulkyBook.Dependencies;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace BulkyBook
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
            // Add services to the container.
            services.AddControllersWithViews();

            // Get the connection string from appsettings.json
            string? connectionString = Configuration?.GetConnectionString("DefaultConnection");

            // Configure the DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            // Register dependencies
            services.AddScoped<IMyDependency, MyDependency>();
            services.AddSingleton<IMyDependency, MyDependency2>();
            services.AddSingleton<AuthService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // You can set this to true if you have an issuer
                        ValidateAudience = true,
                        ValidateLifetime = true, // This will check if the expiration time is valid
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Configuration["JwtSettings:Audience"],
                        ValidIssuer = Configuration["JwtSettings:Issuer"], // Replace with your actual issuer
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("adham",
                    policy => { policy.Requirements.Add(new RequireCustomClaimRequirement("adham")); });
            });
            services.AddSingleton<IAuthorizationHandler, RequireCustomClaimHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}