using System.Runtime.Serialization;

namespace Service.CoinMarketCapReader.Grpc.Models;

[DataContract]
public class MarketInfoDeleteRequest
{
    [DataMember(Order = 1)] public string Asset { get; set; }
    [DataMember(Order = 2)] public string BrokerId { get; set; }
}