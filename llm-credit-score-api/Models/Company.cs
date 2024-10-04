namespace llm_credit_score_api.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CountryName { get; set; }
        public int SecurityNumber { get; set; }
        public string? Ticker { get; set; }
        public string? IdBbUnique { get; set; }
        public string? IdBbCompany { get; set; }
        public string? SecurityType { get; set; }
        public string? MarketStatus { get; set; }
        public int ExchangeCountryId { get; set; }
        public int DomicileId { get; set; }
        public int IndustrySectorNum { get; set; }
        public int IndustryGroupNum { get; set; }
        public int IndustrySubGroupNum { get; set; }
        public int Revision { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Flag { get; set; }
    }
}
