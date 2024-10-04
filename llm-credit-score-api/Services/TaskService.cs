using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories.Interfaces;
using llm_credit_score_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace llm_credit_score_api.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ChannelWriter<AppTask> _channel;
        private readonly ILogger<TaskService> _logger;

        public TaskService(IUnitOfWork unitOfWork, ChannelWriter<AppTask> channel, ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _channel = channel;
            _logger = logger;
        }

        public async Task<GetTaskResponse> GetTask(GetTaskRequest request)
        {
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var query = request.PageSize > 0 ? taskRepo.Query(request.PageNum, request.PageSize) : taskRepo.Query();
                var tasks = await query
                    .Include(x => x.Report)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
                return new GetTaskResponse() { Tasks = tasks };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetTaskResponse() { Error = ex.Message };
            }
        }

        public async Task<GetTaskResponse> GetTask(int id)
        {
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var task = await taskRepo.Query(x => x.TaskId == id)
                    .Include(x => x.Report)
                    .FirstOrDefaultAsync();
                var tasks = task != null ? new List<AppTask>() { task } : [];
                return new GetTaskResponse() { Tasks = tasks };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GetTaskResponse() { Error = ex.Message };
            }
        }

        public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request)
        {
            try
            {
                var taskRepo = _unitOfWork.GetRepository<AppTask>();
                var task = new AppTask()
                {
                    TaskKey = request.TaskKey,
                    CompanyId = request.CompanyId,
                    Status = TaskStat.Queued,
                    CreateDate = DateTime.Now,
                };
                taskRepo.Add(task);

                await _unitOfWork.SaveChangesAsync();
                await _channel.WriteAsync(task);
                return new CreateTaskResponse() { Task = task };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new CreateTaskResponse() { Error = ex.Message };
            }
        }
    }
}
