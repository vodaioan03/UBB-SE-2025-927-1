using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Api.DTO.Requests
{
    public class UpdateSectionRequest
    {
        public int? SubjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? RoadmapId { get; set; }
        public int? OrderNumber { get; set; }
    }
}
