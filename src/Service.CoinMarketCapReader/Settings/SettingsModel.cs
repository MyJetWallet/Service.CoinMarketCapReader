using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.CoinMarketCapReader.Settings
{
    public class SettingsModel
    {
        [YamlProperty("CoinMarketCapReader.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("CoinMarketCapReader.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("CoinMarketCapReader.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("CoinMarketCapReader.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }
        
        [YamlProperty("CoinMarketCapReader.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }
        
        [YamlProperty("CoinMarketCapReader.CoinMarketCapApiKey")]
        public string CoinMarketCapApiKey { get; set; }
        
        [YamlProperty("CoinMarketCapReader.CoinInfoTimerInMin")]
        public int CoinInfoTimerInMin { get; set; }
        
        [YamlProperty("CoinMarketCapReader.MarketInfoTimerInSec")]
        public int MarketInfoTimerInSec { get; set; }

        [YamlProperty("CoinMarketCapReader.MessageTemplatesGrpcServiceUrl")]
        public string MessageTemplatesGrpcServiceUrl { get; set; }
        
        [YamlProperty("CoinMarketCapReader.DefaultBrokerId")]
        public string DefaultBrokerId { get; set; }
    }
}
