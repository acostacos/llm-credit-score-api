using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Models;
using llm_credit_score_api.Repositories;

namespace llm_credit_score_api_application_test.Repositories
{
    public class UnitOfWorkTests
    {
        private readonly Mock<IAppDbContext> _mockAppDbContext;
        private readonly UnitOfWork _unitOfWork;
        public UnitOfWorkTests()
        {
            _mockAppDbContext = new Mock<IAppDbContext>();
            _unitOfWork = new UnitOfWork(_mockAppDbContext.Object);
        }

        [Fact]
        public void GetRepository_Success_ShouldReturnRepositoryOfType()
        {
            var output = _unitOfWork.GetRepository<TestDbObject>();

            Assert.IsType<Repository<TestDbObject>>(output);
        }

        [Fact]
        public void GetRepository_ReportType_ShouldReturnReportRepository()
        {
            var output = _unitOfWork.GetRepository<Report>();

            Assert.IsType<ReportRepository>(output);
        }

        [Fact]
        public void GetRepository_AppTaskType_ShouldReturnTaskRepository()
        {
            var output = _unitOfWork.GetRepository<AppTask>();

            Assert.IsType<TaskRepository>(output);
        }

        [Fact]
        public void GetRepository_CalledMultipleTimes_ShouldReturnSameRepository()
        {
            var expected = _unitOfWork.GetRepository<TestDbObject>();
            var output = _unitOfWork.GetRepository<TestDbObject>();

            Assert.Equal(expected, output);
        }

        [Fact]
        public async Task SaveChangesAsync_Success_ShouldCallDbContextMethod()
        {
            await _unitOfWork.SaveChangesAsync();

            _mockAppDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_Success_ShouldCallDbContextMethod()
        {
            _unitOfWork.Dispose();

            _mockAppDbContext.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
