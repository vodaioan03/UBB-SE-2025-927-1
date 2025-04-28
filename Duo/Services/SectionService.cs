using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Services
{
    public class SectionService : ISectionService
    {
        private readonly SectionServiceProxy sectionServiceProxy;

        public SectionService(SectionServiceProxy sectionServiceProxy)
        {
            this.sectionServiceProxy = sectionServiceProxy;
        }

        public async Task<List<Section>> GetAllSections()
        {
            return await sectionServiceProxy.GetAllSections();
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            return await sectionServiceProxy.GetSectionById(sectionId);
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            return await sectionServiceProxy.GetByRoadmapId(roadmapId);
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            return await sectionServiceProxy.CountSectionsFromRoadmap(roadmapId);
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            return await sectionServiceProxy.LastOrderNumberFromRoadmap(roadmapId);
        }

        public async Task<int> AddSection(Section section)
        {
            // Validate section via API
            ValidationHelper.ValidateSection(section);

            // Get all sections to determine the order number
            var allSections = await GetAllSections();
            section.OrderNumber = allSections.Count + 1;

            return await sectionServiceProxy.AddSection(section);
        }

        public async Task DeleteSection(int sectionId)
        {
            await sectionServiceProxy.DeleteSection(sectionId);
        }

        public async Task UpdateSection(Section section)
        {
            // Validate section via API
            ValidationHelper.ValidateSection(section);
            await sectionServiceProxy.UpdateSection(section);
        }

        // Track completion directly without the need for a separate model
        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            var response = await sectionServiceProxy.TrackCompletion(sectionId, isCompleted);
            return response;
        }

        // Validate section dependencies
        public async Task<bool> ValidateDependencies(int sectionId)
        {
            var dependencies = await sectionServiceProxy.GetSectionDependencies(sectionId);

            // Check dependencies logic here
            foreach (var dependency in dependencies)
            {
                if (!dependency.IsCompleted)
                {
                    return false; // Dependency not completed
                }
            }

            return true; // All dependencies met
        }
    }
}
