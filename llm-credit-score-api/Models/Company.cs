using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace llm_credit_score_api.Models
{
    public class Company
    {
        [Key, Column("company_id")]
        public int CompanyId { get; set; }
        [Column("company_name")]
        public string? CompanyName { get; set; }
        [Column("country_name")]
        public string? CountryName { get; set; }
        [Column("security_number")]
        public int SecurityNumber { get; set; }
        [Column("ticker")]
        public string? Ticker { get; set; }
        [Column("id_bb_unique")]
        public string? IdBbUnique { get; set; }
        [Column("id_bb_company")]
        public string? IdBbCompany { get; set; }
        [Column("security_type")]
        public string? SecurityType { get; set; }
        [Column("market_status")]
        public string? MarketStatus { get; set; }
        [Column("exchange_country_id")]
        public int ExchangeCountryId { get; set; }
        [Column("domicile_id")]
        public int DomicileId { get; set; }
        [Column("industry_sector_num")]
        public int IndustrySectorNum { get; set; }
        [Column("industry_group_num")]
        public int IndustryGroupNum { get; set; }
        [Column("industry_subgroup_num")]
        public int IndustrySubGroupNum { get; set; }
        [Column("revision")]
        public int Revision { get; set; }
        [Column("start_date")]
        public DateTime? StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("flag")]
        public int Flag { get; set; }
        public List<Report>? Reports { get; set; }
    }
}
