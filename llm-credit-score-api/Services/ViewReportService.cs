using llm_credit_score_api.Messages;
using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api.Services
{
    public class ViewReportService : IViewReportService
    {
        public ViewReportResponse ViewReports()
        {
            Console.WriteLine("view test");
            return new ViewReportResponse();
        }
    }
}
