using Soilution.DataService.DataManagement;
using Soilution.DataService.SqlRepository;
using Soilution.DataService.Analytics;
using Soilution.DataService.MockedData;
using Soilution.DataService.DeviceManagement;
using NLog.Web;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Application startup");
try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.RegisterDataManagementApps();
    builder.Services.RegisterDeviceManagementApps();
    builder.Services.SetupSQLRepositories(builder.Configuration);
    //builder.Services.SetupMockedRepositories();
    builder.Services.RegisterAnalyticsSuite();

    // Logging setup
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application startup encountered exception");
    throw;
}
