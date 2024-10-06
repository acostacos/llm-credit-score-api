using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services;
using llm_credit_score_api_application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace llm_credit_score_api_application_test.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ChannelWriter<AppTask>> _mockChannel;
        private readonly Mock<ILogger<TaskService>> _mockLogger;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockChannel = new Mock<ChannelWriter<AppTask>>();
            _mockLogger = new Mock<ILogger<TaskService>>();
            _mockTaskRepository = new Mock<ITaskRepository>();

            _mockUnitOfWork.Setup(x => x.GetRepository<AppTask>()).Returns(_mockTaskRepository.Object);
            
            _taskService = new TaskService(_mockUnitOfWork.Object, _mockChannel.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetTask_NoFilter_ShouldReturnAllTasks()
        {
            var expected = new List<AppTask>()
            {
                new AppTask() { TaskId = 1, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Done },
                new AppTask() { TaskId = 2, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Done },
            };
            _mockTaskRepository.Setup(x => x.GetTasksAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

            var request = new GetTaskRequest();
            var output = await _taskService.GetTask(request);

            _mockTaskRepository.Verify(x => x.GetTasksAsync(0, 0), Times.Once());
            Assert.Null(output.Error);
            Assert.Equal(expected, output.Tasks);
        }

        [Fact]
        public async Task GetTask_WithPageInfo_ShouldQueryTasksWithPage()
        {
            var expected = new List<AppTask>()
            {
                new AppTask() { TaskId = 1, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Done },
                new AppTask() { TaskId = 2, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Done },
            };
            _mockTaskRepository.Setup(x => x.GetTasksAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

            var request = new GetTaskRequest() { PageNum = 3, PageSize = 20 };
            var output = await _taskService.GetTask(request);

            _mockTaskRepository.Verify(x => x.GetTasksAsync(3, 20), Times.Once());
            Assert.Null(output.Error);
            Assert.Equal(expected, output.Tasks);
        }

        [Fact]
        public async Task GetTask_WithError_ShouldReturnError()
        {
            var testException = new Exception("Test Error Message");
            _mockTaskRepository.Setup(x => x.GetTasksAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(testException);

            var request = new GetTaskRequest();
            var output = await _taskService.GetTask(request);

            Assert.Equal(testException.Message, output.Error);
        }

        [Fact]
        public async Task GetTask_WithId_ShouldQuerySpecificTask()
        {
            var expected = new AppTask() { TaskId = 1, TaskKey = TaskKey.GenerateReport, Status = TaskStat.Done };
            _mockTaskRepository.Setup(x => x.GetTaskAsync(It.IsAny<int>())).ReturnsAsync(expected);

            var output = await _taskService.GetTask(expected.TaskId);

            _mockTaskRepository.Verify(x => x.GetTaskAsync(expected.TaskId), Times.Once());
            Assert.Null(output.Error);
            Assert.NotNull(output.Tasks);
            Assert.Equal(expected, output.Tasks.First());
        }

        [Fact]
        public async Task GetTask_WithIdWithError_ShouldReturnError()
        {
            var testException = new Exception("Test Error Message");
            _mockTaskRepository.Setup(x => x.GetTaskAsync(It.IsAny<int>())).ThrowsAsync(testException);

            var output = await _taskService.GetTask(0);

            Assert.Equal(testException.Message, output.Error);
        }

        [Fact]
        public async Task CreateTask_Success_ShouldReturnNewTask()
        {
            string taskKey = TaskKey.GenerateReport;
            int companyId = 1;
            _mockChannel.Setup(x => x.WriteAsync(It.IsAny<AppTask>(), It.IsAny<CancellationToken>()));

            var request = new CreateTaskRequest() { TaskKey = taskKey, CompanyId = companyId };
            var output = await _taskService.CreateTask(request);

            _mockTaskRepository.Verify(x => x.Add(It.IsAny<AppTask>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _mockChannel.Verify(x => x.WriteAsync(It.IsAny<AppTask>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Null(output.Error);
            Assert.NotNull(output.Task);
            Assert.Equal(output.Task.TaskKey, taskKey);
            Assert.Equal(output.Task.CompanyId, companyId);
        }

        [Fact]
        public async Task CreateReport_WithError_ShouldReturnError()
        {
            string taskKey = TaskKey.GenerateReport;
            var testException = new Exception("Test Error Message");
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ThrowsAsync(testException);

            var request = new CreateTaskRequest() { TaskKey = taskKey };
            var output = await _taskService.CreateTask(request);

            Assert.Equal(testException.Message, output.Error);
        }
    }
}
