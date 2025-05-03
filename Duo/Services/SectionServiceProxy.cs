using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Exceptions;
using Duo.Models.Sections;

public class SectionServiceProxy
{
    private readonly HttpClient httpClient;

    public SectionServiceProxy(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<int> AddSection(Section section)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/sections/add", section);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException("Failed to add section via proxy.", ex);
        }
    }

    public async Task<int> CountSectionsFromRoadmap(int roadmapId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<int>($"api/sections/count/{roadmapId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to count sections for roadmap ID {roadmapId}.", ex);
        }
    }

    public async Task DeleteSection(int sectionId)
    {
        try
        {
            await httpClient.DeleteAsync($"api/sections/{sectionId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to delete section with ID {sectionId}.", ex);
        }
    }

    public async Task<List<Section>> GetAllSections()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<Section>>("api/sections");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException("Failed to retrieve all sections.", ex);
        }
    }

    public async Task<List<Section>> GetByRoadmapId(int roadmapId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<Section>>($"api/sections/roadmap/{roadmapId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to retrieve sections for roadmap ID {roadmapId}.", ex);
        }
    }

    public async Task<Section> GetSectionById(int sectionId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Section>($"api/sections/{sectionId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to retrieve section with ID {sectionId}.", ex);
        }
    }

    public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<int>($"api/sections/lastordernumber/{roadmapId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to get last order number for roadmap ID {roadmapId}.", ex);
        }
    }

    public async Task UpdateSection(Section section)
    {
        try
        {
            await httpClient.PutAsJsonAsync("api/sections/update", section);
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to update section with ID {section.Id}.", ex);
        }
    }

    public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/sections/completion/{sectionId}/{isCompleted}", new { });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to track completion for section ID {sectionId}.", ex);
        }
    }

    public async Task<List<SectionDependency>> GetSectionDependencies(int sectionId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<List<SectionDependency>>($"api/sections/dependencies/{sectionId}");
        }
        catch (Exception ex)
        {
            throw new SectionServiceProxyException($"Failed to get dependencies for section ID {sectionId}.", ex);
        }
    }
}
