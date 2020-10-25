using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JedzenioPlanner.Api.Controllers.Public
{
    /// <summary>
    /// Disclaimer controller
    /// </summary>
    [Route("/disclaimer")]
    [ApiController]
    public class DisclaimerController
    {
        private readonly IConfiguration _configuration;

        public DisclaimerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Read disclaimer
        /// </summary>
        /// <returns>Disclaimer string</returns>
        /// <response code="200">When disclaimer is returned successfully</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string ReadDisclaimer()
            => _configuration["Disclaimer"];
    }
}