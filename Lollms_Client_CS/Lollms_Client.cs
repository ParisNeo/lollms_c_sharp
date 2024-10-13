using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;

namespace Lollms_Client_CS
{
    // Enums
    public enum ELF_GENERATION_FORMAT
    {
        LOLLMS = 0,
        OPENAI = 1,
        OLLAMA = 2,
        LITELLM = 2
    }

    public enum ELF_COMPLETION_FORMAT
    {
        Instruct = 0,
        Chat = 1
    }

    public class TemplateClass
    {
        [JsonPropertyName("start_header_id_template")]
        public string StartHeaderIdTemplate { get; set; } = "!@>";
        [JsonPropertyName("end_header_id_template")]
        public string EndHeaderIdTemplate { get; set; } = ": ";
        [JsonPropertyName("separator_template")]
        public string SeparatorTemplate { get; set; } = "\n";
        [JsonPropertyName("start_user_header_id_template")]
        public string StartUserHeaderIdTemplate { get; set; } = "!@>";
        [JsonPropertyName("end_user_header_id_template")]
        public string EndUserHeaderIdTemplate { get; set; } = ": ";
        [JsonPropertyName("end_user_message_id_template")]
        public string EndUserMessageIdTemplate { get; set; } = "";
        [JsonPropertyName("start_ai_header_id_template")]
        public string StartAiHeaderIdTemplate { get; set; } = "!@>";
        [JsonPropertyName("end_ai_header_id_template")]
        public string EndAiHeaderIdTemplate { get; set; } = ": ";
        [JsonPropertyName("end_ai_message_id_template")]
        public string EndAiMessageIdTemplate { get; set; } = "";
        [JsonPropertyName("system_message_template")]
        public string SystemMessageTemplate { get; set; } = "system";
    }
    public class LollmsDetokenizeResponse
    {
        public string Text { get; set; }
    }

    public class LollmsTokenizeResponse
    {
        public List<string> NamedTokens { get; set; }
    }
    public class LollmsClient
    {
        // Properties
        public string HostAddress { get; set; }
        public string ModelName { get; set; }
        public int CtxSize { get; set; }
        public int N_Predict { get; set; }
        public int Personality { get; set; }
        public double Temperature { get; set; }
        public int TopK { get; set; }
        public double TopP { get; set; }
        public double RepeatPenalty { get; set; }
        public int RepeatLastN { get; set; }
        public int? Seed { get; set; }
        public int NThreads { get; set; }
        public string ServiceKey { get; set; }
        public ELF_GENERATION_FORMAT DefaultGenerationMode { get; set; }
        public int MinNPredict { get; set; } = 10;
        public TemplateClass Template { get; set; }

        // HttpClient instance
        private readonly HttpClient _httpClient;

        // Constructor
        public LollmsClient(
            string hostAddress = "http://localhost:9600",
            string modelName = null,
            int ctxSize = 4096,
            int personality = -1,
            int? nPredict = 4096,
            double temperature = 0.1,
            int topK = 50,
            double topP = 0.95,
            double repeatPenalty = 0.8,
            int repeatLastN = 40,
            int? seed = null,
            int nThreads = 8,
            string serviceKey = "",
            ELF_GENERATION_FORMAT defaultGenerationMode = ELF_GENERATION_FORMAT.LOLLMS
        )
        {
            // Initialize properties
            this.HostAddress = hostAddress;
            this.ModelName = modelName;
            this.CtxSize = ctxSize;
            this.Personality = personality;
            this.N_Predict = nPredict ?? 4096;
            this.Temperature = temperature;
            this.TopK = topK;
            this.TopP = topP;
            this.RepeatPenalty = repeatPenalty;
            this.RepeatLastN = repeatLastN;
            this.Seed = seed;
            this.NThreads = nThreads;
            this.ServiceKey = serviceKey;
            this.DefaultGenerationMode = defaultGenerationMode;
            this.MinNPredict = 10;
            this.Template = new TemplateClass();
            // Initialize HttpClient
            _httpClient = new HttpClient();
        }

        public async Task LoadTemplateAsync()
        {
            string url = "/template";
            if (!string.IsNullOrEmpty(this.HostAddress))
            {
                url = this.HostAddress.TrimEnd('/') + "/template";
            }
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var templateData = JsonSerializer.Deserialize<TemplateClass>(responseBody);
                this.Template = templateData;
                Console.WriteLine("Template loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching template: " + ex.Message);
            }
        }

