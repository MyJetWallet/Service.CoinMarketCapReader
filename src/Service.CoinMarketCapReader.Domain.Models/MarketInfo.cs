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
        public string AboutLessTemplateId { get; set; }
        public string AboutMoreTemplateId { get; set; }
        public double VolumeChange24H { get; set; }
        public double PercentChange1H { get; set; }
        public double PercentChange24H { get; set; }
        public double PercentChange7d { get; set; }
        public double PercentChange30d { get; set; }
        public double PercentChange60d { get; set; }
        public double PercentChange90d { get; set; }
        public double MarketCapChange24H { get; set; }

    }
}