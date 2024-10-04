using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace llm_credit_score_api.Models
{
    public class AppTask
    {
        [Key, Column("task_id")]
        public int TaskId { get; set; }
        [Column("task_key")]
        public required string TaskKey { get; set; }
        [Column("status")]
        public required string Status { get; set; }
        [Column("message")]
        public string? Message { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("report_id")]
        public int? ReportId { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        public virtual Report? Report { get; set; }
    }
}
