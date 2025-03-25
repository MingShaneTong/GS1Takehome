using GS1Takehome.Models.Entities;

namespace GS1Takehome.Models.ResponseModels
{
	public record PriceStatusResponse
	{
		public PriceStatus Status { get; set; }
		public string Reason { get; set; }
	}
}
