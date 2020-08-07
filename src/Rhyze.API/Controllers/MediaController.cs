using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rhyze.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        [HttpPost("upload/track")]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
                           MemoryBufferThreshold = int.MaxValue,
                           ValueLengthLimit = int.MaxValue)]
        public async Task<object> UploadTrackAsync()
        {
            if (!Request.HasFormContentType)
            {
                return new BadRequestResult();
            }

            var form = await Request.ReadFormAsync();

            var md5 = MD5.Create();
            var result = new Dictionary<string, string>();

            foreach (var file in form.Files)
            {
                using (var stream = file.OpenReadStream())
                {
                    var hash = string.Join("", md5.ComputeHash(stream).Select(b => b.ToString("x2")));
                    result[file.FileName] = hash;
                }
            }

            return result;
        }
    }
}