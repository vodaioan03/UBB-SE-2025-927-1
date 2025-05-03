using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Services
{
    /// <summary>
    /// Provides methods to interact with sections, delegating to the SectionServiceProxy.
    /// </summary>
    public class SectionService : ISectionService
    {
        private readonly SectionServiceProxy sectionServiceProxy;

        public SectionService(SectionServiceProxy sectionServiceProxy)
        {
            this.sectionServiceProxy = sectionServiceProxy;
        }

        public async Task<int> AddSection(Section section)
        {
            try
            {
                ValidationHelper.ValidateSection(section);
                var allSections = await GetAllSections();
                section.OrderNumber = allSections.Count + 1;
                return await sectionServiceProxy.AddSection(section);
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
                return await sectionServiceProxy.CountSectionsFromRoadmap(roadmapId);
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
                await sectionServiceProxy.DeleteSection(sectionId);
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
                return await sectionServiceProxy.GetAllSections();
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
                return await sectionServiceProxy.GetByRoadmapId(roadmapId);
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
                return await sectionServiceProxy.GetSectionById(sectionId);
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
                return await sectionServiceProxy.LastOrderNumberFromRoadmap(roadmapId);
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
                ValidationHelper.ValidateSection(section);
                await sectionServiceProxy.UpdateSection(section);
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
                return await sectionServiceProxy.TrackCompletion(sectionId, isCompleted);
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

        public async Task<bool> ValidateDependencies(int sectionId)
        {
            try
            {
                var dependencies = await sectionServiceProxy.GetSectionDependencies(sectionId);
                foreach (var dependency in dependencies)
                {
                    if (!dependency.IsCompleted)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"HTTP error validating dependencies: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error validating dependencies: {ex.Message}");
                return false;
            }
        }
    }
}