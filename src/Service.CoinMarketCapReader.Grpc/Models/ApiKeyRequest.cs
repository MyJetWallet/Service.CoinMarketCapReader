using System.Runtime.Serialization;

namespace Service.CoinMarketCapReader.Grpc.Models
{
    [DataContract]
    public class ApiKeyRequest
    {
        [DataMember(Order = 1)]
        public string ApiKey { get; set; }
    }
}