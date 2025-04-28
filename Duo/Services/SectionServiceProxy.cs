



public SectionServiceProxy(HttpClient httpClient)
{
    this.httpClient = httpClient;
}

public async Task<int> AddSection(Section section)
{
    var response = await httpClient.PostAsJsonAsync("api/sections/add", section);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<int>();
}

public async Task<int> CountSectionsFromRoadmap(int roadmapId)
{
    return await httpClient.GetFromJsonAsync<int>($"api/sections/count/{roadmapId}");
}

public async Task DeleteSection(int sectionId)
{
    await httpClient.DeleteAsync($"api/sections/{sectionId}");
}

public async Task<List<Section>> GetAllSections()
{
    return await httpClient.GetFromJsonAsync<List<Section>>("api/sections");
}

public async Task<List<Section>> GetByRoadmapId(int roadmapId)
{
    return await httpClient.GetFromJsonAsync<List<Section>>($"api/sections/roadmap/{roadmapId}");
}

public async Task<Section> GetSectionById(int sectionId)
{
    return await httpClient.GetFromJsonAsync<Section>($"api/sections/{sectionId}");
}

public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
{
    return await httpClient.GetFromJsonAsync<int>($"api/sections/lastordernumber/{roadmapId}");
}

public async Task UpdateSection(Section section)
{
    await httpClient.PutAsJsonAsync("api/sections/update", section);
}
}