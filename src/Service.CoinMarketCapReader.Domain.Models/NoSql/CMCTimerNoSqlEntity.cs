using MyNoSqlServer.Abstractions;

namespace Service.CoinMarketCapReader.Domain.Models.NoSql
{
    public class CMCTimerNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "coinmarketcap-api-keys";

        public static string GeneratePartitionKey() => "timers";
        public static string GenerateRowKey() => "timer";

        public TimerPeriods TimerPeriods { get; set; }

        public static CMCTimerNoSqlEntity Create(TimerPeriods timers) => new CMCTimerNoSqlEntity()
        {
            TimerPeriods = timers,
            RowKey = GenerateRowKey(),
            PartitionKey = GeneratePartitionKey()
        };
    }
}