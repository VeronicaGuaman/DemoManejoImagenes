using Azure.Storage.Blobs;

namespace DemoManejoImagenes.Services
{
    public interface IProductBlobConfiguration
    {
        string DeleteBlob(string name, string containerName);
        BlobContainerClient GetContainer(string containerName);
        Task<string> UploadBlobAsync(IFormFile file, string containerName);
    }
}
