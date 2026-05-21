using FactoryScheduler.Api.Data;
using FactoryScheduler.Api.Repositories;
using FactoryScheduler.Api.Services;
using FactoryScheduler.Api.BackgroundServices;
using FactoryScheduler.Api.Scheduling;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using IJobRepository = FactoryScheduler.Api.Repositories.IJobRepository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 43));
    options.UseMySql(connectionString, serverVersion);

});

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add repositories and services
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ISchedulingStrategy, EarliestFinishTimeStrategy>();
builder.Services.AddHostedService<JobStatusUpdaterService>();

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/openapi/v1.json"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
