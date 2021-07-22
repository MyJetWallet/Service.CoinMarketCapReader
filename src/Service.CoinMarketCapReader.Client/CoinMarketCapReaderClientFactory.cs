using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.CoinMarketCapReader.Grpc;

namespace Service.CoinMarketCapReader.Client
{
    [UsedImplicitly]
    public class CoinMarketCapReaderClientFactory: MyGrpcClientFactory
    {
        public CoinMarketCapReaderClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public ICMCReaderService GetCMCReaderService() => CreateGrpcService<ICMCReaderService>();
    }
}
