using Duo.Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected DataContext dataContext;
        public BaseController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}