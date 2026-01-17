using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Users.Interfaces;
using SampleCkWebApp.Infrastructure.Security;
using SampleCkWebApp.Infrastructure.Common;

namespace SampleCkWebApp.Infrastructure.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetDatabaseOptions();
        services.TryAddDatabaseOptions(dbOptions);

        services.TryAddScoped<IUserRepository, UserRepository>();

        services.TryAddScoped<IPasswordHasher, BCryptPasswordHasher>();

        services.TryAddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
