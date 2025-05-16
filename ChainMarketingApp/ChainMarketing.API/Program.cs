
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Application.Services;
using ChainMarketing.Domain.Interfaces;
using ChainMarketing.Infrastructure.Auth;
using ChainMarketing.Infrastructure.Data;
using ChainMarketing.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ChainMarketing.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IReferralTreeService, ReferralTreeService>();
            builder.Services.AddScoped<IReferralPathRepository, ReferralPathRepository>();
            // Register repository
            builder.Services.AddScoped<IReferralPathRepository, ReferralPathRepository>();
            builder.Services.AddScoped<ICommissionService, CommissionService>();
            builder.Services.AddScoped<IAdminService, AdminService>();  

            // Register service
            builder.Services.AddScoped<ReferralPathService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
            // Configures CORS to allow requests from any origin, with any HTTP method, and any headers.
            // This is useful for allowing the frontend (which might be hosted on a different domain) to communicate with the backend.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin() // Allows any domain to make requests.
                               .AllowAnyMethod() // Allows any HTTP method (GET, POST, PUT, DELETE, etc.).
                               .AllowAnyHeader(); // Allows any HTTP headers to be sent.
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            // Enable the CORS policy defined earlier to allow cross-origin requests.
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();

            app.UseAuthorization();
       

            app.MapControllers();

            app.Run();
        }
    }
}
