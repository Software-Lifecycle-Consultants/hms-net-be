using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.FileService;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Polly;
using Swashbuckle.AspNetCore.Filters;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        // Adding authorize header in swagger - Changes
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            // Register a custom operation filter to handle FromForm parameters (nested objects)
            options.OperationFilter<FromFormOperationFilter>();
        });

        // Configuration for request body size limit - this is required when uploading large files
        IConfiguration configuration = builder.Configuration;
        long requestBodyLimit = configuration.GetValue<long>("RequestBodyLimit", 30000000); // Default to 30 MB if not specified

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = requestBodyLimit;
        });

        // builder.Services.AddDbContext<HMSDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        //adding mysql
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HMSDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddLogging(); //registers logging services with the dependency injection container, allowing the application to use logging
        //configuring logging
        builder.Logging.ClearProviders();//clear microsoft defaults
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")); //get appsettings configs
        //add debug windows we need
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        //add authentication and identity API endpoints - Changes
        builder.Services.AddAuthentication();

        builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<HMSDBContext>();

        builder.Services.AddScoped<IRepositoryService<Contact>, ContactsRepositoryService>();
        builder.Services.AddScoped<IRepositoryService<AdminRoom>, AdminRoomRepositoryService>();
        builder.Services.AddScoped<IRepositoryService<Room>, RoomRepositoryService>();
        builder.Services.AddScoped<IRepositoryService<Image>, ImageRepositoryService>();
        builder.Services.AddTransient<IFileService, ImageFileService>();
        //builder.Services.AddSingleton(typeof(ILogger), typeof(ILogger<Program>));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        app.UseSwagger();
        app.UseSwaggerUI();

        //map Identity APIs - Changes        
        app.MapIdentityApi<IdentityUser>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors(options =>
        {
            options
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
        );

        // Ensure Uploads directory exists
         var uploadsPath = Path.Combine(builder.Environment.ContentRootPath,"Uploads");
         if(!Directory.Exists(uploadsPath))
             Directory.CreateDirectory(uploadsPath);

        //access image files
        app.UseStaticFiles(new StaticFileOptions
        { 
            FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Uploads")),
            RequestPath = "/Resources"
        });

        try
        {
            //Using Polly Nuget Package:Polly is a .NET resilience and transient-fault-handling library that allows you to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback.
            //Install Polly.Extensions.Http :it provides support for asynchronous policies
            //https://www.nuget.org/packages/polly/

            // Retry policy for ensuring the database is ready
            var retryPolicy = Policy
                .Handle<MySqlException>()
                .WaitAndRetryAsync(50, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
               

            await retryPolicy.ExecuteAsync(async () =>
            {
              

                //Apply DB migrations
                using (var scope = app.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetRequiredService<HMSDBContext>();
                        await context.Database.MigrateAsync(); // Apply migrations
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                // //add role data seeding to app
                using (var scope = app.Services.CreateScope())
                {
                    try
                    {

                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        var roles = new[] { "Admin", "User", "Member" };

                        foreach (var role in roles)
                        {
                            if (!await roleManager.RoleExistsAsync(role))
                                await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

                using (var scope = app.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    string email = "admin@admin.com";
                    string password = "Test1234@";

                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new IdentityUser();
                        user.UserName = email;
                        user.Email = email;

                        await userManager.CreateAsync(user, password);

                        await userManager.AddToRoleAsync(user, "Admin");
                    }

                }
            });
        }
        catch (Exception)
        {
            throw;
            //handle later with logging
            //"An error occurred while migrating or seeding the database."
        }



        app.Run();
    }
}

//https://slc-open-hms-api.azurewebsites.net/api/Contacts
