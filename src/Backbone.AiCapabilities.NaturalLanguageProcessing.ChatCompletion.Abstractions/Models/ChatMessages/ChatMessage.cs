namespace Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Models.ChatMessages;

/// <summary>
/// Represents a message in a chat completion conversation.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Gets or sets the role of the message sender.
    /// </summary>
    public ChatCompletionMessageAuthorRole AuthorRole { get; set; }

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public string Content { get; set; } = default!;
}