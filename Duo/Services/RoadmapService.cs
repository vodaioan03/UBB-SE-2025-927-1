using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Repositories;

namespace Duo.Services
{
    public class RoadmapService : IRoadmapService
    {
        private RoadmapServiceProxy serviceProxy;

        public RoadmapService(RoadmapServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        public async Task<List<Roadmap>> GetAllRoadmaps()
        {
            return await serviceProxy.GetAllAsync();
        }

        public async Task<Roadmap> GetRoadmapById(int roadmapId)
        {
            return await serviceProxy.GetByIdAsync(roadmapId);
        }

        public async Task<Roadmap> GetByName(string roadmapName)
        {
            return await serviceProxy.GetByNameAsync(roadmapName);
        }

        public async Task<int> AddRoadmap(Roadmap roadmap)
        {
            return await serviceProxy.AddAsync(roadmap);
        }

        public async Task DeleteRoadmap(Roadmap roadmap)
        {
            await serviceProxy.DeleteAsync(roadmap.Id);
        }
    }
}
