using JobScheduler.Core;
using JobScheduler.Infraestructure.Data;
using JobScheduler.Server.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseWindowsService();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddJobs(builder.Configuration);

const string connectionString = "DataSource=myshareddb;mode=memory;cache=shared";
var keepAliveConnection = new SqliteConnection(connectionString);
keepAliveConnection.Open();

builder.Services.AddDbContext<JobSchedulerDbContext>(options => { options.UseSqlite(connectionString); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }