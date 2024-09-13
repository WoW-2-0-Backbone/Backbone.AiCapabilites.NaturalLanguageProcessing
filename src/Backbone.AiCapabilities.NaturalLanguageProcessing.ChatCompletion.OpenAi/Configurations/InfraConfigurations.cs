using Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Services.Interfaces;
using Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.OpenAi.Services;
using Backbone.AiCapabilities.SemanticKernel.Abstractions.OpenAi.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.OpenAi.Configurations;

/// <summary>
/// Provides extension methods to configure the semantic kernel.
/// </summary>
public static class InfraConfigurations
{
    /// <summary>
    /// Configures the Semantic Kernel integration using OpenAI model.
    /// </summary>
    /// <param name="kernelBuilder">The Semantic Kernel build to customize.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="configuration"></param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IKernelBuilder AddOpenAiChatCompletionServices(
        this IKernelBuilder kernelBuilder, 
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Get OpenAI settings
        var openAiSettings = new SemanticKernelOpenAiSettings();
        configuration.GetSection(nameof(SemanticKernelOpenAiSettings)).Bind(openAiSettings);

        // Add OpenAI text completion
        kernelBuilder.AddOpenAIChatCompletion(modelId: openAiSettings.ModelId, apiKey: openAiSettings.ApiKey);

        // Add text completion service
        services.AddSingleton<IChatCompletionService, OpenAiTextCompletionService>();

        return kernelBuilder;
    }
}