using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UplLogEmail.Constants;
using UplLogEmail.Context;
using UplLogEmail.Models.Data;
using UplLogEmail.Models.Tables;
using UplLogEmail.Services;
using UplLogEmail.Utils;

try
{
    IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(FilePaths.APP_BASE_PATH)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    var serviceCollection = new ServiceCollection();
    serviceCollection.AddSingleton(config);
    serviceCollection.AddHttpClient();
    serviceCollection.AddServices();

    IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

    var reportsTable = new TSC_Reports(config);

    // var states = await reportsTable.Set<StatesTable>().ToListAsync();
    var states = new List<StatesTable>() { new() { State = "NJ" } };

    var contextFactory = serviceProvider.GetRequiredService<IContextFactory>();
    var facilitiesGetter = serviceProvider.GetRequiredService<IFacilitiesGetter>();

    Log.Information("Application started");

    foreach (var state in states)
    {
        Log.Information("Processing state: {State}", state.State);

        var context = contextFactory.GetStateContext(state.State);

        var facilities = new List<FacilityData>();

        try
        {
            facilities = await facilitiesGetter.GetFacilities(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching facilities for {State}", state.State);
            continue;
        }

        foreach (var facility in facilities)
        {
            Log.Information(
                "Processing facility: {Facility}, Contacts: {Contacts}",
                facility.FacName,
                facility.Contacts?.Count
            );

            try
            {
                var LogEmailer = serviceProvider.GetRequiredService<ILogEmailer>();
                await LogEmailer.EmailLogs(facility, context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending email for {Facility}", facility.FacName);
            }
        }
    }

    Log.Information("Application completed successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed");
}
finally
{
    Log.CloseAndFlush();
}
