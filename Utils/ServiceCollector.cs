using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using UplLogEmail.Constants;
using UplLogEmail.Context;
using UplLogEmail.Services;
using UplLogEmail.Services.EmailService;

namespace UplLogEmail.Utils;

public static class ServiceCollector
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IContextFactory, ContextFactory>();
        serviceCollection.AddScoped<IFacilitiesGetter, FacilitiesGetter>();
        serviceCollection.AddScoped<IUploadLogGetter, UploadLogGetter>();
        serviceCollection.AddScoped<IEmailService, EmailService>();
        serviceCollection.AddScoped<IPdfGetter, PdfGetter>();
        serviceCollection.AddScoped<ILogEmailer, LogEmailer>();

        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        ConfigureSerilog();
    }

    public static void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information,
                theme: AnsiConsoleTheme.Code
            )
            .WriteTo.File(
                $"{FilePaths.LOG_FILE_PATH}.txt",
                restrictedToMinimumLevel: LogEventLevel.Warning,
                rollingInterval: RollingInterval.Month,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 20,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}
