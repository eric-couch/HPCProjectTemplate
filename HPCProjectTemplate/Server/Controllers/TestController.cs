using Microsoft.AspNetCore.Mvc;

namespace HPCProjectTemplate.Server.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        [Route("api/test")]
        [Route("api/test/index")]
        [Route("api/erictest")]
        [Route("api/test/{id}")]
        // valid http methods: get, post, put, delete, patch, head, options
        public IActionResult Index(int id)
        {
            return Ok($"Hello World. The id passed to this method was {id}");
        }
    }
}
