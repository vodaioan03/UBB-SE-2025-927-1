using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Api.DTO.Requests
{
    public class UpdateModuleRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsBonus { get; set; }
        public int? Cost { get; set; }
        public string? ImageUrl { get; set; }
        public int? CourseId { get; set; }
    }
}