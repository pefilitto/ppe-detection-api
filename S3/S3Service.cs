using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ppe_detection_api.Common;

namespace ppe_detection_api.S3;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsSettings _awsSettings;

    public S3Service(IAmazonS3 s3Client, IOptions<AwsSettings> awsSettings)
    {
        _s3Client = s3Client;
        _awsSettings = awsSettings.Value;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string fileName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Arquivo não pode estar vazio");
        
        var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{fileName}";

        using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _awsSettings.BucketName,
            Key = $"reports/{uniqueFileName}",
            InputStream = stream,
            ContentType = file.ContentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(request);

        return GetImageUrl($"reports/{uniqueFileName}");
    }

    public string GetImageUrl(string fileName)
    {
        return $"https://{_awsSettings.BucketName}.s3.{_awsSettings.Region}.amazonaws.com/{fileName}";
    }
}