using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartSummarizer
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;

        public GeminiService()
        {
            // ============================================================
            // GEMINI API KEY CONFIGURATION
            // ============================================================
            // Your API key and correct model name from Google AI Studio
            // ============================================================

            string apiKey = "AIzaSyAJyqmVkv663p64eHg70QjP_KA9i3LFxsc"; // YOUR API KEY
            string modelName = "gemini-2.5-flash"; // Correct model from listing

            // ============================================================

            _apiKey = apiKey;
            _modelName = modelName;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60); // Increased timeout for longer responses
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SmartSummarizer/1.0");
        }

        public bool IsApiKeyConfigured()
        {
            return !string.IsNullOrEmpty(_apiKey) &&
                   _apiKey.StartsWith("AIza") &&
                   _apiKey.Length >= 35;
        }

        public async Task<string> ListAvailableModels()
        {
            try
            {
                if (!IsApiKeyConfigured())
                {
                    return "ERROR: API key not configured";
                }

                string url = $"https://generativelanguage.googleapis.com/v1/models?key={_apiKey}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject obj = JObject.Parse(responseJson);
                    var models = obj["models"];

                    if (models != null)
                    {
                        string result = "Available Models:\n\n";
                        foreach (var model in models)
                        {
                            string name = model["name"]?.ToString();
                            string displayName = model["displayName"]?.ToString();
                            string description = model["description"]?.ToString();

                            result += $"• {name}\n";
                            if (!string.IsNullOrEmpty(displayName))
                                result += $"  Name: {displayName}\n";
                            if (!string.IsNullOrEmpty(description))
                                result += $"  Description: {description}\n";
                            result += "\n";
                        }
                        return result;
                    }
                    return responseJson;
                }
                else
                {
                    return $"Error: {response.StatusCode} - {responseJson}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> TestApiKeyAsync()
        {
            try
            {
                if (!IsApiKeyConfigured())
                {
                    return "ERROR: API key not configured or invalid format.\n\n" +
                           "Key should start with 'AIza' and be about 39 characters long.\n\n" +
                           "Get your free key at: https://makersuite.google.com/app/apikey";
                }

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = "Say 'API is working!' in one sentence."
                                }
                            }
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = $"https://generativelanguage.googleapis.com/v1/models/{_modelName}:generateContent?key={_apiKey}";

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject obj = JObject.Parse(responseJson);
                    var candidates = obj["candidates"];

                    if (candidates != null && candidates.HasValues)
                    {
                        var contentObj = candidates[0]?["content"];
                        var parts = contentObj?["parts"];

                        if (parts != null && parts.HasValues)
                        {
                            string result = parts[0]?["text"]?.ToString();
                            return $"SUCCESS: API is working with model {_modelName}!\n\nResponse: {result}";
                        }
                    }

                    return $"SUCCESS: API key is valid and working with model {_modelName}!";
                }
                else
                {
                    return $"ERROR: {response.StatusCode}\n\nResponse: {responseJson}";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        public async Task<string> GetSummaryAsync(string textToSummarize)
        {
            try
            {
                if (!IsApiKeyConfigured())
                {
                    return "Error: API key not configured. Please add your Gemini API key in GeminiService.cs";
                }

                if (string.IsNullOrWhiteSpace(textToSummarize))
                {
                    return "Error: No text provided for summarization.";
                }

                if (textToSummarize.Length > 1000000)
                {
                    textToSummarize = textToSummarize.Substring(0, 1000000) + "...";
                }

                // Improved prompt for better summaries
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $@"Please provide a comprehensive yet concise summary of the following text. Follow these guidelines:
1. Write a summary that is 3-5 sentences long
2. Capture the main ideas and key points
3. Use clear, natural language
4. Include important details and context
5. Make it informative and well-structured

Text to summarize:
{textToSummarize}

Summary:"
                                }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,  // Slightly higher for more creative summaries
                        maxOutputTokens = 500,  // Increased for longer summaries
                        topP = 0.95,
                        topK = 40
                    }
                };

                string json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = $"https://generativelanguage.googleapis.com/v1/models/{_modelName}:generateContent?key={_apiKey}";

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject obj = JObject.Parse(responseJson);
                    var candidates = obj["candidates"];

                    if (candidates != null && candidates.HasValues)
                    {
                        var contentObj = candidates[0]?["content"];
                        var parts = contentObj?["parts"];

                        if (parts != null && parts.HasValues)
                        {
                            string summary = parts[0]?["text"]?.ToString();
                            return summary?.Trim() ?? "No summary generated.";
                        }
                    }
                    return "Could not extract summary from API response.";
                }
                else
                {
                    return $"Error: {response.StatusCode} - {responseJson}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}