using GS1Takehome.Models.Entities;

namespace GS1Takehome.Models.Services
{
    public class CounterRetailerService : IDataReceiverService
    {
        public int Counter;
        public CounterRetailerService(int countdown) 
        {
            Counter = countdown;
        }

        public bool CanSubmitPrice(string gtin)
        {
            // will not return true until a few attempts
            Counter--;
            return Counter <= 0;
        }

        public void SubmitPrice(ItemPrice price)
        {
			if (Counter > 0)
			{
                throw new Exception("Counter has not completed.");
			}
		}
    }
}
