using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected IRepository repository;
        public BaseController(IRepository repository)
        {
            this.repository = repository;
        }
    }
}