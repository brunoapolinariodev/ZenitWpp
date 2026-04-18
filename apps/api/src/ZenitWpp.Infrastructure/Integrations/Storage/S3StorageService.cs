using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using ZenitWpp.Application.Common.Interfaces;

namespace ZenitWpp.Infrastructure.Integrations.Storage;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;
    private readonly string _publicUrl;

    public S3StorageService(IConfiguration config)
    {
        var accessKey = config["Storage:S3:AccessKey"]!;
        var secretKey = config["Storage:S3:SecretKey"]!;
        var serviceUrl = config["Storage:S3:ServiceUrl"];
        var region = config["Storage:S3:Region"] ?? "auto";

        _bucket = config["Storage:S3:BucketName"]!;
        _publicUrl = (config["Storage:S3:PublicUrl"] ?? string.Empty).TrimEnd('/');

        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        var s3Config = new AmazonS3Config
        {
            ForcePathStyle = true,    // obrigatório para R2 e MinIO
        };

        if (!string.IsNullOrWhiteSpace(serviceUrl))
            s3Config.ServiceURL = serviceUrl;      // Cloudflare R2 endpoint
        else
            s3Config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);

        _s3 = new AmazonS3Client(credentials, s3Config);
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        var folder = DateTime.UtcNow.ToString("yyyy-MM");
        var ext = Path.GetExtension(fileName);
        var key = $"uploads/{folder}/{Guid.NewGuid():N}{ext}";

        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType,
            AutoCloseStream = false,
        };

        await _s3.PutObjectAsync(request, ct);

        return string.IsNullOrWhiteSpace(_publicUrl)
            ? $"https://{_bucket}.s3.amazonaws.com/{key}"
            : $"{_publicUrl}/{key}";
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken ct = default)
    {
        // Extrai a key a partir da URL pública
        var key = string.IsNullOrWhiteSpace(_publicUrl)
            ? new Uri(fileUrl).AbsolutePath.TrimStart('/')
            : fileUrl.Replace(_publicUrl + "/", string.Empty);

        await _s3.DeleteObjectAsync(_bucket, key, ct);
    }
}
