using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    internal class ImageController
    {
        public static bool CargarImagen(System.IO.Stream path, string nuevoNombre)
        {
            IAmazonS3 client = new AmazonS3Client(awsAccessKeyId: "AKIAWPD4JNNA3OSYPE24", awsSecretAccessKey: "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", region:RegionEndpoint.SAEast1);
            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest();
            uploadRequest.BucketName = "fotosdpto";
            uploadRequest.Key = nuevoNombre;
            uploadRequest.InputStream = path;
            utility.Upload(uploadRequest);
            return true;
        }
    }
}
