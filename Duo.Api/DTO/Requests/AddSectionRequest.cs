using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Api.DTO.Requests
{
    public class AddSectionRequest
    {
        public int? SubjectId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int RoadmapId { get; set; }
        public int? OrderNumber { get; set; }
    }
}