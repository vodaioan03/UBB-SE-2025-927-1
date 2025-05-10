﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class RoadmapServiceProxy : IRoadmapService, IRoadmapServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174/";

        public RoadmapServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Roadmap>> GetAllAsync()
        {
            var response = await httpClient.GetFromJsonAsync<List<Roadmap>>($"{url}api/Roadmaps");
            return response ?? new List<Roadmap>();
        }

        public async Task<int> AddAsync(Roadmap roadmap)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}api/Roadmaps", roadmap);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Roadmap>();
            return result?.Id ?? 0;
        }

        public async Task DeleteAsync(Roadmap roadmap)
        {
            var id = roadmap.Id;
            var response = await httpClient.DeleteAsync($"{url}api/Roadmaps/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
            var response = await httpClient.GetFromJsonAsync<Roadmap>($"{url}api/Roadmaps/{roadmapId}");
            if (response == null)
            {
                throw new Exception("Roadmap not found");
            }
            return response;
        }

        public async Task<Roadmap> GetByNameAsync(string roadmapName)
        {
            var response = await httpClient.GetFromJsonAsync<List<Roadmap>>($"{url}api/Roadmaps/search?name={roadmapName}");
            if (response == null || !response.Any())
            {
                throw new Exception("Roadmap not found");
            }
            return response.FirstOrDefault();
        }
    }
}
