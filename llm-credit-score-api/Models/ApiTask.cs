using llm_credit_score_api.Constants;

namespace llm_credit_score_api.Models
{
    public class ApiTask
    {
        public int Id { get; set; }
        public TaskKey TaskKey { get; set; }
        public TaskStat Status { get; set; }
    }
}
