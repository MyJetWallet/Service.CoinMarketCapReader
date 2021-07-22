using MyNoSqlServer.Abstractions;

namespace Service.CoinMarketCapReader.Domain.Models.NoSql
{
    public class MarketInfoNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "coinmarketcap-market-info";

        public static string GeneratePartitionKey(string brokerId) => brokerId;
        public static string GenerateRowKey(string asset) => asset;

        public MarketInfo MarketInfo { get; set; }

        public static MarketInfoNoSqlEntity Create(MarketInfo info) => new MarketInfoNoSqlEntity()
        {
            MarketInfo = info,
            RowKey = GenerateRowKey(info.Asset),
            PartitionKey = GeneratePartitionKey(info.BrokerId)
        };
    }
}