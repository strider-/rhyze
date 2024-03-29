﻿using Rhyze.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    /// <summary>
    /// Contract for blob storage management
    /// </summary>
    public interface IBlobStore
    {
        /// <summary>
        /// Uploads a blob to the given path
        /// </summary>
        /// <param name="blobPath">The path to store the blob at</param>
        /// <param name="blob">The data to store</param>
        /// <param name="contentType">The mime type of the data</param>
        /// <param name="metadata">Any additional metadata to store with the blob</param>
        /// <returns></returns>
        Task<Error> UploadAsync(string blobPath, Stream blob, string contentType, IDictionary<string, string> metadata = null);
    }
}
