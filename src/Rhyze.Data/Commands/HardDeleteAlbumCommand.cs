using RepoDb;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class HardDeleteAlbumCommand : ICommandAsync
    {
        // Passing an array of tracks is a deliberate design choice;
        // A user could delete an album & immediately start to re-upload it,
        // and deleting by album name could have adverse effects in that scenario.

        private readonly IEnumerable<Track> _tracks;

        public HardDeleteAlbumCommand(IEnumerable<Track> tracks) => _tracks = tracks;

        public async Task ExecuteAsync(IDbConnection conn)
        {
            var ids = _tracks.Select(t => t.Id).Cast<object>();

            await conn.DeleteAllAsync<Track>(primaryKeys: ids);
        }
    }
}