        public void UpdateSettings(Dictionary<string, object> settings)
        {
            if (settings.ContainsKey("host_address")) this.HostAddress = settings["host_address"].ToString();
            if (settings.ContainsKey("model_name")) this.ModelName = settings["model_name"].ToString();
            if (settings.ContainsKey("ctx_size")) this.CtxSize = Convert.ToInt32(settings["ctx_size"]);
            if (settings.ContainsKey("n_predict")) this.N_Predict = Convert.ToInt32(settings["n_predict"]);
            if (settings.ContainsKey("personality")) this.Personality = Convert.ToInt32(settings["personality"]);
            if (settings.ContainsKey("temperature")) this.Temperature = Convert.ToDouble(settings["temperature"]);
            if (settings.ContainsKey("top_k")) this.TopK = Convert.ToInt32(settings["top_k"]);
            if (settings.ContainsKey("top_p")) this.TopP = Convert.ToDouble(settings["top_p"]);
            if (settings.ContainsKey("repeat_penalty")) this.RepeatPenalty = Convert.ToDouble(settings["repeat_penalty"]);
            if (settings.ContainsKey("repeat_last_n")) this.RepeatLastN = Convert.ToInt32(settings["repeat_last_n"]);
            if (settings.ContainsKey("seed")) this.Seed = Convert.ToInt32(settings["seed"]);
            if (settings.ContainsKey("n_threads")) this.NThreads = Convert.ToInt32(settings["n_threads"]);
            if (settings.ContainsKey("service_key")) this.ServiceKey = settings["service_key"].ToString();
            if (settings.ContainsKey("default_generation_mode")) this.DefaultGenerationMode = (ELF_GENERATION_FORMAT)Enum.Parse(typeof(ELF_GENERATION_FORMAT), settings["default_generation_mode"].ToString());
            Console.WriteLine("Settings updated:", settings);
        }

        public string SeparatorTemplate()
        {
            return this.Template.SeparatorTemplate;
        }

        public string SystemMessage()
        {
            return this.Template.StartHeaderIdTemplate + this.Template.SystemMessageTemplate + this.Template.EndHeaderIdTemplate;
        }

        public string AiMessage(string aiName = "assistant")
        {
            return this.Template.StartAiHeaderIdTemplate + aiName + this.Template.EndAiHeaderIdTemplate;
        }

        public string UserMessage(string userName = "user")
        {
            return this.Template.StartUserHeaderIdTemplate + userName + this.Template.EndUserHeaderIdTemplate;
        }

        public string CustomMessage(string messageName = "message")
        {
            return this.Template.StartAiHeaderIdTemplate + messageName + this.Template.EndAiHeaderIdTemplate;
        }

        public void UpdateServerAddress(string newAddress)
        {
            this.HostAddress = newAddress;
        }

