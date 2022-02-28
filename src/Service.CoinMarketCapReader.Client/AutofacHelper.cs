using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyNoSqlServer.DataReader;
using Service.CoinMarketCapReader.Domain.Models.NoSql;
using Service.CoinMarketCapReader.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.CoinMarketCapReader.Client
{
    public static class AutofacHelper
    {
        public static void RegisterCoinMarketCapReaderClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CoinMarketCapReaderClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCMCReaderService()).As<ICMCReaderService>().SingleInstance();
        }

        public static void RegisterCoinMarketCapNoSqlReader(this ContainerBuilder builder, IMyNoSqlSubscriber client)
        {
            builder.RegisterMyNoSqlReader<MarketInfoNoSqlEntity>(client,
                MarketInfoNoSqlEntity.TableName);
        }

    }
}
