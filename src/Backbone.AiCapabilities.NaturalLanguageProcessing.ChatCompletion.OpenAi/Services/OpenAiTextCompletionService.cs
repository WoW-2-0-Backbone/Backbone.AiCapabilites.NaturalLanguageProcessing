using System.Collections.Concurrent;
using Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Models.ChatMessages;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using IInternalChatCompletionService =
    Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Services.Interfaces.IChatCompletionService;

namespace Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.OpenAi.Services;

/// <summary>
/// Provides functionalities to interact with OpenAI's Chat Completion API using Semantic Kernel.
/// </summary>
public class OpenAiTextCompletionService : IInternalChatCompletionService
{
    private readonly ConcurrentDictionary<Guid, ChatHistory> _chatSessions = new();
    private readonly IChatCompletionService _chatCompletionService;

    public OpenAiTextCompletionService(Kernel kernel)
    {
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    }

    /// <summary>
    /// Starts a new chat session.
    /// </summary>
    /// <param name="initialMessage">The initial message to start the chat with.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>Chat ID to distinguish from other chats.</returns>
    public ValueTask<Guid> StartNewChatAsync(string? initialMessage = default, CancellationToken cancellationToken = default)
    {
        var chatId = Guid.NewGuid();
        var chatHistory = !string.IsNullOrWhiteSpace(initialMessage) ? new ChatHistory(initialMessage) : new ChatHistory();
        _chatSessions[chatId] = chatHistory;

        return ValueTask.FromResult(chatId);
    }

    /// <summary>
    /// Sends a user message to the specified chat if provided, otherwise to a temporary chat.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="chatId">The ID of the chat to which the message will be sent.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the chat completion service.</returns>
    public async ValueTask<ChatMessage> SendMessageAsync(string message, Guid chatId = default, CancellationToken cancellationToken = default)
    {
        if (chatId == Guid.Empty)
            chatId = await StartNewChatAsync(cancellationToken: cancellationToken);

        if (!_chatSessions.TryGetValue(chatId, out var chatHistory))
            throw new ArgumentException($"Chat session with ID {chatId} not found.", nameof(chatId));

        chatHistory.AddUserMessage(message);
        var response = await _chatCompletionService.GetChatMessageContentAsync(chatHistory, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(response.Content))
            throw new InvalidOperationException("No response received from the chat completion service.");

        chatHistory.AddAssistantMessage(response.Content);
        return MapToChatMessage(response);
    }

    /// <summary>
    /// Gets the last response from the chat completion service.
    /// </summary>
    /// <param name="chatId">The ID of the chat to get the last response from.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The last response from the chat completion service.</returns>
    public ValueTask<ChatMessage?> GetLastResponseAsync(Guid chatId, CancellationToken cancellationToken = default)
    {
        if (!_chatSessions.TryGetValue(chatId, out var chatHistory))
            throw new ArgumentException($"Chat session with ID {chatId} not found.", nameof(chatId));

        var lastResponse = chatHistory.LastOrDefault(m => m.Role == AuthorRole.Assistant);
        return ValueTask.FromResult(lastResponse is not null ? MapToChatMessage(lastResponse) : null);
    }

    private static ChatMessage MapToChatMessage(ChatMessageContent content)
    {
        var role = ChatCompletionMessageAuthorRole.User;

        if (content.Role == AuthorRole.System)
            role = ChatCompletionMessageAuthorRole.System;

        if (content.Role == AuthorRole.Assistant)
            role = ChatCompletionMessageAuthorRole.Assistant;

        return new ChatMessage
        {
            AuthorRole = role,
            Content = content.Content!
        };
    }
}