using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using OpenAI.Chat;                // chat message classes
using System.ClientModel;         // required for ApiKeyCredential

namespace Backend.Services
{
  public class OpenAIService
  {
    private readonly AzureOpenAIClient _client;
    private readonly string _deploymentName;

    public OpenAIService(IConfiguration config)
    {
      string endpoint = config["AzureOpenAI:Endpoint"]
                        ?? throw new ArgumentNullException("AzureOpenAI:Endpoint");
      string apiKey = config["AzureOpenAI:ApiKey"]
                      ?? throw new ArgumentNullException("AzureOpenAI:ApiKey");
      _deploymentName = config["AzureOpenAI:DeploymentName"]
                        ?? throw new ArgumentNullException("AzureOpenAI:DeploymentName");

      // Use System.ClientModel.ApiKeyCredential
      var credential = new ApiKeyCredential(apiKey);

      _client = new AzureOpenAIClient(new Uri(endpoint), credential);
    }

    public async Task<string> ExecutePromptAsync(string prompt)
    {
      var chatClient = _client.GetChatClient(_deploymentName);

      var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(prompt)
            };

      var response = await chatClient.CompleteChatAsync(messages);

      // Get the last text content
      string output = response.Value.Content.Last().Text ?? string.Empty;
      return output;
    }
  }
}
