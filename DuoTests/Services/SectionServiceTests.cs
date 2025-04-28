using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Services;
using Duo.Models.Sections;

namespace Duo.Tests.Services
{
    [TestClass]
    public class SectionServiceUT
    {
        private Mock<SectionServiceProxy> mockProxy;
        private SectionService sectionService;

        [TestInitialize]
        public void Setup()
        {
            mockProxy = new Mock<SectionServiceProxy>();
            sectionService = new SectionService(mockProxy.Object);
        }

        [TestMethod]
        public async Task GetAllSections_ReturnsSections()
        {
            // Arrange
            var sections = new List<Section> { new Section { SectionId = 1 }, new Section { SectionId = 2 } };
            mockProxy.Setup(p => p.GetAllSections()).ReturnsAsync(sections);

            // Act
            var result = await sectionService.GetAllSections();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetSectionById_ReturnsSection()
        {
            // Arrange
            var section = new Section { SectionId = 1 };
            mockProxy.Setup(p => p.GetSectionById(1)).ReturnsAsync(section);

            // Act
            var result = await sectionService.GetSectionById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SectionId);
        }

        [TestMethod]
        public async Task GetByRoadmapId_ReturnsSections()
        {
            // Arrange
            var sections = new List<Section> { new Section { SectionId = 1 } };
            mockProxy.Setup(p => p.GetByRoadmapId(5)).ReturnsAsync(sections);

            // Act
            var result = await sectionService.GetByRoadmapId(5);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task CountSectionsFromRoadmap_ReturnsCount()
        {
            // Arrange
            mockProxy.Setup(p => p.CountSectionsFromRoadmap(10)).ReturnsAsync(3);

            // Act
            var count = await sectionService.CountSectionsFromRoadmap(10);

            // Assert
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_ReturnsOrderNumber()
        {
            // Arrange
            mockProxy.Setup(p => p.LastOrderNumberFromRoadmap(2)).ReturnsAsync(5);

            // Act
            var order = await sectionService.LastOrderNumberFromRoadmap(2);

            // Assert
            Assert.AreEqual(5, order);
        }

        [TestMethod]
        public async Task AddSection_AddsSectionAndSetsOrder()
        {
            // Arrange
            var sections = new List<Section> { new Section { SectionId = 1 }, new Section { SectionId = 2 } };
            mockProxy.Setup(p => p.GetAllSections()).ReturnsAsync(sections);
            mockProxy.Setup(p => p.AddSection(It.IsAny<Section>())).ReturnsAsync(3);

            var newSection = new Section { Title = "New Section" };

            // Act
            var sectionId = await sectionService.AddSection(newSection);

            // Assert
            Assert.AreEqual(3, sectionId);
            Assert.AreEqual(3, newSection.OrderNumber); // Should be sections.Count + 1
        }

        [TestMethod]
        public async Task DeleteSection_DeletesSection()
        {
            // Act
            await sectionService.DeleteSection(1);

            // Assert
            mockProxy.Verify(p => p.DeleteSection(1), Times.Once);
        }

        [TestMethod]
        public async Task UpdateSection_UpdatesSection()
        {
            // Arrange
            var section = new Section { SectionId = 1, Title = "Updated Title" };

            // Act
            await sectionService.UpdateSection(section);

            // Assert
            mockProxy.Verify(p => p.UpdateSection(section), Times.Once);
        }

        [TestMethod]
        public async Task TrackCompletion_ReturnsCompletionStatus()
        {
            // Arrange
            mockProxy.Setup(p => p.TrackCompletion(1, true)).ReturnsAsync(true);

            // Act
            var result = await sectionService.TrackCompletion(1, true);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ValidateDependencies_ReturnsTrue_WhenAllCompleted()
        {
            // Arrange
            var dependencies = new List<SectionDependency>
            {
                new SectionDependency { IsCompleted = true },
                new SectionDependency { IsCompleted = true }
            };
            mockProxy.Setup(p => p.GetSectionDependencies(1)).ReturnsAsync(dependencies);

            // Act
            var result = await sectionService.ValidateDependencies(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ValidateDependencies_ReturnsFalse_WhenAnyDependencyNotCompleted()
        {
            // Arrange
            var dependencies = new List<SectionDependency>
            {
                new SectionDependency { IsCompleted = true },
                new SectionDependency { IsCompleted = false }
            };
            mockProxy.Setup(p => p.GetSectionDependencies(1)).ReturnsAsync(dependencies);

            // Act
            var result = await sectionService.ValidateDependencies(1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
