using Microsoft.AspNetCore.Mvc;

namespace Paycaso.CardReader.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardReaderController : ControllerBase
    {
        private readonly ILogger<CardReaderController> _logger;

        public CardReaderController(ILogger<CardReaderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get OK.
        /// </summary>
        /// <response code="200">Returns just OK message.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOk()
        {
            return Ok("Ok!!!");
        }
    }
}