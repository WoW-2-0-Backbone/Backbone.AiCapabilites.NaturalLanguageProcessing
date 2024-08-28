namespace Backbone.AiCapabilities.NaturalLanguageProcessing.TextCompletion.Abstractions.Services.Interfaces;

/// <summary>
/// Defines functionalities for text completion service.
/// </summary>
public interface ITextCompletionService
{
    /// <summary>
    /// Generates text content based on the given prompt.
    /// </summary>
    /// <param name="prompt">The input prompt</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>The generated output content</returns>
    ValueTask<string> GetContentAsync(string prompt, CancellationToken cancellationToken = default);
}