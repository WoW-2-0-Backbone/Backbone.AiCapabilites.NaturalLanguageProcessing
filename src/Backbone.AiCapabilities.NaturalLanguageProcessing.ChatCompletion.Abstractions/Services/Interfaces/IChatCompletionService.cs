using Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Models.ChatMessages;

namespace Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Services.Interfaces;

/// <summary>
/// Defines functionalities for chat completion service.
/// </summary>
public interface IChatCompletionService
{
    /// <summary>
    /// Starts a new chat session.
    /// </summary>
    /// <param name="initialMessage">The initial message to start the chat with.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>Chat ID to distinguish from other chats.</returns>
    ValueTask<Guid> StartNewChatAsync(string? initialMessage = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a user message to the specified chat if provided, otherwise/// <param name="chatId">The ID of the chat to get the last response from.</param> to a temporary chat.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="chatId">The ID of the chat to which the message will be sent.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the chat completion service.</returns>
    ValueTask<ChatMessage> SendMessageAsync(string message, Guid chatId = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the last response from the chat completion service.
    /// </summary>
    /// <param name="chatId">The ID of the chat to get the last response from.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The last response from the chat completion service.</returns>
    ValueTask<ChatMessage?> GetLastResponseAsync(Guid chatId, CancellationToken cancellationToken = default);
}