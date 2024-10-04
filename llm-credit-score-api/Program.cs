using llm_credit_score_api;
using llm_credit_score_api.Data;
using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IGeneratorService, GeneratorService>();

builder.Services.AddSingleton(Channel.CreateUnbounded<AppTask>(new UnboundedChannelOptions() { SingleReader = true }));
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<AppTask>>().Reader);
builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<AppTask>>().Writer);

builder.Services.AddHostedService<GeneratorWorker>();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlite"))
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("https://localhost:5000");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
