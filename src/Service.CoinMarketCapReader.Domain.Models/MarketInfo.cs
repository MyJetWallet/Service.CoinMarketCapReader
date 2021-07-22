namespace Service.CoinMarketCapReader.Domain.Models
{
    public class MarketInfo
    {
        public string Asset { get; set; }
        public string BrokerId { get; set; }
        public string WhitepaperUrl { get; set; }
        public string OfficialWebsiteUrl { get; set; }
        public double MarketCap { get; set; }
        public double Supply { get; set; }
        public double Volume24 { get; set; }
        public double Price { get; set; }

    }
}