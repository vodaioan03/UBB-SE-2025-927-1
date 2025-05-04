using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Duo.Models.Sections;
using Duo.Services;

namespace Duo.Tests.Services
{
    [TestClass]
    public class SectionServiceProxyTests
    {
        private class TestHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
            public TestHandler(Func<HttpRequestMessage, HttpResponseMessage> r) => _responder = r;
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken ct) =>
                Task.FromResult(_responder(request));
        }

        private HttpClient CreateClient(Func<HttpRequestMessage, HttpResponseMessage> fn) =>
            new HttpClient(new TestHandler(fn));

        private Section SampleSection => new Section(
            id: 1,
            subjectId: 2,
            title: "T",
            description: "D",
            roadmapId: 3,
            orderNumber: 4
        );

        [TestMethod]
        public async Task AddSection_Success_ReturnsServerValue()
        {
            var expected = 123;
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            });

            var proxy = new SectionServiceProxy(client);
            var actual = await proxy.AddSection(SampleSection);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task AddSection_HttpRequestException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.AddSection(SampleSection));
        }

        [TestMethod]
        public async Task AddSection_GenericException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.AddSection(SampleSection));
        }

        [TestMethod]
        public async Task CountSectionsFromRoadmap_Success()
        {
            var expected = 7;
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            });
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(expected, await proxy.CountSectionsFromRoadmap(99));
        }

        [TestMethod]
        public async Task CountSectionsFromRoadmap_HttpRequestException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.CountSectionsFromRoadmap(1));
        }

        [TestMethod]
        public async Task CountSectionsFromRoadmap_GenericException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.CountSectionsFromRoadmap(1));
        }

        [TestMethod]
        public async Task DeleteSection_NoError_Completes()
        {
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK));
            var proxy = new SectionServiceProxy(client);

            await proxy.DeleteSection(5);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task DeleteSection_HttpException_Handled()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            await proxy.DeleteSection(5);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task DeleteSection_GenericException_Handled()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            await proxy.DeleteSection(5);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetAllSections_Success()
        {
            var list = new List<Section> { SampleSection };
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(list)
            });
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetAllSections();
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllSections_HttpException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetAllSections();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetAllSections_GenericException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetAllSections();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByRoadmapId_Success()
        {
            var list = new List<Section> { SampleSection };
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(list)
            });
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetByRoadmapId(2);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetByRoadmapId_HttpException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetByRoadmapId(2);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByRoadmapId_GenericException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetByRoadmapId(2);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetSectionById_Success()
        {
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(SampleSection)
            });
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetSectionById(3);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        public async Task GetSectionById_HttpException_ReturnsNull()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            Assert.IsNull(await proxy.GetSectionById(3));
        }

        [TestMethod]
        public async Task GetSectionById_GenericException_ReturnsNull()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            Assert.IsNull(await proxy.GetSectionById(3));
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_Success()
        {
            var expected = 88;
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            });
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(expected, await proxy.LastOrderNumberFromRoadmap(4));
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_HttpException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.LastOrderNumberFromRoadmap(4));
        }

        [TestMethod]
        public async Task LastOrderNumberFromRoadmap_GenericException_ReturnsZero()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            Assert.AreEqual(0, await proxy.LastOrderNumberFromRoadmap(4));
        }

        [TestMethod]
        public async Task UpdateSection_NoError_Completes()
        {
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK));
            var proxy = new SectionServiceProxy(client);

            await proxy.UpdateSection(SampleSection);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task UpdateSection_HttpException_Handled()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            await proxy.UpdateSection(SampleSection);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task UpdateSection_GenericException_Handled()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            await proxy.UpdateSection(SampleSection);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TrackCompletion_Success()
        {
            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(true)
            });
            var proxy = new SectionServiceProxy(client);

            Assert.IsTrue(await proxy.TrackCompletion(7, true));
        }

        [TestMethod]
        public async Task TrackCompletion_HttpException_ReturnsFalse()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            Assert.IsFalse(await proxy.TrackCompletion(7, true));
        }

        [TestMethod]
        public async Task TrackCompletion_GenericException_ReturnsFalse()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            Assert.IsFalse(await proxy.TrackCompletion(7, true));
        }

        [TestMethod]
        public async Task GetSectionDependencies_Success()
        {
            var deps = new List<SectionDependency>
            {
                new SectionDependency { SectionId = 1, IsCompleted = true }
            };

            var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(deps)
            });
            var proxy = new SectionServiceProxy(client);

            // act
            var result = await proxy.GetSectionDependencies(5);

            // assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].SectionId);
            Assert.IsTrue(result[0].IsCompleted);
        }

        [TestMethod]
        public async Task GetSectionDependencies_HttpException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new HttpRequestException()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetSectionDependencies(5);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetSectionDependencies_GenericException_ReturnsEmpty()
        {
            var client = new HttpClient(new TestHandler(_ => throw new Exception()));
            var proxy = new SectionServiceProxy(client);

            var result = await proxy.GetSectionDependencies(5);
            Assert.AreEqual(0, result.Count);
        }
    }
}
