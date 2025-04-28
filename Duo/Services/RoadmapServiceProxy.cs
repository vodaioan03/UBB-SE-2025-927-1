using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services
{
    public class RoadmapServiceProxy
    {
        private readonly HttpClient httpClient;

        public RoadmapServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Roadmap>> GetAllAsync()
        {
            var response = await httpClient.GetFromJsonAsync<List<Roadmap>>("api/Roadmaps");
            return response ?? new List<Roadmap>();
        }

        public async Task<int> AddAsync(Roadmap roadmap)
        {
            var response = await httpClient.PostAsJsonAsync("api/Roadmaps", roadmap);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Roadmap>();
                return result?.Id ?? 0;
            }
            else
            {
                throw new Exception("Failed to add roadmap");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"api/Roadmaps/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete roadmap");
            }
        }

        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
            var response = await httpClient.GetFromJsonAsync<Roadmap>($"api/Roadmaps/{roadmapId}");
            if (response == null)
            {
                throw new Exception("Roadmap not found");
            }
            return response;
        }

        public async Task<Roadmap> GetByNameAsync(string roadmapName)
        {
            var response = await httpClient.GetFromJsonAsync<List<Roadmap>>($"api/Roadmaps/search?name={roadmapName}");
            if (response == null || !response.Any())
            {
                throw new Exception("Roadmap not found");
            }
            return response.FirstOrDefault();
        }
    }
}
