using llm_credit_score_api.Messages;

namespace llm_credit_score_api.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<GetTaskResponse> GetTask(GetTaskRequest request);
        public Task<GetTaskResponse> GetTask(int id);
        public Task<CreateTaskResponse> CreateTask(CreateTaskRequest request);
    }
}
