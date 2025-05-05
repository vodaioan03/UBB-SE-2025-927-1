using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models.Sections;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    /// <summary>
    /// Provides methods to interact with the Sections API.
    /// </summary>
    /// <remarks>
    /// Implements <see cref="ISectionServiceProxy"/> so that callers
    /// can depend on the interface and tests can inject mocks.
    /// </remarks>
    public class SectionServiceProxy : ISectionServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "http://localhost:7174";

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionServiceProxy"/> class.
        /// </summary>
        /// <param name="httpClient">HTTP client used to call the backend API.</param>
        public SectionServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<int> AddSection(Section section)
        {
            try
            {
                var response = await this.httpClient.PostAsJsonAsync(
                    $"{this.url}/api/sections/add",
                    section).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>().ConfigureAwait(false);
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
                return await this.httpClient
                    .GetFromJsonAsync<int>($"{this.url}/api/sections/count/{roadmapId}")
                    .ConfigureAwait(false);
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
                await this.httpClient
                    .DeleteAsync($"{this.url}/api/sections/{sectionId}")
                    .ConfigureAwait(false);
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
                return await this.httpClient
                    .GetFromJsonAsync<List<Section>>($"{this.url}/api/sections")
                    .ConfigureAwait(false);
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
                return await this.httpClient
                    .GetFromJsonAsync<List<Section>>(
                        $"{this.url}/api/sections/roadmap/{roadmapId}")
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(
                    $"HTTP error retrieving sections by roadmap ID: {ex.Message}");
                return new List<Section>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"Unexpected error retrieving sections by roadmap ID: {ex.Message}");
                return new List<Section>();
            }
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            try
            {
                return await this.httpClient
                    .GetFromJsonAsync<Section>($"{this.url}/api/sections/{sectionId}")
                    .ConfigureAwait(false);
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
                return await this.httpClient
                    .GetFromJsonAsync<int>(
                        $"{this.url}/api/sections/lastordernumber/{roadmapId}")
                    .ConfigureAwait(false);
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
                await this.httpClient
                    .PutAsJsonAsync($"{this.url}/api/sections/update", section)
                    .ConfigureAwait(false);
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
                var response = await this.httpClient
                    .PostAsJsonAsync(
                        $"{this.url}/api/sections/completion/{sectionId}/{isCompleted}",
                        new { })
                    .ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>().ConfigureAwait(false);
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
                return await this.httpClient
                    .GetFromJsonAsync<List<SectionDependency>>(
                        $"{this.url}/api/sections/dependencies/{sectionId}")
                    .ConfigureAwait(false);
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