        public async Task<List<string>> TokenizeAsync(string prompt)
        {
            string url = "/lollms_tokenize";
            if (!string.IsNullOrEmpty(this.HostAddress))
            {
                url = this.HostAddress.TrimEnd('/') + "/lollms_tokenize";
            }

            var requestData = new { prompt = prompt };
            var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<LollmsTokenizeResponse>(responseBody);
                return responseData.NamedTokens;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during tokenize: " + ex.Message);
                return null;
            }
        }

        public async Task<string> DetokenizeAsync(List<string> tokensList)
        {
            string url = "/lollms_detokenize";
            if (!string.IsNullOrEmpty(this.HostAddress))
            {
                url = this.HostAddress.TrimEnd('/') + "/lollms_detokenize";
            }

            var requestData = new { tokens = tokensList };
            var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<LollmsDetokenizeResponse>(responseBody);
                Console.WriteLine(responseData.Text);
                return responseData.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during detokenize: " + ex.Message);
                return null;
            }
        }

        public async Task<string> Generate(
            string prompt,
            int? nPredict = null,
            bool stream = false,
            double? temperature = null,
            int? topK = null,
            double? topP = null,
            double? repeatPenalty = null,
            int? repeatLastN = null,
            int? seed = null,
            int? nThreads = null,
            string serviceKey = "",
            Func<string, string, bool> streamingCallback = null
        )
        {
            switch (this.DefaultGenerationMode)
            {
                case ELF_GENERATION_FORMAT.LOLLMS:
                    return await LollmsGenerate(prompt, this.HostAddress, this.ModelName, this.Personality, nPredict, stream, temperature, topK, topP, repeatPenalty, repeatLastN, seed, nThreads, serviceKey, streamingCallback);
                case ELF_GENERATION_FORMAT.OPENAI:
                    return await OpenAIGenerate(prompt, this.HostAddress, this.ModelName, this.Personality, nPredict, stream, temperature, topK, topP, repeatPenalty, repeatLastN, seed, nThreads, ELF_COMPLETION_FORMAT.Instruct, serviceKey, streamingCallback);
                // Implement other cases if needed
                default:
                    throw new ArgumentException("Invalid generation mode");
            }
        }

        public async Task<string> LollmsGenerate(
            string prompt,
            string hostAddress = null,
            string modelName = null,
            int personality = -1,
            int? nPredict = null,
            bool stream = false,
            double? temperature = null,
            int? topK = null,
            double? topP = null,
            double? repeatPenalty = null,
            int? repeatLastN = null,
            int? seed = null,
            int? nThreads = null,
            string serviceKey = "",
            Func<string, string, bool> streamingCallback = null
        )
        {
            string url = "/lollms_generate";
            if (!string.IsNullOrEmpty(hostAddress))
            {
                url = hostAddress.TrimEnd('/') + "/lollms_generate";
            }

            var requestData = new
            {
                prompt = prompt,
                model_name = modelName,
                personality = personality,
                n_predict = nPredict ?? this.N_Predict,
                stream = stream,
                temperature = temperature ?? this.Temperature,
                top_k = topK ?? this.TopK,
                top_p = topP ?? this.TopP,
                repeat_penalty = repeatPenalty ?? this.RepeatPenalty,
                repeat_last_n = repeatLastN ?? this.RepeatLastN,
                seed = seed ?? this.Seed,
                n_threads = nThreads ?? this.NThreads
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(serviceKey))
            {
                requestContent.Headers.Add("Authorization", $"Bearer {serviceKey}");
            }

            try
            {
                var response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during LOLLMS generate: " + ex.Message);
                return null;
            }
        }

        public async Task<string> OpenAIGenerate(
            string prompt,
            string hostAddress = null,
            string modelName = null,
            int personality = -1,
            int? nPredict = null,
            bool stream = false,
            double? temperature = null,
            int? topK = null,
            double? topP = null,
            double? repeatPenalty = null,
            int? repeatLastN = null,
            int? seed = null,
            int? nThreads = null,
            ELF_COMPLETION_FORMAT completionFormat = ELF_COMPLETION_FORMAT.Instruct,
            string serviceKey = "",
            Func<string, string, bool> streamingCallback = null
        )
        {
            string url = "/generate_completion";
            if (!string.IsNullOrEmpty(hostAddress))
            {
                url = hostAddress.TrimEnd('/') + "/generate_completion";
            }

            var requestData = new
            {
                prompt = prompt,
                model_name = modelName,
                personality = personality,
                n_predict = nPredict ?? this.N_Predict,
                stream = stream,
                temperature = temperature ?? this.Temperature,
                top_k = topK ?? this.TopK,
                top_p = topP ?? this.TopP,
                repeat_penalty = repeatPenalty ?? this.RepeatPenalty,
                repeat_last_n = repeatLastN ?? this.RepeatLastN,
                seed = seed ?? this.Seed,
                n_threads = nThreads ?? this.NThreads,
                completion_format = completionFormat.ToString()
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(serviceKey))
            {
                requestContent.Headers.Add("Authorization", $"Bearer {serviceKey}");
            }

            try
            {
                var response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during OpenAI generate: " + ex.Message);
                return null;
            }
        }

        // Implement other methods similarly...

        public async Task<List<string>> ListMountedPersonalities(string hostAddress = null)
        {
            string url = "/list_mounted_personalities";
            if (!string.IsNullOrEmpty(hostAddress))
            {
                url = hostAddress.TrimEnd('/') + "/list_mounted_personalities";
            }

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var personalities = JsonSerializer.Deserialize<List<string>>(responseBody);
                return personalities;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error listing mounted personalities: " + ex.Message);
                return null;
            }
        }

        public async Task<List<string>> ListModels(string hostAddress = null)
        {
            string url = "/list_models";
            if (!string.IsNullOrEmpty(hostAddress))
            {
                url = hostAddress.TrimEnd('/') + "/list_models";
            }

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var models = JsonSerializer.Deserialize<List<string>>(responseBody);
                return models;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error listing models: " + ex.Message);
                return null;
            }
        }

        public string EncodeImage(string imagePath, int maxImageWidth = -1)
        {
            try
            {
                using var image = System.Drawing.Image.FromFile(imagePath);

                int width = image.Width;
                int height = image.Height;

                // Resize if necessary
                if (maxImageWidth != -1 && width > maxImageWidth)
                {
                    double ratio = (double)maxImageWidth / width;
                    width = maxImageWidth;
                    height = (int)(height * ratio);
                    // Resize the image
                    var resizedImage = new Bitmap(image, new System.Drawing.Size(width, height));
                    using var ms = new MemoryStream();
                    resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return Convert.ToBase64String(ms.ToArray());
                }
                else
                {
                    using var ms = new MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encoding image: " + ex.Message);
                return null;
            }
        }
    }
}