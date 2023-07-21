using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Comunes
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadImageAsync(string base64Image,string ruta)
        {
            BlobClient blobClient;
            byte[] imageBytes;
            string imageName = Guid.NewGuid().ToString(); // Generar un nombre único para la imagen
            string imagePath = ruta + "/" +  $"{imageName}.jpg"; // Puedes ajustar la extensión según el formato de la imagen

            try
            { 
           
                imageBytes = Convert.FromBase64String(base64Image);
            
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
                // Comprueba si el contenedor existe, si no existe, créalo
                if (!await containerClient.ExistsAsync())
                {
                    await containerClient.CreateAsync();
                }

                blobClient = containerClient.GetBlobClient(imagePath);

            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(stream);
            }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return blobClient.Uri.ToString();
        }

        public string UploadImageAzure(string base64Image, string ruta)
        {
            BlobClient blobClient;
            byte[] imageBytes;
            string imageName = Guid.NewGuid().ToString();
            string imagePath =  ruta + "/" + $"{imageName}.jpg";

            try
            {
                imageBytes = Convert.FromBase64String(base64Image);

                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                if (!containerClient.Exists())
                {
                    containerClient.Create();
                }

                blobClient = containerClient.GetBlobClient(imagePath);

                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    blobClient.Upload(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return blobClient.Uri.ToString();
        }

    }
}
