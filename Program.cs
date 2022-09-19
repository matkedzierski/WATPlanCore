using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using WATPlanCore.Aggregators;
using WATPlanCore.Data;
using WATPlanCore.ExternalServices;
using WATPlanCore.ExternalServices.Aggregators;
using WATPlanCore.Models.Settings;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .MinimumLevel.Override("WATPlanCore", LogEventLevel.Debug)
    .WriteTo.Console(LogEventLevel.Information)
    .WriteTo.File(builder.Environment.ContentRootPath + builder.Configuration.GetSection("LogFile").Value,
        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 2000000)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("watplan", policyBuilder =>
    {
        policyBuilder.WithOrigins(
                "https://watplan.web.app",
                "https://watplan.pl",
                "watplan.web.app",
                "watplan.pl",
                "localhost:4200",
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Swagger OpenAPI explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "WATPlan REST API",
            Description = "Interfejs dostępu do planów i wydarzeń WATPlan'u",
            Contact = new OpenApiContact
            {
                Name = "Email",
                Email = "watplanofficial@gmail.com"
            }
        });
        options.UseInlineDefinitionsForEnums();
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
    }
);

logger.Information("Initializing database connection");
//Database Context
builder.Services.AddDbContext<PlansDbContext>(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("Using development SQL connection");
            var conn = builder.Configuration.GetConnectionString("OracleDevDatabase");
            logger.Information(conn);
            Console.WriteLine(conn);
            options.UseOracle(conn ?? string.Empty);
        }
        else
        {
            Console.WriteLine("Using production SQL connection");
            var conn = builder.Configuration.GetConnectionString("MSSQLProductionDatabase");
            logger.Information(conn);
            Console.WriteLine(conn);
            options.UseSqlServer(conn ?? string.Empty);
        }
    }
);

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<PlanSoftSettings>(builder.Configuration.GetSection("PlanSoftSettings"));
builder.Services.AddSingleton<IUnitAggregator, UnitAggregator>();
builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
builder.Services.AddSingleton<IPlanAggregator, PlanAggregator>();
builder.Services.AddTransient<IMailService, MailService>();


var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("watplan");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

// Configure the HTTP request pipeline.
Debug.WriteLine("Enabling swagger");
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Run();