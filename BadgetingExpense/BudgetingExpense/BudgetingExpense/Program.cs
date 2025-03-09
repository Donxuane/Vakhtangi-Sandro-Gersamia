using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.DataAccess.Configuration;
using BudgetingExpenses.Service.Configuration;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/ErrorLogs.log", restrictedToMinimumLevel: LogEventLevel.Error)
    .WriteTo.File("Logs/InformationLogs.log", restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorization();
builder.Services.ConfigureDatabaseRules(builder.Configuration);
builder.Services.ConfigureJWTBearerToken(builder.Configuration);
builder.Services.AddRepositoryInstances();
builder.Services.AddServiceInstances();
builder.Services.AddScoped<ConfigureRoles>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<ConfigureRoles>();
    roles.RoleCeeder().GetAwaiter().GetResult();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<UserCredentialsMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
