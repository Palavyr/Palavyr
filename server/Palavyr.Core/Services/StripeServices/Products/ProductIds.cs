namespace Palavyr.Core.Services.StripeServices.Products
{
    public class ProductIds
    {
        public string FreeProductId { get; set; }
        public string LyteProductId { get; set; }
        public string PremiumProductId { get; set; }
        public string ProProductId { get; set; }

        public ProductIds(
            string freeProductId,
            string lyteProductId,
            string premiumProductId,
            string proProductId
        )
        {
            FreeProductId = freeProductId;
            LyteProductId = lyteProductId;
            PremiumProductId = premiumProductId;
            ProProductId = proProductId;
        }
    }
}