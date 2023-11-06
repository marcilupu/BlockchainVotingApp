using BlockchainVotingApp.AppCode.Services.Users;
using BlockchainVotingApp.Core.Extensions;
using BlockchainVotingApp.Core.Infrastructure;
using BlockchainVotingApp.Data.Ef.Config;
using BlockchainVotingApp.SmartContract.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddCoreServices();
builder.Services.AddSmartContractService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
