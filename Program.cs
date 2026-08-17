using System.Text;
using Homework1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Homework1.DL;
using Homework1.DL.Interfaces;
using Homework1.BL;
using Homework1.BL.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<KeyService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<IUserDL, UserDL>();
builder.Services.AddScoped<IAuthBL, AuthBL>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
// Configure JWT authentication using the same builder

var signingKeyText =
    builder.Configuration["Jwt:SigningKey"]
    ?? throw new Exception("Thiếu Jwt:SigningKey");

var encryptionKeyText =
    builder.Configuration["Jwt:EncryptionKey"]
    ?? throw new Exception("Thiếu Jwt:EncryptionKey");


var signingKey =
    new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(signingKeyText)
    );

var encryptionKey =
    new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(encryptionKeyText)
    );
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // ============================
                // KIỂM TRA CHỮ KÝ
                // ============================

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    signingKey,


                // ============================
                // GIẢI MÃ JWE
                // ============================

                TokenDecryptionKey =
                    encryptionKey,


                // ============================
                // KIỂM TRA ISSUER
                // ============================

                ValidateIssuer = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],


                // ============================
                // KIỂM TRA AUDIENCE
                // ============================

                ValidateAudience = true,

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],


                // ============================
                // KIỂM TRA EXPIRATION
                // ============================

                ValidateLifetime = true,


                // ============================
                // QUAN TRỌNG CHO PHÂN QUYỀN
                // ============================

                RoleClaimType =
                    System.Security.Claims.ClaimTypes.Role,

                NameClaimType =
                    System.Security.Claims.ClaimTypes.Name
            };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowVueApp");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homework1 API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
