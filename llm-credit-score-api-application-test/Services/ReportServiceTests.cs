using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services;
using llm_credit_score_api.Services.Interfaces;
using llm_credit_score_api_application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace llm_credit_score_api_application_test.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly Mock<ILogger<ReportService>> _mockLogger;
        private readonly Mock<IReportRepository> _mockReportRepository;
        private readonly Mock<IRepository<Company>> _mockCompanyRepository;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTaskService = new Mock<ITaskService>();
            _mockLogger = new Mock<ILogger<ReportService>>();
            _mockReportRepository = new Mock<IReportRepository>();
            _mockCompanyRepository = new Mock<IRepository<Company>>();

            _mockUnitOfWork.Setup(x => x.GetRepository<Report>()).Returns(_mockReportRepository.Object);
            _mockUnitOfWork.Setup(x => x.GetRepository<Company>()).Returns(_mockCompanyRepository.Object);

            _reportService = new ReportService(_mockUnitOfWork.Object, _mockTaskService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetReport_NoFilter_ShouldReturnAllReports()
        {
            var expected = new List<Report>()
            {
                new Report() { ReportId = 1 },
                new Report() { ReportId = 2 },
            };
            _mockReportRepository.Setup(x => x.GetReportsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

            var request = new GetReportRequest();
            var output = await _reportService.GetReport(request);

            _mockReportRepository.Verify(x => x.GetReportsAsync(0, 0), Times.Once());
            Assert.Null(output.Error);
            Assert.Equal(expected, output.Reports);
        }

        [Fact]
        public async Task GetReport_WithPageInfo_ShouldQueryReportsWithPage()
        {
            var expected = new List<Report>()
            {
                new Report() { ReportId = 1 },
                new Report() { ReportId = 2 },
            };
            _mockReportRepository.Setup(x => x.GetReportsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

            var request = new GetReportRequest() { PageNum = 3, PageSize = 20 };
            var output = await _reportService.GetReport(request);

            _mockReportRepository.Verify(x => x.GetReportsAsync(3, 20), Times.Once());
            Assert.Null(output.Error);
            Assert.Equal(expected, output.Reports);
        }

        [Fact]
        public async Task GetReport_WithError_ShouldReturnError()
        {
            var testException = new Exception("Test Error Message");
            _mockReportRepository.Setup(x => x.GetReportsAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(testException);

            var request = new GetReportRequest();
            var output = await _reportService.GetReport(request);

            Assert.Equal(testException.Message, output.Error);
        }

        [Fact]
        public async Task GetReport_WithId_ShouldQuerySpecificReport()
        {
            var expected = new Report() { ReportId = 1 };
            _mockReportRepository.Setup(x => x.GetReportAsync(It.IsAny<int>())).ReturnsAsync(expected);

            var output = await _reportService.GetReport(expected.ReportId);

            _mockReportRepository.Verify(x => x.GetReportAsync(expected.ReportId), Times.Once());
            Assert.Null(output.Error);
            Assert.NotNull(output.Reports);
            Assert.Equal(expected, output.Reports.First());
        }

        [Fact]
        public async Task GetReport_WithIdWithError_ShouldReturnError()
        {
            var testException = new Exception("Test Error Message");
            _mockReportRepository.Setup(x => x.GetReportAsync(It.IsAny<int>())).ThrowsAsync(testException);

            var output = await _reportService.GetReport(0);

            Assert.Equal(testException.Message, output.Error);
        }

        [Fact]
        public async Task CreateReport_Success_ShouldReturnNewReport()
        {
            int companyId = 1;
            int taskId = 2;
            string content = "Hello World!";
            _mockReportRepository.Setup(x => x.GetReportByTaskIdAsync(taskId));

            var request = new CreateReportRequest() { CompanyId = companyId, TaskId = taskId, Content = content };
            var output = await _reportService.CreateReport(request);

            _mockReportRepository.Verify(x => x.Add(It.IsAny<Report>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.Null(output.Error);
            Assert.NotNull(output.Report);
            Assert.Equal(output.Report.CompanyId, companyId);
            Assert.Equal(output.Report.TaskId, taskId);
            Assert.Equal(output.Report.Content, content);
        }

        [Fact]
        public async Task CreateReport_ExistingReport_ShouldThrowError()
        {
            _mockReportRepository.Setup(x => x.GetReportByTaskIdAsync(It.IsAny<int>())).ReturnsAsync(new Report());

            var request = new CreateReportRequest();
            var output = await _reportService.CreateReport(request);

            Assert.Equal("Report for task already exists", output.Error);
        }

        [Fact]
        public async Task CreateReport_WithError_ShouldReturnError()
        {
            var testException = new Exception("Test Error Message");
            _mockReportRepository.Setup(x => x.GetReportByTaskIdAsync(It.IsAny<int>())).ThrowsAsync(testException);

            var request = new CreateReportRequest();
            var output = await _reportService.CreateReport(request);

            Assert.Equal(testException.Message, output.Error);
        }

        [Fact]
        public async Task GenerateReport_Success_ShouldReturnNewTask()
        {
            var company = new Company() { CompanyId = 1 };
            var task = new AppTask() { TaskId = 1, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Queued };
            _mockCompanyRepository.Setup(x => x.GetByIdAsync(company.CompanyId)).ReturnsAsync(company);
            _mockTaskService.Setup(x => x.CreateTask(It.IsAny<CreateTaskRequest>()))
                .ReturnsAsync(new CreateTaskResponse() { Task = task });

            var request = new GenerateReportRequest() { CompanyId = company.CompanyId };
            var output = await _reportService.GenerateReport(request);

            _mockCompanyRepository.Verify(x => x.GetByIdAsync(company.CompanyId), Times.Once);
            _mockTaskService.Verify(x => x.CreateTask(It.IsAny<CreateTaskRequest>()), Times.Once);
            Assert.Equal(task, output.Task);
            Assert.Equal(company, output.Company);
        }

        [Fact]
        public async Task GenerateReport_InvalidCompany_ShouldThrowError()
        {
            _mockCompanyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()));

            var request = new GenerateReportRequest() { CompanyId = 1 };
            var output = await _reportService.GenerateReport(request);

            Assert.Equal("Invalid company passed", output.Error);
        }

        [Fact]
        public async Task GenerateReport_NoTaskCreated_ShouldThrowError()
        {
            var company = new Company() { CompanyId = 1 };
            _mockCompanyRepository.Setup(x => x.GetByIdAsync(company.CompanyId)).ReturnsAsync(company);
            _mockTaskService.Setup(x => x.CreateTask(It.IsAny<CreateTaskRequest>()))
                .ReturnsAsync(new CreateTaskResponse());

            var request = new GenerateReportRequest() { CompanyId = company.CompanyId };
            var output = await _reportService.GenerateReport(request);

            Assert.Equal("Task not created", output.Error);
        }

        [Fact]
        public async Task GenerateReport_WithError_ShouldThrowError()
        {
            var testException = new Exception("Test Error Message");
            _mockCompanyRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(testException);

            var request = new GenerateReportRequest() { CompanyId = 1 };
            var output = await _reportService.GenerateReport(request);

            Assert.Equal(testException.Message, output.Error);
        }
    }
}
