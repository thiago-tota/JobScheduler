using System.Net.Http.Json;
using JobScheduler.Core;

namespace JobScheduler.Client.Services
{
    public class ApiService : IApi
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JobDto>> GetSchedulerHistory()
            => await GetRequest<List<JobDto>>(ApiPaths.SchedulerHistory);

        private async Task<T> GetRequest<T>(string path)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<T>(path);
                if (response != null)
                {
                    return response;
                }
                throw new NotImplementedException(); // TODO: Invalid response handling
            }
            catch
            {
                throw new NotImplementedException(); // TODO: Invalid response handling
            }
        }
    }
}
