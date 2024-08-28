using System.Text;
using Backbone.AiCapabilities.NaturalLanguageProcessing.TextCompletion.Abstractions.Services.Interfaces;
using Microsoft.SemanticKernel.TextGeneration;

namespace Backbone.AiCapabilities.NaturalLanguageProcessing.TextCompletion.OpenAi.Services;

/// <summary>
/// Provides functionalities to interact with OpenAI's Text Completion API.
/// </summary>
public class OpenAiTextCompletionService(ITextGenerationService textGenerationService) : ITextCompletionService
{
    /// <summary>
    /// Gets generated text content.
    /// </summary>
    /// <param name="prompt">The input prompt</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>The generated output content</returns>
    public async ValueTask<string> GetContentAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var multiChunkContent = await textGenerationService.GetTextContentsAsync(prompt, cancellationToken: cancellationToken);
        var stringBuilder = new StringBuilder();
        var textContent = multiChunkContent.Aggregate(stringBuilder, (sb, content) => sb.Append(content.Text));
        return textContent.ToString();
    }
}