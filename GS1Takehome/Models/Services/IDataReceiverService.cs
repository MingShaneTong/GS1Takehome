using GS1Takehome.Models.Entities;

namespace GS1Takehome.Models.Services
{
	/**
	 * The service that handles price data. 
	 */
    public interface IDataReceiverService
	{
		bool CanSubmitPrice(string gtin);
		void SubmitPrice(ItemPrice price);
	}
}
