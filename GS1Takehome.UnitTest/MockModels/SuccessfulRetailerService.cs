using GS1Takehome.Models.Entities;

namespace GS1Takehome.Models.Services
{
    public class SuccessfulRetailerService : IDataReceiverService
    {
        public bool CanSubmitPrice(string gtin)
        {
            return true;
        }

        public void SubmitPrice(ItemPrice price)
        {
        }
    }
}
