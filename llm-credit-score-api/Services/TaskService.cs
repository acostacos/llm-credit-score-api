using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class TaskService : ITaskService
    {
        public GetTaskResponse GetTask(GetTaskRequest request)
        {
            return new GetTaskResponse();
        }

        public CreateTaskResponse CreateTask(CreateTaskRequest request)
        {
            var task = new Models.Task()
            {
                Status = TaskStat.Queued,
            };
            return new CreateTaskResponse();
        }
    }
}
