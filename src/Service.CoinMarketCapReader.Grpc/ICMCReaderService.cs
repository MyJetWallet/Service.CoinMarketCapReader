using System.ServiceModel;
using System.Threading.Tasks;
using Service.CoinMarketCapReader.Grpc.Models;

namespace Service.CoinMarketCapReader.Grpc
{
    [ServiceContract]
    public interface ICMCReaderService
    {
        [OperationContract]
        Task AddApiKey(ApiKeyRequest request);
        
        [OperationContract]
        Task RemoveApiKey(ApiKeyRequest request);
       
        [OperationContract]
        Task UpdateMarketInfo(MarketInfoUpdateMessage request);
        
        [OperationContract]
        Task<OperationResponse> CreateMarketInfo(MarketInfoUpdateMessage request);

        [OperationContract]
        Task<OperationResponse> DeleteMarketInfo(MarketInfoDeleteRequest request);
    }
}