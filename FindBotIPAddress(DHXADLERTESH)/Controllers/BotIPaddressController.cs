using BOTIP.Model;
using BOTIP.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FindBotIPAddress_DHXADLERTESH_.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotIPaddressController : ControllerBase
    {
        

        private readonly ILogger<BotIPaddressController> _logger;
        private readonly IBOTService _service;
        private readonly IWebHostEnvironment _env;

        public BotIPaddressController(ILogger<BotIPaddressController> logger, IWebHostEnvironment env, IBOTService service)
        {
            _env = env;
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetBotIPAddress")]
        public Response Get(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, "bin", fileName);
                // Here we can use factory or abstarct factory design pattern of creation Design pattern to
                // create instance of service
               var response = this._service.Process(filePath);

                return new Response()
                {
                    suspiciousIps = response
                };

            }
            catch(Exception ex)
            {
                return new Response()
                {
                    Error = ex.Message
                };
            }
            
        }
    }
}