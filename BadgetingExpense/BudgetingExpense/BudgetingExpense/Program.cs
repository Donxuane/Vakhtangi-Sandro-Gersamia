using System.Data.Common;
using BudgetingExpense.api.Configuration;
using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.UnitOfWork;
using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpenses.Service.IServiceContracts;
using BudgetingExpenses.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("default2")));

builder.Services.AddIdentity<IdentityModel, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<DbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("default2")));
builder.Services.ConfigureJWTBearerToken(builder.Configuration);
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
