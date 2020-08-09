using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class CreateTrackCommand : ICommandAsync
    {
        private readonly Track _track;

        public CreateTrackCommand(Track track) => _track = track;

        public async Task ExecuteAsync(IDbConnection conn)
        {
            _track.Id = _track.Id != Guid.Empty
                ? _track.Id
                : Guid.NewGuid();

            await conn.InsertAsync(_track);
        }
    }
}