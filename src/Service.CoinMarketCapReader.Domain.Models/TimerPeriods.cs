namespace Service.CoinMarketCapReader.Domain.Models.NoSql
{
    public class TimerPeriods
    {
        public int CoinInfoTimerInMin { get; set; }
        public int MarketInfoTimerInSec { get; set; }
    }
}