using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Duo.Models.Sections;
using Duo.Services;
using Duo.Services.Interfaces;
using Duo.Models.Quizzes;

namespace Duo.Tests.Services
{
    [TestClass]
    public class SectionServiceTests
    {
        private Mock<ISectionServiceProxy> mockProxy;
        private SectionService sectionService;

        [TestInitialize]
        public void Setup()
        {
            mockProxy = new Mock<ISectionServiceProxy>();
            sectionService = new SectionService(mockProxy.Object); 
        }
        private Section CreateSampleSection(int id) => new Section(
            id: id,
            subjectId: 1,
            title: $"Section {id}",
            description: $"Description {id}",
            roadmapId: 1,
            orderNumber: id
)
        {
            Quizzes = new List<Quiz> {
        new Quiz(id: 1, sectionId: id, orderNumber: 1),
        new Quiz(id: 2, sectionId: id, orderNumber: 2)
    },
            Exam = new Exam(id: 1, sectionId: id)
        };

        [TestMethod]
        public async Task AddSection_HttpException_ReturnsZero()
        {
            // Arrange
            var section = CreateSampleSection(1);
            mockProxy.Setup(p => p.GetAllSections()).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.AddSection(section);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task AddSection_UnexpectedException_ReturnsZero()
        {
            // Arrange
            var section = CreateSampleSection(1);
            mockProxy.Setup(p => p.GetAllSections()).ThrowsAsync(new System.Exception("Unexpected error"));

            // Act
            var result = await sectionService.AddSection(section);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task AddSection_NoExistingSections_SetsOrder1()
        {
            // Arrange
            var section = CreateSampleSection(1);
            mockProxy.Setup(p => p.GetAllSections()).ReturnsAsync(new List<Section>());
            mockProxy.Setup(p => p.AddSection(section)).ReturnsAsync(1);

            // Act
            var result = await sectionService.AddSection(section);

            // Assert
            Assert.AreEqual(1, section.OrderNumber);
        }


        [TestMethod]
        public async Task CountSectionsFromRoadmap_HttpException_ReturnsZero()
        {
            // Arrange
            mockProxy.Setup(p => p.CountSectionsFromRoadmap(1)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.CountSectionsFromRoadmap(1);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetAllSections_HttpException_ReturnsEmptyList()
        {
            // Arrange
            mockProxy.Setup(p => p.GetAllSections()).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.GetAllSections();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByRoadmapId_HttpException_ReturnsEmptyList()
        {
            // Arrange
            mockProxy.Setup(p => p.GetByRoadmapId(1)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.GetByRoadmapId(1);

            // Assert
            Assert.AreEqual(0, result.Count);
        }


        [TestMethod]
        public async Task GetSectionById_HttpException_ReturnsNull()
        {
            // Arrange
            mockProxy.Setup(p => p.GetSectionById(1)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.GetSectionById(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_HttpException_ReturnsZero()
        {
            // Arrange
            mockProxy.Setup(p => p.LastOrderNumberFromRoadmap(1)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.LastOrderNumberFromRoadmap(1);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task TrackCompletion_HttpException_ReturnsFalse()
        {
            // Arrange
            mockProxy.Setup(p => p.TrackCompletion(1, true)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.TrackCompletion(1, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TrackCompletion_NullSection_ReturnsFalse()
        {
            // Arrange
            mockProxy.Setup(p => p.GetSectionById(1)).ReturnsAsync((Section)null);

            // Act
            var result = await sectionService.TrackCompletion(1, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetSectionById_NotFound_ReturnsNull()
        {
            // Arrange
            mockProxy.Setup(p => p.GetSectionById(1)).ReturnsAsync((Section)null);

            // Act
            var result = await sectionService.GetSectionById(1);

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ValidateDependencies_HttpException_ReturnsFalse()
        {
            // Arrange
            mockProxy.Setup(p => p.GetSectionDependencies(1)).ThrowsAsync(new HttpRequestException("HTTP error"));

            // Act
            var result = await sectionService.ValidateDependencies(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ValidateDependencies_NullDependencies_ReturnsFalse()
        {
            // Arrange
            mockProxy.Setup(p => p.GetSectionDependencies(1))
                     .ReturnsAsync((List<SectionDependency>)null);

            // Act
            var result = await sectionService.ValidateDependencies(1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
