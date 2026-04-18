namespace ZenitWpp.Infrastructure.Integrations.Storage;

public class NullStorageService : IStorageService
{
    public Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
        => Task.FromResult($"https://storage.placeholder/{fileName}");

    public Task DeleteAsync(string fileUrl, CancellationToken ct = default)
        => Task.CompletedTask;
}
