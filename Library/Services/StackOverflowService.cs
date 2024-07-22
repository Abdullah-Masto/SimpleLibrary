using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimpleLibrary.Services
{
    public class StackOverflowService
    {
        private readonly HttpClient _httpClient;

        public StackOverflowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Question>> GetRecentQuestionsAsync()
        {
            var response = await _httpClient.GetAsync("https://api.stackexchange.com/2.3/questions?order=desc&sort=activity&site=stackoverflow&pagesize=50");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResult>(jsonString);

            return result.Items;
        }

        public async Task<Question> GetQuestionDetailsAsync(int questionId)
        {
            var response = await _httpClient.GetAsync($"https://api.stackexchange.com/2.3/questions/{questionId}?order=desc&sort=activity&site=stackoverflow&filter=withbody");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResult>(jsonString);

            return result.Items?.FirstOrDefault();
        }
    }

    public class ApiResult
    {
        public List<Question> Items { get; set; }
    }

    public class Question
    {
        public int question_Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
    }
}
