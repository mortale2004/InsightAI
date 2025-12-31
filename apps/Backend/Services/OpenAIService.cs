using Azure.AI.OpenAI;
using OpenAI.Chat;                // chat message classes
using System;
using System.ClientModel;         // required for ApiKeyCredential
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<string> ExecutePromptAsync(List<ChatMessage> messages)
        {
            var chatClient = _client.GetChatClient(_deploymentName);
            var response = await chatClient.CompleteChatAsync(messages);

            // Get the last text content
            string output = response.Value.Content.Last().Text ?? string.Empty;
            return output;
        }

        public async IAsyncEnumerable<string> ExecutePromptStreamingAsync(List<ChatMessage> messages)
        {
            var chatClient = _client.GetChatClient(_deploymentName);
            await foreach (var update in chatClient.CompleteChatStreamingAsync(
                       messages))
            {
                foreach (ChatMessageContentPart content in update.ContentUpdate)
                {
                    if (!string.IsNullOrEmpty(content.Text))
                    {
                        yield return content.Text;
                    }
                }
            }
        }
    }
}
