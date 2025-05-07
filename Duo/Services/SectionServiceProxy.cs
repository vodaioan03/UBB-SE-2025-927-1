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
            var response = await this.httpClient.PostAsJsonAsync(
                    $"{this.url}/api/sections/add",
                    section).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>().ConfigureAwait(false);
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            return await this.httpClient
                    .GetFromJsonAsync<int>($"{this.url}/api/sections/count/{roadmapId}")
                    .ConfigureAwait(false);
        }

        public async Task DeleteSection(int sectionId)
        {
            await this.httpClient
                    .DeleteAsync($"{this.url}/api/sections/{sectionId}")
                    .ConfigureAwait(false);
        }

        public async Task<List<Section>> GetAllSections()
        {
            return await this.httpClient
                    .GetFromJsonAsync<List<Section>>($"{url}/api/sections")
                    .ConfigureAwait(false);
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            return await this.httpClient
                    .GetFromJsonAsync<List<Section>>(
                        $"{this.url}/api/sections/roadmap/{roadmapId}")
                    .ConfigureAwait(false);
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            return await this.httpClient
                    .GetFromJsonAsync<Section>($"{this.url}/api/sections/{sectionId}")
                    .ConfigureAwait(false);
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            return await this.httpClient
                    .GetFromJsonAsync<int>(
                        $"{this.url}/api/sections/lastordernumber/{roadmapId}")
                    .ConfigureAwait(false);
        }

        public async Task UpdateSection(Section section)
        {
            await this.httpClient
                    .PutAsJsonAsync($"{this.url}/api/sections/update", section)
                    .ConfigureAwait(false);
        }

        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            var response = await this.httpClient
                    .PostAsJsonAsync(
                        $"{this.url}/api/sections/completion/{sectionId}/{isCompleted}",
                        new { })
                    .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>().ConfigureAwait(false);
        }

        public async Task<List<SectionDependency>> GetSectionDependencies(int sectionId)
        {
            return await this.httpClient
                    .GetFromJsonAsync<List<SectionDependency>>(
                        $"{this.url}/api/sections/dependencies/{sectionId}")
                    .ConfigureAwait(false);
        }
    }
}
