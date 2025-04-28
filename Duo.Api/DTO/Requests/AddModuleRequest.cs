using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Api.DTO.Requests
{
    public class AddModuleRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required bool IsBonus { get; set; }
        public required int Cost { get; set; }
        public required string ImageUrl { get; set; }
        public int? CourseId { get; set; }
    }
}