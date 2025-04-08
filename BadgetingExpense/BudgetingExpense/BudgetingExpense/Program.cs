using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.Api.Configuration;
using BudgetingExpense.Api.CustomFilters;
using BudgetingExpense.Api.CustomMiddleware;
using BudgetingExpense.DataAccess.Configuration;
using BudgetingExpenses.Service.Configuration;
using BudgetingExpenses.Service.Live;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddHostedService<ConfigureDatabase>();
builder.AddMultipleJsonFileConfiguration();
builder.Services.AddILoggerConfiguration();
builder.Services.ConfigureDatabaseRules(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorization();
builder.Services.ConfigureJWTBearerToken(builder.Configuration)
    .AddRepositoryInstances()
    .AddServiceInstances();
builder.Services.AddScoped<ConfigureSeeding>();
builder.Services.AddScoped<CategoryValidationFilter>();
builder.Services.AddScoped<PropertyNormalizationFilter>();
builder.Services.AddSingleton<LiveLogsForwarder>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("Api",client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("CurrencyRate")["Url"]);
});

var app = builder.Build();
app.MapRazorPages();
app.MapHub<AppLiveHub>("/logHub");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.Services.GetRequiredService<LiveLogsForwarder>().Start();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RefreshExpiredTokensMiddleware>();
app.UseAuthentication();
app.UseMiddleware<UserCredentialsMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
