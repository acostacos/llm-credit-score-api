using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace llm_credit_score_api.Models
{
    public class FinancialRatio
    {
        [Key, Column("company_financial_ratio_id")]
        public int FinancialRatioId { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("company_name")]
        public string? CompanyName { get; set; }
        [Column("fiscal_year")]
        public int FiscalYear { get; set; }
        [Column("latest_update_date")]
        public DateTime LatestUpdateDate { get; set; }
        [Column("shareholders_equity")]
        public float? ShareholdersEquity { get; set; }
        [Column("cash_and_cash_equivalents")]
        public float? CashAndCashEquivalents { get; set; }
        [Column("total_current_asset")]
        public float? TotalCurrentAsset { get; set; }
        [Column("total_current_liab")]
        public float? TotalCurrentLiab { get; set; }
        [Column("long_term_debt")]
        public float? LongTermDebt { get; set; }
        [Column("short_term_investment")]
        public float? ShortTermInvestment { get; set; }
        [Column("other_short_term_liab")]
        public float? OtherShortTermLiab { get; set; }
        [Column("shares_outstanding")]
        public float? SharesOutstanding { get; set; }
        [Column("current_debt")]
        public float? CurrentDebt { get; set; }
        [Column("total_asset")]
        public float? TotalAsset { get; set; }
        [Column("total_equity")]
        public float? TotalEquity { get; set; }
        [Column("total_liab")]
        public float? TotalLiab { get; set; }
        [Column("net_income")]
        public float? NetIncome { get; set; }
        [Column("total_revenue")]
        public float? TotalRevenue { get; set; }
        [Column("inventory")]
        public float? Inventory { get; set; }
        [Column("investment_in_assets")]
        public float? InvestmentInAssets { get; set; }
        [Column("net_debt")]
        public float? NetDebt { get; set; }
        public virtual Company? Company { get; set; }
    }
}
