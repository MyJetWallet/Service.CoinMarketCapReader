using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service.CoinMarketCapReader.Domain.Models
{
    public class CMCMarketInfoResponse
    {
        [JsonPropertyName("data")] public List<MarketInfoData> Data { get; set; }
    }

    public class MarketInfoData
    {
        [JsonPropertyName("symbol")] public string Symbol { get; set; }
        [JsonPropertyName("total_supply")] public double TotalSupply { get; set; }
        [JsonPropertyName("cmc_rank")] public long CmcRank { get; set; }
        [JsonPropertyName("quote")] public Quote Quote { get; set; }
    }

    public class Quote
    {
        [JsonPropertyName("USD")] public QuoteValues QuoteValues { get; set; }
    }

    public class QuoteValues
    {
        [JsonPropertyName("price")] public double Price { get; set; }
        [JsonPropertyName("market_cap")] public double MarketCap { get; set; }
        [JsonPropertyName("volume_24h")] public double Volume24 { get; set; }
        [JsonPropertyName("volume_change_24h")] public double VolumeChange24H { get; set; }
        [JsonPropertyName("percent_change_1h")] public double PercentChange1H { get; set; }
        [JsonPropertyName("percent_change_24h")] public double PercentChange24H { get; set; }
        [JsonPropertyName("percent_change_7d")] public double PercentChange7d { get; set; }
        [JsonPropertyName("percent_change_30d")] public double PercentChange30d { get; set; }
        [JsonPropertyName("percent_change_60d")] public double PercentChange60d { get; set; }
        [JsonPropertyName("percent_change_90d")] public double PercentChange90d { get; set; }
    }
}