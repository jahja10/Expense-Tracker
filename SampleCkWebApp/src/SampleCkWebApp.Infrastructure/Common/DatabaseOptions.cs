using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace SampleCkWebApp.Infrastructure.Common;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string? ConnectionString { get; init; }

    public static ValidateOptionsResult Validate(DatabaseOptions? options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail($"Configuration section '{SectionName}' is null.");

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            return ValidateOptionsResult.Fail($"Property '{nameof(ConnectionString)}' is required.");

        return ValidateOptionsResult.Success;
    }
}

public static class DatabaseOptionsExtensions
{
    public static IServiceCollection TryAddDatabaseOptions(this IServiceCollection services, DatabaseOptions? options = null)
    {
        var validationResult = DatabaseOptions.Validate(options);
        if (!validationResult.Succeeded)
            throw new OptionsValidationException(DatabaseOptions.SectionName, typeof(DatabaseOptions), validationResult.Failures);

        services.TryAddSingleton(options!);
        return services;
    }

    public static DatabaseOptions? GetDatabaseOptions(this IConfiguration configuration)
    {
        var section = configuration.GetSection(DatabaseOptions.SectionName);
        if (!section.Exists()) return null;

        DatabaseOptions options = new();
        section.Bind(options);
        return options;
    }
}
