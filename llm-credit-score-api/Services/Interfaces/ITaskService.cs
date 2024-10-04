using llm_credit_score_api.Messages;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface ITaskService
    {
        public GetTaskResponse GetTask(GetTaskRequest request);
        public CreateTaskResponse CreateTask(CreateTaskRequest request);
    }
}
