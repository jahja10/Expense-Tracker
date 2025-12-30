using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.Categories.Interfaces.Application;


namespace SampleCkWebApp.Application.Categories;

public static class DependencyInjection
{
    public static IServiceCollection AddCategoriesApplication(this IServiceCollection services)
    {
        services.TryAddScoped<ICategoryService, CategoryService>();
        return services;
    }
}