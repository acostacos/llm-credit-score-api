using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace llm_credit_score_api.Services
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService; 
        private readonly IReportService _reportService;
        private readonly ILogger<GeneratorService> _logger;

        public GeneratorService(IUnitOfWork unitOfWork, IMessageService messageService, IReportService reportService, ILogger<GeneratorService> logger)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _reportService = reportService;
            _logger = logger;
        }

        public async Task GenerateReport(int taskId)
        {
            AppTask? task = null;
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var companyRepo = _unitOfWork.GetRepository<Company>();

                task = await taskRepo.GetByIdAsync(taskId) ?? throw new Exception("Cannot find task");
                task.Status = TaskStat.InProgress;
                await _unitOfWork.SaveChangesAsync();

                var company = await companyRepo
                    .Query(x => x.CompanyId == task.CompanyId)
                    .Include(x => x.FinancialRatios)
                    .FirstOrDefaultAsync();
                if (company == null)
                {
                    throw new Exception("Invalid company passed");
                }

                var prompt = GeneratePrompt(company);
                var response = await GetLLMResponse(prompt);
                var reportId = await CreateReport(task.TaskId, company.CompanyId, response);

                task.Status = TaskStat.Done;
                task.ReportId = reportId;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (task != null)
                {
                    task.Status = TaskStat.Error;
                    task.Message = ex.Message;
                    await _unitOfWork.SaveChangesAsync();
                }

                _logger.LogError(ex.Message);
                throw;
            }
        }

        private string GeneratePrompt(Company company)
        {
            var prompt = LLMConstants.Prompt;
            var statistics = "";
            if (company.FinancialRatios != null)
            {
                var PROPERTIES_TO_IGNORE = new string[] { "FinancialRatioId", "CompanyId", "CompanyName", "FiscalYear", "LatestUpdateDate", "Company" };
                foreach (var fr in company.FinancialRatios)
                {
                    var statProps = new List<string>();
                    var props = typeof(FinancialRatio).GetProperties();
                    foreach (var prop in props)
                    {
                        var propName = prop.Name;
                        if (!PROPERTIES_TO_IGNORE.Contains(propName))
                        {
                            var propValue = prop.GetValue(fr);
                            if (propValue != null)
                            {
                                statProps.Add($"({propName}, {propValue})");
                            }
                        }
                    }

                    statistics += $"{fr.FiscalYear}: {string.Join(",", statProps)}\n";
                }
            }

            prompt = prompt.Replace("[COMPANY_NAME]", company.CompanyName);
            prompt = prompt.Replace("[STATISTICS]", statistics);
            return prompt;
        }

        private async Task<string> GetLLMResponse(string prompt)
        {
            var body = new LLMRequest()
            {
                Model = LLMConstants.Model,
                Messages = new List<InputMessage>()
                {
                    new InputMessage()
                    {
                        Role = "user",
                        Content = prompt,
                    }
                },
            };
            var jsonDecodeOpt = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
            var response = await _messageService.PostAsync<LLMResponse>(LLMConstants.Url, body, jsonDecodeOpt);
            var message = response?.Choices?.FirstOrDefault(x => x?.Message?.Role == "assistant")?.Message?.Content;
            return message ?? "";
        }

        private async Task<int> CreateReport(int taskId, int companyId, string content)
        {
            var createReportReq = new CreateReportRequest()
            {
                TaskId = taskId,
                CompanyId = companyId,
                Content = content,
            };
            var response = await _reportService.CreateReport(createReportReq);
            if (response.Report == null)
            {
                throw new Exception(response.Error ?? "Report not created");
            }

            return response.Report.ReportId;
        }
    }
}
