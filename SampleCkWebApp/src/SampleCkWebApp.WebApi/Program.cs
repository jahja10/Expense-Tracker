using SampleCkWebApp.Application;
using SampleCkWebApp.WebApi;
using Serilog;
using System.Text.Json.Serialization;



using SampleCkWebApp.Application.Users;
using SampleCkWebApp.Application.Users.Interfaces.Application;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Users;


using SampleCkWebApp.Application.Categories;
using SampleCkWebApp.Application.Categories.Interfaces.Application;
using SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Categories;

using SampleCkWebApp.Application.PaymentMethods;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.PaymentMethods;

using SampleCkWebApp.Application.Transactions;
using SampleCkWebApp.Application.Transactions.Interfaces.Application;
using SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Transactions;



using SampleCkWebApp.Infrastructure.Common;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.RecurringTransactions;
using SampleCkWebApp.Application.RecurringTransactions;


using SampleCkWebApp.Application.Budgets.Interfaces.Application;
using SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Budgets;
using SampleCkWebApp.Application.Budgets;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("ENV Database__ConnectionString = " + Environment.GetEnvironmentVariable("Database__ConnectionString"));
Console.WriteLine("CFG Database:ConnectionString = " + builder.Configuration["Database:ConnectionString"]);

Directory.SetCurrentDirectory(AppContext.BaseDirectory);
builder.Configuration.SetBasePath(AppContext.BaseDirectory);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.TryGetValue("RequestPath", out var property)
        && property.ToString().StartsWith("\"/health"))
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog(Log.Logger);

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
});


var dbOptions = builder.Configuration.GetDatabaseOptions();
if (dbOptions is null)
{
    throw new InvalidOperationException("Missing 'Database' section in configuration.");
}
builder.Services.TryAddDatabaseOptions(dbOptions);


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
builder.Services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();

builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();



builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

Log.Logger.Information("Application starting");

var app = builder.Build();

app.UseRouting();

ServicePool.Create(app.Services);

app.UseExceptionHandler("/error");

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.ConfigureAuditLogging(httpContext);
    };
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Start();

Log.Logger.Information("Application started");
foreach (var url in app.Urls)
{
    Log.Logger.Information("Listening on: {url}", url);
}

app.WaitForShutdown();
Log.Logger.Information("Application shutdown gracefully");
