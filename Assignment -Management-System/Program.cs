
using Assignment__Management_System.Helpers;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assignment__Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(corsOptions =>

                corsOptions.AddPolicy("MyPolicy", CorsPolicy =>

                CorsPolicy.AllowAnyHeader()

                .AllowAnyMethod()

                .AllowAnyOrigin())

            );

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var Connectionstring = config.GetSection("constr").Value;

            builder.Services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(Connectionstring)    
            );

            builder.Services.Configure<JWT>(config.GetSection("JWT"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["JWT:Issuer"],
                    ValidAudience = config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]))
                };
            });

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminServices>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            builder.Services.AddScoped<JWTService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
