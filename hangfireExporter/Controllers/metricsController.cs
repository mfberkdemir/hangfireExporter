using Microsoft.AspNetCore.Mvc;

namespace hangfireExporter.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class metricsController : ControllerBase
    {        
        public string Get()
        {
            return "";
        }        
    }
}
