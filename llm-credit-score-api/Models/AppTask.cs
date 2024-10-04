using System.ComponentModel.DataAnnotations.Schema;

namespace llm_credit_score_api.Models
{
    public class AppTask
    {
        [Column("task_id")]
        public int Id { get; set; }
        [Column("task_key")]
        public required string TaskKey { get; set; }
        [Column("status")]
        public required string Status { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("report_id")]
        public int? ReportId { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
    }
}
