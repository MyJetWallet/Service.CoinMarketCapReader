using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service.CoinMarketCapReader.Domain.Models
{
    public class CMCGlobalMetricsResponse
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public object ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }

        [JsonPropertyName("notice")]
        public object Notice { get; set; }
    }

    public class Qoute
    {
        [JsonPropertyName("total_market_cap")]
        public double TotalMarketCap { get; set; }

        [JsonPropertyName("total_volume_24h")]
        public double TotalVolume24h { get; set; }

        [JsonPropertyName("total_volume_24h_reported")]
        public double TotalVolume24hReported { get; set; }

        [JsonPropertyName("altcoin_volume_24h")]
        public double AltcoinVolume24h { get; set; }

        [JsonPropertyName("altcoin_volume_24h_reported")]
        public double AltcoinVolume24hReported { get; set; }

        [JsonPropertyName("altcoin_market_cap")]
        public double AltcoinMarketCap { get; set; }

        [JsonPropertyName("defi_volume_24h")]
        public double DefiVolume24h { get; set; }

        [JsonPropertyName("defi_volume_24h_reported")]
        public double DefiVolume24hReported { get; set; }

        [JsonPropertyName("defi_24h_percentage_change")]
        public double Defi24hPercentageChange { get; set; }

        [JsonPropertyName("defi_market_cap")]
        public double DefiMarketCap { get; set; }

        [JsonPropertyName("stablecoin_volume_24h")]
        public double StablecoinVolume24h { get; set; }

        [JsonPropertyName("stablecoin_volume_24h_reported")]
        public double StablecoinVolume24hReported { get; set; }

        [JsonPropertyName("stablecoin_24h_percentage_change")]
        public double Stablecoin24hPercentageChange { get; set; }

        [JsonPropertyName("stablecoin_market_cap")]
        public double StablecoinMarketCap { get; set; }

        [JsonPropertyName("derivatives_volume_24h")]
        public double DerivativesVolume24h { get; set; }

        [JsonPropertyName("derivatives_volume_24h_reported")]
        public double DerivativesVolume24hReported { get; set; }

        [JsonPropertyName("derivatives_24h_percentage_change")]
        public double Derivatives24hPercentageChange { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("total_market_cap_yesterday")]
        public double TotalMarketCapYesterday { get; set; }

        [JsonPropertyName("total_volume_24h_yesterday")]
        public double TotalVolume24hYesterday { get; set; }

        [JsonPropertyName("total_market_cap_yesterday_percentage_change")]
        public double TotalMarketCapYesterdayPercentageChange { get; set; }

        [JsonPropertyName("total_volume_24h_yesterday_percentage_change")]
        public double TotalVolume24hYesterdayPercentageChange { get; set; }
    }

    public class GlobalQuote
    {
        [JsonPropertyName("USD")]
        public Qoute Qoute { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("active_cryptocurrencies")]
        public int ActiveCryptocurrencies { get; set; }

        [JsonPropertyName("total_cryptocurrencies")]
        public int TotalCryptocurrencies { get; set; }

        [JsonPropertyName("active_market_pairs")]
        public int ActiveMarketPairs { get; set; }

        [JsonPropertyName("active_exchanges")]
        public int ActiveExchanges { get; set; }

        [JsonPropertyName("total_exchanges")]
        public int TotalExchanges { get; set; }

        [JsonPropertyName("eth_dominance")]
        public double EthDominance { get; set; }

        [JsonPropertyName("btc_dominance")]
        public double BtcDominance { get; set; }

        [JsonPropertyName("eth_dominance_yesterday")]
        public double EthDominanceYesterday { get; set; }

        [JsonPropertyName("btc_dominance_yesterday")]
        public double BtcDominanceYesterday { get; set; }

        [JsonPropertyName("eth_dominance_24h_percentage_change")]
        public double EthDominance24hPercentageChange { get; set; }

        [JsonPropertyName("btc_dominance_24h_percentage_change")]
        public double BtcDominance24hPercentageChange { get; set; }

        [JsonPropertyName("defi_volume_24h")]
        public double DefiVolume24h { get; set; }

        [JsonPropertyName("defi_volume_24h_reported")]
        public double DefiVolume24hReported { get; set; }

        [JsonPropertyName("defi_market_cap")]
        public double DefiMarketCap { get; set; }

        [JsonPropertyName("defi_24h_percentage_change")]
        public double Defi24hPercentageChange { get; set; }

        [JsonPropertyName("stablecoin_volume_24h")]
        public double StablecoinVolume24h { get; set; }

        [JsonPropertyName("stablecoin_volume_24h_reported")]
        public double StablecoinVolume24hReported { get; set; }

        [JsonPropertyName("stablecoin_market_cap")]
        public double StablecoinMarketCap { get; set; }

        [JsonPropertyName("stablecoin_24h_percentage_change")]
        public double Stablecoin24hPercentageChange { get; set; }

        [JsonPropertyName("derivatives_volume_24h")]
        public double DerivativesVolume24h { get; set; }

        [JsonPropertyName("derivatives_volume_24h_reported")]
        public double DerivativesVolume24hReported { get; set; }

        [JsonPropertyName("derivatives_24h_percentage_change")]
        public double Derivatives24hPercentageChange { get; set; }

        [JsonPropertyName("quote")]
        public GlobalQuote Quote { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
    
}