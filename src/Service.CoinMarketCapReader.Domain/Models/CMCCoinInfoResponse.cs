using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service.CoinMarketCapReader.Domain.Models
{
    public class CMCCoinInfoResponse
    {
        [JsonPropertyName("data")]
        public Dictionary<string, CoinInfo> Data { get; set; }
    }
    
    public class CoinInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("logo")]
        public string LogoUrl { get; set; }
        
        [JsonPropertyName("urls")]
        public Urls Urls { get; set; }
    }

    public class Urls
    {
        [JsonPropertyName("website")]
        public string[] Website { get; set; }
        
        [JsonPropertyName("technical_doc")]
        public string[] TechnicalDoc { get; set; }
    }
}