using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Sections;
using Duo.Exceptions;

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
            try
            {
                return await sectionServiceProxy.GetAllSections();
            }
            catch (Exception ex)
            {
                throw new SectionServiceException("Failed to retrieve all sections.", ex);
            }
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            try
            {
                return await sectionServiceProxy.GetSectionById(sectionId);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to retrieve section with ID {sectionId}.", ex);
            }
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            try
            {
                return await sectionServiceProxy.GetByRoadmapId(roadmapId);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to retrieve sections for roadmap ID {roadmapId}.", ex);
            }
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            try
            {
                return await sectionServiceProxy.CountSectionsFromRoadmap(roadmapId);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to count sections for roadmap ID {roadmapId}.", ex);
            }
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            try
            {
                return await sectionServiceProxy.LastOrderNumberFromRoadmap(roadmapId);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to get last order number for roadmap ID {roadmapId}.", ex);
            }
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
            catch (Exception ex)
            {
                throw new SectionServiceException("Failed to add new section.", ex);
            }
        }

        public async Task DeleteSection(int sectionId)
        {
            try
            {
                await sectionServiceProxy.DeleteSection(sectionId);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to delete section with ID {sectionId}.", ex);
            }
        }

        public async Task UpdateSection(Section section)
        {
            try
            {
                ValidationHelper.ValidateSection(section);
                await sectionServiceProxy.UpdateSection(section);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to update section with ID {section.Id}.", ex);
            }
        }

        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            try
            {
                return await sectionServiceProxy.TrackCompletion(sectionId, isCompleted);
            }
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to track completion for section ID {sectionId}.", ex);
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
            catch (Exception ex)
            {
                throw new SectionServiceException($"Failed to validate dependencies for section ID {sectionId}.", ex);
            }
        }
    }
}
