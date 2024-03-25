using HMS.Models;
using HMS.Services.FileService;
using HMS.Services.Repository_Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

internal class Program
{
    private static void Main(string[] args)
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
        });

        // builder.Services.AddDbContext<HMSDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        //adding mysql
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HMSDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddLogging();
        //configuring logging
        builder.Logging.ClearProviders();//clear microsoft defaults
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")); //get appsettings configs
        //add debug windows we need
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        //add authentication and identity API endpoints - Changes
        builder.Services.AddAuthentication();

        builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<HMSDBContext>();

        builder.Services.AddScoped<IRepositoryService<Contact>, ContactsRepositoryService>();
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
        app.MapIdentityApi<AppUser>();

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
        //access image files
        app.UseStaticFiles(new StaticFileOptions
        { 
            FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Uploads")),
            RequestPath = "/Resources"
        });
        app.Run();
    }
}

//https://slc-open-hms-api.azurewebsites.net/api/Contacts
