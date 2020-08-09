using RepoDb;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class SoftDeleteAlbumCommand : ICommandAsync
    {
        private readonly string _name;

        public SoftDeleteAlbumCommand(string name) => _name = name;

        public async Task ExecuteAsync(IDbConnection conn)
        {
            await conn.ExecuteNonQueryAsync("UPDATE Tracks SET SoftDelete = 1 WHERE Album = @Album", new
            {
                Album = _name
            });
        }
    }
}
