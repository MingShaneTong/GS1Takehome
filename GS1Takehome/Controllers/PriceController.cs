using GS1Takehome.Models;
using GS1Takehome.Models.Entities;
using GS1Takehome.Models.ResponseModels;
using GS1Takehome.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace GS1Takehome.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PriceController : ControllerBase
	{
		private readonly ILogger<PriceController> _logger;
		private PriceModel PriceModel;

		public PriceController(ILogger<PriceController> logger, PriceModel priceModel)
		{
			_logger = logger;
			PriceModel = priceModel;
		}

		[HttpPost(Name = "SubmitPrice")]
		public SubmitPriceResponse PostSubmitPrice(ItemPrice itemPrice)
		{
			int id = PriceModel.SubmitPrice(itemPrice);
			return new SubmitPriceResponse() 
			{ 
				Id = id
			};
		}

		[HttpGet(Name = "SubmitPriceStatus")]
		public PriceStatusResponse GetSubmitPriceStatus(int id)
		{
			try 
			{
				PriceStatus status = PriceModel.GetSubmitPriceStatus(id);
				return new PriceStatusResponse()
				{
					Status = status
				};
			}
			catch (Exception ex)
			{
				return new PriceStatusResponse()
				{
					Status = PriceStatus.Failed,
					Reason = ex.Message
				};
			}
		}

		[HttpGet("ping", Name = "Ping")]
		public String Ping()
		{
			return "Ping";
		}
	}
}