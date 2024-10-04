using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace llm_credit_score_api.Models
{
    public class Report
    {
        [Key, Column("report_id")]
        public int ReportId { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("content")]
        public string? Content { get; set; }
        public Company? Company { get; set; }
        public AppTask? Task { get; set; }
    }
}
