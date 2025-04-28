using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services
{
    public class RoadmapServiceProxy
    {
        private readonly HttpClient httpClient;

        public RoadmapServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Roadmap>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(Roadmap roadmap)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
            throw new NotImplementedException();
        }

        public async Task<Roadmap> GetByNameAsync(string roadmapName)
        {
            throw new NotImplementedException();
        }
    }
}
