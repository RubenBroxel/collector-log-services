
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

public class GcpServices : IGcpServices
{
    public async Task SendToGcpBucketAsync(GcpLogFile logFile)
    {
        string bucketName = logFile.GcpBucket;
        string filePath   = logFile.FileLocalPath;
        string file       = logFile.FileName;
        string type       = logFile.FileType;
        string folder     = logFile.GcpFolder;

        // Explicitly use service account credentials by specifying the private key file.
        // The service account should have Object Manage permissions for the bucket.
        GoogleCredential credential = null;
        using (var jsonStream = new FileStream( logFile.GcpCredential, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            credential = GoogleCredential.FromStream(jsonStream);
        }
        var storageClient = StorageClient.Create(credential);

        using (var fileStream = new FileStream(filePath + "/" + file, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            //subir recurso a bucket en GCP
            //1.-Nombre del Bucket
            //2.-Ruta destino dentro del Bucket con el nombre del archivo 
            //3.-Tipo de recurso 
            //4.-Recurso
            storageClient.UploadObject(bucketName, folder + "/" + file, type, fileStream);
        }

        // Lista de objetos en ruta bucket 
        foreach (var obj in storageClient.ListObjects(bucketName, ""))
        {
            Console.WriteLine(obj.Name);
        }

        // Download file
        /*using (var fileStream = File.Create("Program-copy.cs"))
        {
            storageClient.DownloadObject(bucketName, "Program.cs", fileStream);
        }
        foreach (var obj in Directory.GetFiles("."))
        {
            Console.WriteLine(obj);
        }*/
    }

}