using RepoDb;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class SoftDeleteAlbumCommand : ICommandAsync
    {
        public SoftDeleteAlbumCommand(string name) => AlbumName = name;

        public async Task ExecuteAsync(IDbConnection conn)
        {
            await conn.ExecuteNonQueryAsync("UPDATE Tracks SET SoftDelete = 1 WHERE Album = @Album", new
            {
                Album = AlbumName
            });
        }

        public string AlbumName { get; }
    }
}