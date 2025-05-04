//using Moq;
//using Xunit;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Duo.Services;
//using Duo.Models.Sections;
//using Assert = Xunit.Assert;

//namespace Duo.Tests.Services
//{
//    public class SectionServiceTests
//    {
//        private readonly Mock<SectionServiceProxy> mockProxy;
//        private readonly SectionService sectionService;

//        public SectionServiceTests()
//        {
//            mockProxy = new Mock<SectionServiceProxy>();
//            sectionService = new SectionService(mockProxy.Object);
//        }

//        [Fact]
//        public async Task GetAllSections_ReturnsSections()
//        {
//            var sections = new List<Section> { new Section { SectionId = 1 }, new Section { SectionId = 2 } };
//            mockProxy.Setup(p => p.GetAllSections()).ReturnsAsync(sections);

//            var result = await sectionService.GetAllSections();

//            Assert.Equal(2, result.Count);
//        }

//        [Fact]
//        public async Task GetSectionById_ReturnsSection()
//        {
//            var section = new Section { SectionId = 1 };
//            mockProxy.Setup(p => p.GetSectionById(1)).ReturnsAsync(section);

//            var result = await sectionService.GetSectionById(1);

//            Assert.NotNull(result);
//            Assert.Equal(1, result.SectionId);
//        }

//        [Fact]
//        public async Task GetByRoadmapId_ReturnsSections()
//        {
//            var sections = new List<Section> { new Section { SectionId = 1 } };
//            mockProxy.Setup(p => p.GetByRoadmapId(5)).ReturnsAsync(sections);

//            var result = await sectionService.GetByRoadmapId(5);

//            Assert.Single(result);
//        }

//        [Fact]
//        public async Task CountSectionsFromRoadmap_ReturnsCount()
//        {
//            mockProxy.Setup(p => p.CountSectionsFromRoadmap(10)).ReturnsAsync(3);

//            var count = await sectionService.CountSectionsFromRoadmap(10);

//            Assert.Equal(3, count);
//        }

//        [Fact]
//        public async Task LastOrderNumberFromRoadmap_ReturnsOrderNumber()
//        {
//            mockProxy.Setup(p => p.LastOrderNumberFromRoadmap(2)).ReturnsAsync(5);

//            var order = await sectionService.LastOrderNumberFromRoadmap(2);

//            Assert.Equal(5, order);
//        }

//        [Fact]
//        public async Task AddSection_AddsSectionAndSetsOrder()
//        {
//            var sections = new List<Section> { new Section { SectionId = 1 }, new Section { SectionId = 2 } };
//            mockProxy.Setup(p => p.GetAllSections()).ReturnsAsync(sections);
//            mockProxy.Setup(p => p.AddSection(It.IsAny<Section>())).ReturnsAsync(3);

//            var newSection = new Section { Title = "New Section" };

//            var sectionId = await sectionService.AddSection(newSection);

//            Assert.Equal(3, sectionId);
//            Assert.Equal(3, newSection.OrderNumber);
//        }

//        [Fact]
//        public async Task DeleteSection_DeletesSection()
//        {
//            await sectionService.DeleteSection(1);

//            mockProxy.Verify(p => p.DeleteSection(1), Times.Once);
//        }

//        [Fact]
//        public async Task UpdateSection_UpdatesSection()
//        {
//            var section = new Section { SectionId = 1, Title = "Updated Title" };

//            await sectionService.UpdateSection(section);

//            mockProxy.Verify(p => p.UpdateSection(section), Times.Once);
//        }

//        [Fact]
//        public async Task TrackCompletion_ReturnsCompletionStatus()
//        {
//            mockProxy.Setup(p => p.TrackCompletion(1, true)).ReturnsAsync(true);

//            var result = await sectionService.TrackCompletion(1, true);

//            Assert.True(result);
//        }

//        [Fact]
//        public async Task ValidateDependencies_ReturnsTrue_WhenAllCompleted()
//        {
//            var dependencies = new List<SectionDependency>
//            {
//                new SectionDependency { IsCompleted = true },
//                new SectionDependency { IsCompleted = true }
//            };
//            mockProxy.Setup(p => p.GetSectionDependencies(1)).ReturnsAsync(dependencies);

//            var result = await sectionService.ValidateDependencies(1);

//            Assert.True(result);
//        }

//        [Fact]
//        public async Task ValidateDependencies_ReturnsFalse_WhenAnyDependencyNotCompleted()
//        {
//            var dependencies = new List<SectionDependency>
//            {
//                new SectionDependency { IsCompleted = true },
//                new SectionDependency { IsCompleted = false }
//            };
//            mockProxy.Setup(p => p.GetSectionDependencies(1)).ReturnsAsync(dependencies);

//            var result = await sectionService.ValidateDependencies(1);

//            Assert.False(result);
//        }
//    }
//}
