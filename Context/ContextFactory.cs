using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UplLogEmail.Context;

public class ContextFactory(IConfiguration configuration) : IContextFactory
{
    private StateContext? _currentStateContext;

    private string _currentState = string.Empty;

    public StateContext GetStateContext(string stateAbbreviation)
    {
        var state = stateAbbreviation.ToUpperInvariant();

        if (_currentState != state)
        {
            _currentStateContext?.Dispose();
            _createStateContext(state);
            _currentState = state;
        }

        return _currentStateContext
            ?? throw new InvalidOperationException("State context not created");
    }

    private void _createStateContext(string state)
    {
        var baseConnectionString = configuration.GetConnectionString("BaseConnectionString");
        var databaseName = state switch
        {
            "TEST" => "TEST_TSC",
            _ => $"TSC_{state}",
        };
        var connectionString = $"{baseConnectionString}Initial Catalog={databaseName};";

        var optionsBuilder = new DbContextOptionsBuilder<StateContext>();
        optionsBuilder.UseSqlServer(
            connectionString,
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            }
        );
        _currentStateContext = new StateContext(optionsBuilder.Options);
    }
}

public interface IContextFactory
{
    public StateContext GetStateContext(string stateAbbreviation);
}
