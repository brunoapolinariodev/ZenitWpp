namespace ZenitWpp.Infrastructure.Integrations.Translation;

public interface ITranslationService
{
    Task<string> TranslateAsync(string text, string targetLanguage, CancellationToken ct = default);
    Task<string> DetectLanguageAsync(string text, CancellationToken ct = default);
}
