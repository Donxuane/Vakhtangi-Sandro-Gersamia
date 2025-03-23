using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.Api.Configuration;
using BudgetingExpense.Api.CustomFilters;
using BudgetingExpense.Api.CustomMiddleware;
using BudgetingExpense.DataAccess.Configuration;
using BudgetingExpenses.Service.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.ConfigureDatabaseRules(builder.Configuration);
builder.AddMultipleJsonFileConfiguration();
builder.Services.AddHostedService<ConfigureDatabase>();
builder.Services.AddILoggerConfiguration();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorization();
builder.Services.ConfigureJWTBearerToken(builder.Configuration);
builder.Services.AddRepositoryInstances();
builder.Services.AddServiceInstances();
builder.Services.AddScoped<ConfigureSeeding>();

builder.Services.AddScoped<CategoryValidationFilter>();


var app = builder.Build();
app.UseMiddleware<ExceptionLoggerMiddleware>();
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
