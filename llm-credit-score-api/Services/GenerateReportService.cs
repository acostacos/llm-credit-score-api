using llm_credit_score_api.Messages;
using llm_credit_score_api.Repository.Interfaces;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class GenerateReportService : IGenerateReportService
    {
        private ICompanyRepository _companyRepository;

        public GenerateReportService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public GenerateReportResponse GenerateReport(GenerateReportRequest request)
        {
            try
            {
                var company = _companyRepository.Get(request.CompanyId);
                if (company == null)
                {
                    throw new Exception("Invalid company");
                }

                // Get company data

                return new GenerateReportResponse() { Company = company };
            }
            catch (Exception ex)
            {
                return new GenerateReportResponse() { Exception = ex };
            }
        }
    }
}
