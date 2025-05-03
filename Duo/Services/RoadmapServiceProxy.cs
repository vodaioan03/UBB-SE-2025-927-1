using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services
{
    public class RoadmapServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174";

        public RoadmapServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Roadmap>> GetAllAsync()
        {
            var resp = await httpClient.GetAsync($"{url}/api/Roadmaps");
            resp.EnsureSuccessStatusCode();

            var list = await resp.Content.ReadFromJsonAsync<List<Roadmap>>();
            return list ?? new List<Roadmap>();
        }

        public async Task<int> AddAsync(Roadmap roadmap)
        {
            var resp = await httpClient.PostAsJsonAsync($"{url}/api/Roadmaps", roadmap);

            if (resp.IsSuccessStatusCode)
            {
                var created = await resp.Content.ReadFromJsonAsync<Roadmap>();
                return created?.Id
                    ?? throw new Exception("Add succeeded but response body was empty.");
            }

            throw new Exception($"Failed to add roadmap (HTTP {(int)resp.StatusCode}).");
        }

        public async Task DeleteAsync(int id)
        {
            var resp = await httpClient.DeleteAsync($"{url}/api/Roadmaps/{id}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"Roadmap #{id} not found.");
            }

            resp.EnsureSuccessStatusCode();
        }

        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
            var resp = await httpClient.GetAsync($"{url}/api/Roadmaps/{roadmapId}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"Roadmap #{roadmapId} not found.");
            }

            resp.EnsureSuccessStatusCode();

            var roadmap = await resp.Content.ReadFromJsonAsync<Roadmap>();
            if (roadmap == null)
            {
                throw new Exception("Failed to decode roadmap.");
            }

            return roadmap;
        }

        public async Task<Roadmap> GetByNameAsync(string roadmapName)
        {
            var resp = await httpClient.GetAsync(
                $"{url}/api/Roadmaps/search?name={Uri.EscapeDataString(roadmapName)}");

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"Roadmap named '{roadmapName}' not found.");
            }

            resp.EnsureSuccessStatusCode();

            var list = await resp.Content.ReadFromJsonAsync<List<Roadmap>>()
                       ?? throw new Exception("Failed to decode search results.");

            if (!list.Any())
            {
                throw new Exception($"Roadmap named '{roadmapName}' not found.");
            }

            return list[0];
        }
    }
}
