using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service.Tools;
using System.Text.Json;
using MyNoSqlServer.Abstractions;
using Service.AssetsDictionary.Client;
using Service.AssetsDictionary.Domain.Models;
using Service.CoinMarketCapReader.Domain.Models;
using Service.CoinMarketCapReader.Domain.Models.NoSql;

namespace Service.CoinMarketCapReader.Jobs
{
    public class CMCUpdateJob : IStartable, IDisposable
    { 
        private const string CoinInfoUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?symbol=";
        private const string CoinMarketUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";
        private readonly MyTaskTimer _marketInfoTimer;
        private readonly MyTaskTimer _coinInfoTimer;
        private readonly MyTaskTimer _apiKeyTimer;

        private readonly HttpClient _client;
        private readonly ILogger<CMCUpdateJob> _logger;
        private readonly IAssetsDictionaryClient _assetClient;
        private readonly IMarketReferenceDictionaryClient _referenceDictionary;
        private readonly IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> _keyWriter;
        private readonly IMyNoSqlServerDataWriter<CMCTimerNoSqlEntity> _timerWriter;
        private readonly IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> _marketInfoWriter;
        private Dictionary<string, bool> _apiKeys = new();
        public CMCUpdateJob(ILogger<CMCUpdateJob> logger, 
            IAssetsDictionaryClient assetClient, 
            IMarketReferenceDictionaryClient referenceDictionaryClient,
            IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> keyWriter, IMyNoSqlServerDataWriter<CMCTimerNoSqlEntity> timerWriter, IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> marketInfoWriter, IMarketReferenceDictionaryClient referenceDictionary)
        {
            _logger = logger;
            _assetClient = assetClient;
            _keyWriter = keyWriter;
            _timerWriter = timerWriter;
            _marketInfoWriter = marketInfoWriter;
            _referenceDictionary = referenceDictionary;
            _marketInfoTimer = new MyTaskTimer(nameof(CMCUpdateJob), TimeSpan.FromSeconds(Program.Settings.MarketInfoTimerInSec), _logger, UpdateMarketInfo).DisableTelemetry();
            _coinInfoTimer = new MyTaskTimer(nameof(CMCUpdateJob), TimeSpan.FromMinutes(Program.Settings.CoinInfoTimerInMin), _logger, UpdateCoinInfo).DisableTelemetry();
            _apiKeyTimer = new MyTaskTimer(nameof(CMCUpdateJob), TimeSpan.FromDays(1), _logger, ResetAllKeys).DisableTelemetry();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        private async Task UpdateCoinInfo()
        {
            _logger.LogInformation("Starting UpdateCoinInfo");
            
            var infos = await _marketInfoWriter.GetAsync();

            var marketInfos =
                infos.ToDictionary(
                    entity => entity.MarketInfo.Asset,
                    entity => new List<MarketInfo>() {entity.MarketInfo});
            
            var assets = _referenceDictionary.GetAllMarketReferences().Where(market=>market.Type == MarketType.Crypto);
            if (assets.Any())
            {
                var link = CoinInfoUrl;
                foreach (var asset in assets)
                {
                    var cmcAsset = asset.AssociateAsset.Replace("test", "");
                    if (!link.EndsWith('='))
                        link += ',';
                    link += cmcAsset;

                    if (!marketInfos.ContainsKey(cmcAsset))
                        marketInfos[cmcAsset] = new List<MarketInfo>();
                    
                    marketInfos[cmcAsset].Add(new MarketInfo()
                    {
                        Asset = asset.AssociateAsset,
                        BrokerId = asset.BrokerId
                    });
                }

                var response = await GetRequest<CMCCoinInfoResponse>(link);
                
                foreach (var (asset, info)  in response.Data)
                {
                    if (marketInfos.TryGetValue(asset, out var marketInfoList))
                    {
                        foreach (var marketInfo in marketInfoList)
                        {
                            if (info.Urls.Website.Any())
                                marketInfo.OfficialWebsiteUrl = info.Urls.Website.First();
                            if (info.Urls.TechnicalDoc.Any())
                                marketInfo.WhitepaperUrl = info.Urls.TechnicalDoc.First();

                            await _marketInfoWriter.InsertOrReplaceAsync(MarketInfoNoSqlEntity.Create(marketInfo));
                        }
                    }
                }
            }
            _logger.LogInformation("Finished UpdateCoinInfo");
        }

        private async Task UpdateMarketInfo()
        {
            _logger.LogInformation("Starting UpdateMarketInfo");
            
            var marketInfos =
                (await _marketInfoWriter.GetAsync()).ToDictionary(entity => entity.MarketInfo.Asset,
                    entity => entity.MarketInfo);
            
            var response = await GetRequest<CMCMarketInfoResponse>(CoinMarketUrl);
            
            foreach (var (asset, marketInfo) in marketInfos)
            {
                var info = response.Data.FirstOrDefault(p => p.Symbol == asset);
                if (info != null)
                {
                    marketInfo.Price = info.Quote.QuoteValues.Price;
                    marketInfo.Supply = info.TotalSupply;
                    marketInfo.MarketCap = info.Quote.QuoteValues.MarketCap;
                    marketInfo.Volume24 = info.Quote.QuoteValues.Volume24;
                    await _marketInfoWriter.InsertOrReplaceAsync(MarketInfoNoSqlEntity.Create(marketInfo));
                }
            }
            
            _logger.LogInformation("Finished UpdateMarketInfo");
        }

        public async Task UpdateKeys()
        {
            var keys = await _keyWriter.GetAsync();
            if (keys.Any())
                _apiKeys = keys.ToDictionary(key=> key.ApiKey, value => true);
        }
        
        private async Task SetApiKeys()
        {
            var keys = await _keyWriter.GetAsync();
            if (keys.Any())
                _apiKeys = keys.ToDictionary(key=> key.ApiKey, value => true);
            else
            {
                _apiKeys = new Dictionary<string, bool>()
                {
                    {Program.Settings.CoinMarketCapApiKey, true}
                };
            }
            _client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY",_apiKeys.First(pair => pair.Value).Key);
        }

        private Task SetNextKey()
        {
            var failedKey = _apiKeys.First(pair => pair.Value == true);
            _apiKeys[failedKey.Key] = false;
            
            if (_apiKeys.All(pair => pair.Value == false))
            {
                _logger.LogError("No working CoinMarketCap Api Keys left");
                throw new Exception("No working CoinMarketCap Api Keys left");
            }
            
            _client.DefaultRequestHeaders.Remove("X-CMC_PRO_API_KEY");
            _client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY",_apiKeys.First(pair => pair.Value).Key);
            _logger.LogInformation("CMC Api key {key} failed",failedKey.Key);
            
            return Task.CompletedTask;
        }

        private Task ResetAllKeys()
        {
            foreach (var apiKey in _apiKeys)
            {
                _apiKeys[apiKey.Key] = true;
            }
            
            return Task.CompletedTask;
        }
        
        public async void Start()
        {
            await SetApiKeys();
            _marketInfoTimer.Start();
            _coinInfoTimer.Start();
            _apiKeyTimer.Start();
        }

        public void Dispose()
        {
            _marketInfoTimer.Dispose();
            _coinInfoTimer.Start();
            _apiKeyTimer.Dispose();
        }

        private async Task<T> GetRequest<T>(string uri)
        {
            try
            {
                using HttpResponseMessage response = await _client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(responseBody);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When getting CoinMarketCap info");
                await SetNextKey();
                throw;
            }
        }
    }
}