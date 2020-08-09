﻿using RepoDb;
using RepoDb.Enumerations;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public class GetAlbumByNameQuery : IQueryAsync<IEnumerable<Track>>
    {
        private readonly string _name;

        public GetAlbumByNameQuery(string name) => _name = name;

        public async Task<IEnumerable<Track>> ExecuteAsync(IDbConnection conn)
        {
            var order = OrderField.Parse(new
            {
                DiscNo = Order.Ascending,
                TrackNo = Order.Ascending
            });

            return await conn.QueryAsync<Track>(t => t.Album == _name, orderBy: order);
        }
    }
}