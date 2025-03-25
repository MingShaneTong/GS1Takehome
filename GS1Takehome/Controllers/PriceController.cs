using GS1Takehome.Models.Entities;
using GS1Takehome.Models.ResponseModels;
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

		[HttpPost("submit", Name = "SubmitPrice")]
		public SubmitPriceResponse SubmitPrice(ItemPrice itemPrice)
		{
			return new SubmitPriceResponse() 
			{ 
				Id = 1
			};
		}

		[HttpGet("priceSubmissionStatus", Name = "SubmitPriceStatus")]
		public PriceStatusResponse SubmitPriceStatus(string id)
		{
			return new PriceStatusResponse()
			{
				Status = PriceStatus.Failed,
				Reason = "Default Response"
			};
		}

		[HttpGet("ping", Name = "Ping")]
		public String Ping()
		{
			return "Ping";
		}
	}
}