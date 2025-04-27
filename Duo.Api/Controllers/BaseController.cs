using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected DataContext dataContext;
        protected IRepository repository;
        public BaseController(DataContext dataContext, IRepository repository)
        {
            this.dataContext = dataContext;
            this.repository = repository;
        }
    }
}