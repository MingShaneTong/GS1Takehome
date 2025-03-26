using GS1Takehome.Models.Entities;
using System.Text.Json.Serialization;

namespace GS1Takehome.Models.ResponseModels
{
	public record PriceStatusResponse
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PriceStatus Status { get; set; }
		public string Reason { get; set; }
	}
}
