using GS1Takehome.Models.Entities;

namespace GS1Takehome.Models.Services
{
	public class FailureRetailerService : IDataReceiverService
	{
		public bool CanSubmitPrice(string gtin)
		{
			return false;
		}

		public void SubmitPrice(ItemPrice price)
		{
			throw new Exception("Service will always fail");
		}
	}
}
