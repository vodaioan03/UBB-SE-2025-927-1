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

        public Task<List<Roadmap>> GetAllRoadmaps()
        {
            return serviceProxy.GetAllAsync();
        }

        public Task<Roadmap> GetRoadmapById(int roadmapId)
        {
            return serviceProxy.GetByIdAsync(roadmapId);
        }

        public Task<Roadmap> GetByName(string roadmapName)
        {
            return serviceProxy.GetByNameAsync(roadmapName);
        }

        public Task<int> AddRoadmap(Roadmap roadmap)
        {
            return serviceProxy.AddAsync(roadmap);
        }

        public Task DeleteRoadmap(Roadmap roadmap)
        {
            return serviceProxy.DeleteAsync(roadmap.Id);
        }
    }
}
