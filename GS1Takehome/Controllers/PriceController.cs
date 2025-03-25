using Microsoft.AspNetCore.Mvc;

namespace GS1Takehome.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PriceController : ControllerBase
	{
		private readonly ILogger<PriceController> _logger;

		public PriceController(ILogger<PriceController> logger)
		{
			_logger = logger;
		}

		[HttpGet("submit", Name = "SubmitPrice")]
		public String SubmitPrice()
		{
			return "Ping";
		}

		[HttpGet("submitStatus", Name = "SubmitPriceStatus")]
		public String SubmitPriceStatus()
		{
			return "Ping";
		}

		[HttpGet("ping", Name = "Ping")]
		public String Ping()
		{
			return "Ping";
		}
	}
}