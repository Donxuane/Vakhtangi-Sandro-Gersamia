using BudgetingExpense.api.Configuration;
using BudgetingExpense.api.CustomMiddleware;
using BudgetingExpense.Api.Configuration;
using BudgetingExpense.DataAccess.Configuration;
using BudgetingExpenses.Service.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHostedService<ConfigureDatabase>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorization();
builder.Services.ConfigureDatabaseRules(builder.Configuration);
builder.Services.ConfigureJWTBearerToken(builder.Configuration);
builder.Services.AddILoggerConfiguration();
builder.Services.AddRepositoryInstances();
builder.Services.AddServiceInstances();
//builder.Services.AddScoped<ConfigureRoles>();


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var roles = scope.ServiceProvider.GetRequiredService<ConfigureRoles>();
//    roles.RoleCeeder().GetAwaiter().GetResult();
//}
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
