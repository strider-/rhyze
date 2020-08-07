using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    public interface IUploadService
    {
        Task UploadTrackAsync();

        Task UploadArtworkAsync();
    }
}
