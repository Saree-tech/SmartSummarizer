using System;
using System.IO;
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
        private readonly int _timeoutSeconds;
        private readonly int _maxOutputTokens;
        private readonly double _temperature;

        public GeminiService()
        {
            // Load configuration from multiple sources
            var config = LoadConfiguration();

            _apiKey = config.ApiKey;
            _modelName = config.ModelName;
            _timeoutSeconds = config.TimeoutSeconds;
            _maxOutputTokens = config.MaxOutputTokens;
            _temperature = config.Temperature;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);
        }

        private Configuration LoadConfiguration()
        {
            var config = new Configuration();
            string configPath = "appsettings.json";

            // Try to load from appsettings.json
            if (File.Exists(configPath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(configPath);
                    var jsonConfig = JObject.Parse(jsonContent);

                    config.ApiKey = jsonConfig["Gemini"]?["ApiKey"]?.ToString();
                    config.ModelName = jsonConfig["Gemini"]?["ModelName"]?.ToString() ?? "gemini-2.5-flash";
                    config.TimeoutSeconds = int.Parse(jsonConfig["Gemini"]?["TimeoutSeconds"]?.ToString() ?? "60");
                    config.MaxOutputTokens = int.Parse(jsonConfig["Gemini"]?["MaxOutputTokens"]?.ToString() ?? "500");
                    config.Temperature = double.Parse(jsonConfig["Gemini"]?["Temperature"]?.ToString() ?? "0.7");
                    config.UserAgent = jsonConfig["Application"]?["UserAgent"]?.ToString() ?? "SmartSummarizer/1.0";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading config: {ex.Message}");
                }
            }

            // Environment variable takes precedence (for CI/CD and production)
            string envApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (!string.IsNullOrEmpty(envApiKey))
            {
                config.ApiKey = envApiKey;
            }

            string envModelName = Environment.GetEnvironmentVariable("GEMINI_MODEL_NAME");
            if (!string.IsNullOrEmpty(envModelName))
            {
                config.ModelName = envModelName;
            }

            return config;
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
                    return GetApiKeyErrorMessage();
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
                    return GetApiKeyErrorMessage();
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
                    return GetApiKeyErrorMessage();
                }

                if (string.IsNullOrWhiteSpace(textToSummarize))
                {
                    return "Error: No text provided for summarization.";
                }

                if (textToSummarize.Length > 1000000)
                {
                    textToSummarize = textToSummarize.Substring(0, 1000000) + "...";
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
                        temperature = _temperature,
                        maxOutputTokens = _maxOutputTokens,
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

        private string GetApiKeyErrorMessage()
        {
            return "ERROR: API key not configured.\n\n" +
                   "To use SmartSummarizer, you need a Google Gemini API key.\n\n" +
                   "Setup Instructions:\n" +
                   "1. Get your free API key at: https://makersuite.google.com/app/apikey\n" +
                   "2. Create a file named 'appsettings.json' in the application folder\n" +
                   "3. Add the following configuration:\n\n" +
                   "{\n" +
                   "  \"Gemini\": {\n" +
                   "    \"ApiKey\": \"your-api-key-here\"\n" +
                   "  }\n" +
                   "}\n\n" +
                   "Or set the environment variable: GEMINI_API_KEY\n\n" +
                   "See README.md for detailed setup instructions.";
        }

        private class Configuration
        {
            public string ApiKey { get; set; }
            public string ModelName { get; set; } = "gemini-2.5-flash";
            public int TimeoutSeconds { get; set; } = 60;
            public int MaxOutputTokens { get; set; } = 500;
            public double Temperature { get; set; } = 0.7;
            public string UserAgent { get; set; } = "SmartSummarizer/1.0";
        }
    }
}