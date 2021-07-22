using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.CoinMarketCapReader.Domain.Models.NoSql;
using Service.CoinMarketCapReader.Grpc;
using Service.CoinMarketCapReader.Grpc.Models;
using Service.CoinMarketCapReader.Jobs;
using Service.CoinMarketCapReader.Settings;

namespace Service.CoinMarketCapReader.Services
{
    public class CMCReaderService: ICMCReaderService
    {
        private readonly ILogger<CMCReaderService> _logger;
        private readonly IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> _keyWriter;
        private readonly IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> _marketInfoWriter;
        private readonly CMCUpdateJob _cmcUpdateJob;

        public CMCReaderService(ILogger<CMCReaderService> logger, 
            CMCUpdateJob cmcUpdateJob, 
            IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> marketInfoWriter, 
            IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> keyWriter)
        {
            _logger = logger;
            _cmcUpdateJob = cmcUpdateJob;
            _marketInfoWriter = marketInfoWriter;
            _keyWriter = keyWriter;
        }

        public async Task AddApiKey(ApiKeyRequest request)
        {
            await _keyWriter.InsertOrReplaceAsync(CMCApiKeyNoSqlEntity.Create(request.ApiKey));
            await _cmcUpdateJob.UpdateKeys();
        }

        public async Task RemoveApiKey(ApiKeyRequest request)
        {
            await _keyWriter.DeleteAsync(CMCApiKeyNoSqlEntity.GeneratePartitionKey(),
                CMCApiKeyNoSqlEntity.GenerateRowKey(request.ApiKey));
            await _cmcUpdateJob.UpdateKeys();
        }

        public async Task UpdateMarketInfo(MarketInfoUpdateMessage request)
        {
            var entity = await _marketInfoWriter.GetAsync(MarketInfoNoSqlEntity.GeneratePartitionKey(request.BrokerId),
                MarketInfoNoSqlEntity.GenerateRowKey(request.Asset));

            if (entity != null)
            {
                if (request.Price != 0)
                    entity.MarketInfo.Price = request.Price;
                if (request.Supply != 0)
                    entity.MarketInfo.Supply = request.Supply;      
                if (request.Volume24 != 0)
                    entity.MarketInfo.Volume24 = request.Volume24;
                if (request.MarketCap != 0)
                    entity.MarketInfo.MarketCap = request.MarketCap;
                if (!string.IsNullOrEmpty(request.WhitepaperUrl))
                    entity.MarketInfo.WhitepaperUrl = request.WhitepaperUrl; 
                if (!string.IsNullOrEmpty(request.OfficialWebsiteUrl))
                    entity.MarketInfo.OfficialWebsiteUrl = request.OfficialWebsiteUrl;

                await _marketInfoWriter.InsertOrReplaceAsync(entity);
            }
        }
    }
}
