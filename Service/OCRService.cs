using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Azure.AI.Vision.Common;
using Azure.AI.Vision.ImageAnalysis;
using Azure;

namespace OCRMultiline.Services
{
    public class OCRService
    {

        private readonly IConfiguration _configuration;

        public OCRService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<string>> RewriteOCRAsync(FlowiseAIRequestModel data)
        {
            var extractedText = await SendRequestFlowiseAIAsync(data, "https://ai.radyalabs.id/api/v1/prediction/893f3ea3-1cb5-468d-b18e-b89d793a107a");
            return RewriteOCR(extractedText);
        }

        public async Task<System.Text.Json.JsonDocument> DatatoJSONAsync(FlowiseAIRequestModel data)
        {
            var extractedText = await SendRequestFlowiseAIAsync(data, "https://ai.radyalabs.id/api/v1/prediction/8a0c6231-2505-4843-80e3-dca98e6b4103");
            return DatatoJSON(extractedText);
        }

        private async Task<string> SendRequestFlowiseAIAsync(FlowiseAIRequestModel data, string apiUrl)
        {
            var token = "NICaQUi80uFWgDFOG9kyFYsjY3ItOecOxnGznBbG7ZY=";

            // Format each string in the array as 'string1', 'string2', ...
            var formattedStrings = data.Question.Select((str, index) => $"'{str}'");

            // Join the formatted strings with a comma and space separator
            var concatenatedQuestion = string.Join(", ", formattedStrings);
            Console.WriteLine($"{{\"question\": \"{concatenatedQuestion}\"}}");
            var jsonContent = $"{{\"question\": \"{concatenatedQuestion}\"}}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        throw new Exception($"Failed to make request. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"An error occurred: {ex.Message}");
                }
            }
        }

        private List<string> RewriteOCR(string extractedText)
        {
            var jsonObject = JObject.Parse(extractedText);
            var text = jsonObject["text"].ToString();

            // Split the text by new line character (\n)
            string[] lines = text.Split('\n');

            // Convert array to list
            List<string> linesList = new List<string>(lines);
            return linesList;
        }

        private JsonDocument DatatoJSON(string extractedText)
        {
            var extractedObject = JObject.Parse(extractedText);
            var text= extractedObject["text"].ToString();
            
            text = text.Replace("\n", "");
            Console.WriteLine(text);
            // Parse the text into a new JObject
            var options = new System.Text.Json.JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };
            var textObject = JsonSerializer.Deserialize<JsonDocument>(text, options);

            // Return the entire jsonObject2 as JSON response
            return textObject;
        }

        public async Task<List<string>> ConvertImageToText(string blobUrl)
        {
            List<string> listOfData = new List<string>();

            using var imageSource = VisionSource.FromUrl(new Uri(blobUrl));

            var analysisOptions = new ImageAnalysisOptions()
            {
                Features = ImageAnalysisFeature.Caption | ImageAnalysisFeature.Text | ImageAnalysisFeature.Objects,
                Language = "en",
                GenderNeutralCaption = true
            };

            string uri = _configuration.GetValue<string>("AzureOCR:uri") ?? string.Empty;
            string key = _configuration.GetValue<string>("AzureOCR:key") ?? string.Empty;
            Console.WriteLine(uri);
            Console.WriteLine(key);
            Console.WriteLine(blobUrl);
            var option = new VisionServiceOptions(uri, new AzureKeyCredential(key));

            using var analyzer = new ImageAnalyzer(option, imageSource, analysisOptions);
            
            ImageAnalysisResult result = analyzer.Analyze();
            if (result?.Text?.Lines != null)
{
    Console.WriteLine(result.Text.Lines.Count);
    // Your other logic here
}
else
{
    Console.WriteLine("Text or Lines is null");
    // Handle the case where result.Text or result.Text.Lines is null
}
            // int mergedMinY = 0;
            // int mergedMaxY = 0;
            // bool isFirstLine = true;
            // StringBuilder mergedLineContent = new StringBuilder();

            // for (int x = 0; x < result.Text.Lines.Count; x++)
            // {
            //     DetectedTextLine detectedTextLine = result.Text.Lines[x];

            //     int currentMinY = detectedTextLine.BoundingPolygon.Min(p => p.Y);
            //     int currentMaxY = detectedTextLine.BoundingPolygon.Max(p => p.Y);

            //     if (!isFirstLine && currentMinY <= mergedMaxY)
            //     {
            //         mergedMaxY = Math.Max(mergedMaxY, currentMaxY);
            //         mergedLineContent.Append(" " + detectedTextLine.Content);
            //     }
            //     else
            //     {
            //         listOfData.Add(mergedLineContent.ToString());
            //         mergedMinY = currentMinY;
            //         mergedMaxY = currentMaxY;
            //         mergedLineContent.Clear().Append(detectedTextLine.Content);
            //         isFirstLine = false;
            //     }
            // }

            return [];
        }
    }
}
