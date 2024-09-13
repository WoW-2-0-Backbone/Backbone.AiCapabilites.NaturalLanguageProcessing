namespace Backbone.AiCapabilities.NaturalLanguageProcessing.ChatCompletion.Abstractions.Models.ChatMessages;

/// <summary>
/// Defines chat completion message author roles.
/// </summary>
public enum ChatCompletionMessageAuthorRole
{
    /// <summary>
    /// The user role.
    /// </summary>
    User,

    /// The system role.    
    System,

    /// <summary>
    /// The assistant role.
    /// </summary>
    Assistant,
}