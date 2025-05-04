using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Repositories;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class RoadmapService : IRoadmapService
    {
        private IRoadmapServiceProxy serviceProxy;

        public RoadmapService(IRoadmapServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        public async Task<List<Roadmap>> GetAllAsync()
        {
            try
            {
                return await serviceProxy.GetAllAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching roadmaps: {ex.Message}");
                return new List<Roadmap>();
            }
        }

        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
            try
            {
                return await serviceProxy.GetByIdAsync(roadmapId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching roadmap by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<Roadmap> GetByNameAsync(string roadmapName)
        {
            try
            {
                return await serviceProxy.GetByNameAsync(roadmapName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching roadmap by name: {ex.Message}");
                return null;
            }
        }

        public async Task<int> AddAsync(Roadmap roadmap)
        {
            try
            {
                return await serviceProxy.AddAsync(roadmap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding roadmap: {ex.Message}");
                return 0;
            }
        }

        public async Task DeleteAsync(Roadmap roadmap)
        {
            try
            {
                await serviceProxy.DeleteAsync(roadmap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting roadmap: {ex.Message}");
            }
        }
    }
}
