using Microsoft.Extensions.Configuration;
using ZenitWpp.Application.Common.Interfaces;

namespace ZenitWpp.Infrastructure.Integrations.Storage;

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public LocalStorageService(IConfiguration config)
    {
        _basePath = config["Storage:Local:BasePath"]
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _baseUrl = (config["Storage:Local:BaseUrl"] ?? "http://localhost:5000").TrimEnd('/');
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        var folder = DateTime.UtcNow.ToString("yyyy-MM");
        var ext = Path.GetExtension(fileName);
        var key = $"{folder}/{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(_basePath, folder, $"{Guid.NewGuid():N}{ext}");

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await content.CopyToAsync(fs, ct);

        return $"{_baseUrl}/uploads/{key}";
    }

    public Task DeleteAsync(string fileUrl, CancellationToken ct = default)
    {
        // Extrai o caminho relativo a partir da URL: /uploads/{folder}/{file}
        var uri = new Uri(fileUrl);
        var relativePath = uri.AbsolutePath.TrimStart('/'); // "uploads/yyyy-MM/filename"
        var fullPath = Path.Combine(
            Path.GetDirectoryName(_basePath)!, // wwwroot
            relativePath
        );

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
