using Moq;
using Moq.Protected;
using Rhyze.Data;
using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Data
{
    [Trait(nameof(Data), nameof(Database))]
    public class DatabaseTests
    {
        private readonly Database _db;
        private readonly Mock<IConnectionContext> _context = new Mock<IConnectionContext>(MockBehavior.Strict);
        private readonly Mock<DbConnection> _conn = new Mock<DbConnection>();

        public DatabaseTests()
        {
            _db = new Database(_context.Object);
            _context.Setup(c => c.CreateDbConnection())
                    .Returns(_conn.Object);
        }

        [Fact]
        public async Task ExecuteAsync_For_Queries_Creates_A_New_Connection()
        {
            var query = new Mock<IQueryAsync<int>>();

            await _db.ExecuteAsync(query.Object);

            _context.Verify(c => c.CreateDbConnection(), Times.Once());
            _conn.Verify(c => c.OpenAsync(default), Times.Once());
            _conn.Protected().Verify("Dispose", Times.Once(), ItExpr.Is<bool>(b => b == true));
        }

        [Fact]
        public async Task ExecuteAsync_For_Commands_Creates_A_New_Connection()
        {
            var cmd = new Mock<ICommandAsync>();

            await _db.ExecuteAsync(cmd.Object);

            _context.Verify(c => c.CreateDbConnection(), Times.Once());
            _conn.Verify(c => c.OpenAsync(default), Times.Once());
            _conn.Protected().Verify("Dispose", Times.Once(), ItExpr.Is<bool>(b => b == true));
        }

        [Fact]
        public async Task ExecuteAsync_Dispatches_To_ExecuteAsync_On_The_Query()
        {
            var query = new Mock<IQueryAsync<int>>();

            await _db.ExecuteAsync(query.Object);

            query.Verify(q => q.ExecuteAsync(_conn.Object), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_Dispatches_To_ExecuteAsync_On_The_Command()
        {
            var cmd = new Mock<ICommandAsync>();

            await _db.ExecuteAsync(cmd.Object);

            cmd.Verify(c => c.ExecuteAsync(_conn.Object), Times.Once());
        }
    }
}