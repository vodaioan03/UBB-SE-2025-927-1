using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Services
{
    /// <summary>
    /// Provides methods to interact with the Sections API.
    /// </summary>
    public class SectionServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "http://localhost:7174";

        public SectionServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> AddSection(Section section)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}/api/sections/add", section);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error adding section: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error adding section: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<int>($"{url}/api/sections/count/{roadmapId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error counting sections: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error counting sections: {ex.Message}");
                return 0;
            }
        }

        public async Task DeleteSection(int sectionId)
        {
            try
            {
                await httpClient.DeleteAsync($"{url}/api/sections/{sectionId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error deleting section: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error deleting section: {ex.Message}");
            }
        }

        public async Task<List<Section>> GetAllSections()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Section>>($"{url}/api/sections");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error retrieving all sections: {ex.Message}");
                return new List<Section>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving all sections: {ex.Message}");
                return new List<Section>();
            }
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Section>>($"{url}/api/sections/roadmap/{roadmapId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error retrieving sections by roadmap ID: {ex.Message}");
                return new List<Section>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving sections by roadmap ID: {ex.Message}");
                return new List<Section>();
            }
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<Section>($"{url}/api/sections/{sectionId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error retrieving section by ID: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving section by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<int>($"{url}/api/sections/lastordernumber/{roadmapId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error retrieving last order number: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving last order number: {ex.Message}");
                return 0;
            }
        }

        public async Task UpdateSection(Section section)
        {
            try
            {
                await httpClient.PutAsJsonAsync($"{url}/api/sections/update", section);
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error updating section: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error updating section: {ex.Message}");
            }
        }

        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}/api/sections/completion/{sectionId}/{isCompleted}", new { });
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error tracking completion: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error tracking completion: {ex.Message}");
                return false;
            }
        }

        public async Task<List<SectionDependency>> GetSectionDependencies(int sectionId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<SectionDependency>>($"{url}/api/sections/dependencies/{sectionId}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error retrieving dependencies: {ex.Message}");
                return new List<SectionDependency>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving dependencies: {ex.Message}");
                return new List<SectionDependency>();
            }
        }
    }
}
