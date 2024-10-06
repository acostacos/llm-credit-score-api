using llm_credit_score_api.Data.Interfaces;
using llm_credit_score_api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace llm_credit_score_api_application_test.Repositories
{
    public class RepositoryTests
    {
        private readonly Mock<IAppDbContext> _mockAppDbContext;
        private readonly Mock<DbSet<TestDbObject>> _mockDbSet;
        private readonly Repository<TestDbObject> _repository;

        public RepositoryTests()
        {
            _mockAppDbContext = new Mock<IAppDbContext>();
            _mockDbSet   = new Mock<DbSet<TestDbObject>>();

            _mockAppDbContext.Setup(x => x.Set<TestDbObject>()).Returns(_mockDbSet.Object);

            _repository = new Repository<TestDbObject>(_mockAppDbContext.Object);
        }

        [Fact]
        public void Add_Success_ShouldAddEntityToSet()
        {
            var testObj = new TestDbObject() { Id = 1 };
            _repository.Add(testObj);

            _mockAppDbContext.Verify(x => x.Set<TestDbObject>(), Times.Once);
            _mockDbSet.Verify(x => x.Add(testObj), Times.Once);
        }

        
        [Fact]
        public void Query_Success_ShouldReturnQueryableDbSet()
        {
            var expected = new List<TestDbObject>() {
                new TestDbObject() { Id = 1 },
                new TestDbObject() { Id = 2 },
            }.AsQueryable();
            _mockDbSet.Setup(x => x.AsQueryable()).Returns(expected);

            var output = _repository.Query();

            _mockAppDbContext.Verify(x => x.Set<TestDbObject>(), Times.Once);
            _mockDbSet.Verify(x => x.AsQueryable(), Times.Once);
            Assert.Equal(expected, output);
        }

        [Fact]
        public async Task GetIdAsync_Success_ShouldReturnRecordWithId()
        {
            var expected = new TestDbObject() { Id = 1 };
            _mockDbSet.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(expected);

            var output = await _repository.GetByIdAsync(expected.Id);

            _mockAppDbContext.Verify(x => x.Set<TestDbObject>(), Times.Once);
            _mockDbSet.Verify(x => x.FindAsync(expected.Id), Times.Once);
            Assert.Equal(expected, output);
        }
    }
}
