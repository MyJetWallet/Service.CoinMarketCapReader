using System.Runtime.Serialization;
using Service.CoinMarketCapReader.Domain.Models;

namespace Service.CoinMarketCapReader.Grpc.Models
{
    [DataContract]
    public class MarketInfoUpdateMessage
    {
        [DataMember(Order = 1)] public string Asset { get; set; }
        [DataMember(Order = 2)] public string BrokerId { get; set; }
        [DataMember(Order = 3)] public string WhitepaperUrl { get; set; }
        [DataMember(Order = 4)] public string OfficialWebsiteUrl { get; set; }
        [DataMember(Order = 5)] public double MarketCap { get; set; }
        [DataMember(Order = 6)] public double Supply { get; set; }
        [DataMember(Order = 7)] public double Volume24 { get; set; }
        [DataMember(Order = 8)] public double Price { get; set; }
        [DataMember(Order = 9)] public string AboutLessTemplateId { get; set; }
        [DataMember(Order = 10)] public string AboutMoreTemplateId { get; set; }
    }
}