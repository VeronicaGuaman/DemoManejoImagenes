using Azure.Storage.Blobs;

namespace DemoManejoImagenes.Services
{
    public class ProductBlobConfiguration : IProductBlobConfiguration
    {
        private readonly BlobServiceClient _blobClient;

        public ProductBlobConfiguration(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];
            _blobClient = new BlobServiceClient(keys);
        }
        public BlobContainerClient GetContainer(string containerName)
        {
            BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
            return container;
        }

        public async Task<string> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            string name = file.FileName;
            BlobContainerClient container = GetContainer(containerName);
            BlobClient blobClient = container.GetBlobClient(name);
            await blobClient.UploadAsync(stream);
            return name;
        }

        public string DeleteBlob(string name, string containerName)
        {
            BlobContainerClient container = GetContainer(containerName);
            BlobClient blobClient = container.GetBlobClient(name);
            blobClient.Delete();
            return name;
        }

    }
}
