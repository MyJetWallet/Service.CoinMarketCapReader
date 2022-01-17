using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.AssetsDictionary.Client;
using Service.CoinMarketCapReader.Domain.Models.NoSql;
using Service.CoinMarketCapReader.Jobs;
using Service.MessageTemplates.Client;

namespace Service.CoinMarketCapReader.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CMCUpdateJob>().AsSelf().AutoActivate().SingleInstance();

            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));
            builder.RegisterAssetsDictionaryClients(noSqlClient);

            builder.RegisterMyNoSqlWriter<CMCApiKeyNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl),
                CMCApiKeyNoSqlEntity.TableName);

            builder.RegisterMyNoSqlWriter<MarketInfoNoSqlEntity>(
                Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), MarketInfoNoSqlEntity.TableName);
            
            builder.RegisterMyNoSqlWriter<CMCTimerNoSqlEntity>(
                Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), CMCTimerNoSqlEntity.TableName);
            
            builder.RegisterMessageTemplatesClient(Program.Settings.MessageTemplatesGrpcServiceUrl);
        }
    }
}