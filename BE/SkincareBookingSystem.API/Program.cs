using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net.payOS;
//using Net.payOS;
using SkincareBookingSystem.API.Extensions;
using SkincareBookingSystem.API.Middlewares;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.Mapping;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  
            builder.Services.AddControllers();

            // Configure DbContext with SQL Server  
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer(
                //    builder.Configuration.GetConnectionString(StaticConnectionString.SqldbDefaultConnection));
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString(StaticConnectionString.PostgreSqlConnection));
            });

            // Configure Identity  
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Thêm dịch vụ Swagger  
            builder.Services.AddSwaggerGen(options =>
            {
                // Bảo mật Swagger với JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter your token with this format: \"Bearer YOUR_TOKEN\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                // API document
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Skincare Booking API",
                    Version = "v1",
                    Description = "API documentation for Skincare Booking System"
                });
                options.EnableAnnotations();

                // Đọc comment từ XML để hiển thị trên Swagger
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
            
            // Add JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });

            // Lấy thông tin từ `appsettings.json`
            var payOSClientId = builder.Configuration["Environment:PAYOS_CLIENT_ID"]
                                ?? throw new Exception("Cannot find PAYOS_CLIENT_ID");

            var payOSApiKey = builder.Configuration["Environment:PAYOS_API_KEY"]
                              ?? throw new Exception("Cannot find PAYOS_API_KEY");

            var payOSChecksumKey = builder.Configuration["Environment:PAYOS_CHECKSUM_KEY"]
                                   ?? throw new Exception("Cannot find PAYOS_CHECKSUM_KEY");

            // Đăng ký PayOS vào DI Container
            builder.Services.AddSingleton(new PayOS(payOSClientId, payOSApiKey, payOSChecksumKey));

            // Register services from Extensions
            builder.Services.RegisterServices(builder.Configuration);

            var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins(corsOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("CORSMiddleware");

                logger.LogInformation($"Request from origin: {context.Request.Headers["Origin"]}");
                logger.LogInformation($"Request method: {context.Request.Method}");
                logger.LogInformation($"Request path: {context.Request.Path}");

                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.StatusCode = 200;
                    await context.Response.CompleteAsync();
                }
                else
                {
                    await next();
                }

                if (context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    logger.LogInformation(
                        $"Response Access-Control-Allow-Origin: {context.Response.Headers["Access-Control-Allow-Origin"]}");
                }
            });

            app.UseCors("AllowSpecificOrigin");


            // Apply database migrations  
            ApplyMigration(app);

            // Configure the HTTP request pipeline.  
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Thêm dev page trong môi trường phát triển  
            }

            app.UseMiddleware<GlobalExceptionHandllingMiddleware>();

            app.UseHttpsRedirection();

            // Kích hoạt Swagger middleware  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skincare Booking API v1");
                    c.RoutePrefix = "swagger"; 
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skincare Booking API v1");
                    c.RoutePrefix = string.Empty; 
                });
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static void ApplyMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}