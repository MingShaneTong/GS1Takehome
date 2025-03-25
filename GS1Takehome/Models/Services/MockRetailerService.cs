using GS1Takehome.Models.Entities;
using System.Collections.Concurrent;

namespace GS1Takehome.Models.Services
{
    public class MockRetailerService : IDataReceiverService
    {
        private ConcurrentDictionary<string, bool> GtinStatusCollection;
        public MockRetailerService()
        {
            GtinStatusCollection = new ConcurrentDictionary<string, bool>();
            SetGtinStatusPreset();
        }

        public bool CanSubmitPrice(string gtin)
        {
            if (GtinStatusCollection.ContainsKey(gtin))
            {
                return GtinStatusCollection[gtin];
            }
            else
            {
                GtinStatusCollection.TryAdd(gtin, false);
                UpdateGtinStatus(gtin);
                return GtinStatusCollection[gtin];
            }
        }

        public void SubmitPrice(ItemPrice price)
        {
            if (!CanSubmitPrice(price.Gtin))
            {
                throw new Exception($"Cannot submit price for GTIN {price.Gtin}.Please check `CanSubmitPrice()` before submitting aprice.");
            }
        }

        private void SetGtinStatusPreset()
        {
            GtinStatusCollection.TryAdd("05021731354670", true);
            GtinStatusCollection.TryAdd("09400096000778", false);
            GtinStatusCollection.TryAdd("09400096001737", true);
            GtinStatusCollection.TryAdd("09400096002628", false);
            GtinStatusCollection.TryAdd("09400096000952", true);
            GtinStatusCollection.TryAdd("09415767676206", false);
            GtinStatusCollection.TryAdd("04549995085334", true);
            GtinStatusCollection.TryAdd("03736940574574", false);
            GtinStatusCollection.TryAdd("09300617325352", true);
            GtinStatusCollection.TryAdd("09414952105484", false);
            GtinStatusCollection.TryAdd("09416946000058", true);
            GtinStatusCollection.TryAdd("00084310248543", false);
            GtinStatusCollection.TryAdd("07739145560645", true);
            GtinStatusCollection.TryAdd("00022927020541", false);
            GtinStatusCollection.TryAdd("08802946000081", true);
            GtinStatusCollection.TryAdd("04561312315665", false);
            GtinStatusCollection.TryAdd("09416050901845", true);
            GtinStatusCollection.TryAdd("09400571748119", false);
            GtinStatusCollection.TryAdd("02932871865878", true);
            GtinStatusCollection.TryAdd("09421906370003", false);
        }

        private void UpdateGtinStatus(string gtin)
        {
            Task.Run(async () =>
            {
                var random = new Random();
                int delay = random.Next(10_000, 60_000);
                await Task.Delay(delay);
                GtinStatusCollection[gtin] = true;
            });
        }
    }
}
