namespace ZenitWpp.Infrastructure.Integrations.Translation;

public class NullTranslationService : ITranslationService
{
    public Task<string> TranslateAsync(string text, string targetLanguage, CancellationToken ct = default)
        => Task.FromResult(text);

    public Task<string> DetectLanguageAsync(string text, CancellationToken ct = default)
        => Task.FromResult("pt-BR");
}
