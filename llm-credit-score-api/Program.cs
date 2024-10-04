using llm_credit_score_api;
using llm_credit_score_api.Data;
using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Repository;
using llm_credit_score_api.Repository.Interfaces;
using llm_credit_score_api.Services;
using llm_credit_score_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("https://localhost:5000");
builder.Services.AddHostedService<GeneratorWorker>();

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
