using MyNoSqlServer.Abstractions;

namespace Service.CoinMarketCapReader.Domain.Models.NoSql
{
    public class CMCApiKeyNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "coinmarketcap-api-keys";

        public static string GeneratePartitionKey() => "api-key";
        public static string GenerateRowKey(string key) => key;

        public string ApiKey { get; set; }

        public static CMCApiKeyNoSqlEntity Create(string key) => new CMCApiKeyNoSqlEntity()
        {
            ApiKey = key,
            RowKey = GenerateRowKey(key),
            PartitionKey = GeneratePartitionKey()
        };
    }
}