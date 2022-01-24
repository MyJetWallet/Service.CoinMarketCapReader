using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.CoinMarketCapReader.Domain.Models;
using Service.CoinMarketCapReader.Domain.Models.NoSql;
using Service.CoinMarketCapReader.Grpc;
using Service.CoinMarketCapReader.Grpc.Models;
using Service.CoinMarketCapReader.Jobs;
using Service.CoinMarketCapReader.Settings;
using Service.MessageTemplates.Client;
using Service.MessageTemplates.Domain.Models;
using Service.MessageTemplates.Grpc;

namespace Service.CoinMarketCapReader.Services
{
    public class CMCReaderService: ICMCReaderService
    {
        private readonly ILogger<CMCReaderService> _logger;
        private readonly IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> _keyWriter;
        private readonly IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> _marketInfoWriter;
        private readonly CMCUpdateJob _cmcUpdateJob;
        private readonly ITemplateService _templateService;

        public CMCReaderService(ILogger<CMCReaderService> logger, 
            CMCUpdateJob cmcUpdateJob, 
            IMyNoSqlServerDataWriter<MarketInfoNoSqlEntity> marketInfoWriter, 
            IMyNoSqlServerDataWriter<CMCApiKeyNoSqlEntity> keyWriter, 
            ITemplateService templateService)
        {
            _logger = logger;
            _cmcUpdateJob = cmcUpdateJob;
            _marketInfoWriter = marketInfoWriter;
            _keyWriter = keyWriter;
            _templateService = templateService;
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
                if (!string.IsNullOrEmpty(request.AboutLessTemplateId))
                {
                    entity.MarketInfo.AboutLessTemplateId = request.AboutLessTemplateId;
                    await EnsureTemplateAreCreated(request.AboutLessTemplateId);
                }

                if (!string.IsNullOrEmpty(request.AboutMoreTemplateId))
                {
                    entity.MarketInfo.AboutMoreTemplateId = request.AboutMoreTemplateId;
                    await EnsureTemplateAreCreated(request.AboutMoreTemplateId);
                }
                
                if (string.IsNullOrEmpty(entity.MarketInfo.AboutLessTemplateId))
                {
                    var aboutLess = $"{request.Asset}-about-less".ToLower();
                    entity.MarketInfo.AboutLessTemplateId = aboutLess;
                    await EnsureTemplateAreCreated(aboutLess);
                }
                
                if (string.IsNullOrEmpty(entity.MarketInfo.AboutMoreTemplateId))
                {
                    var aboutMore = $"{request.Asset}-about-more".ToLower();
                    entity.MarketInfo.AboutMoreTemplateId = aboutMore;
                    await EnsureTemplateAreCreated(aboutMore);
                }

                await _marketInfoWriter.InsertOrReplaceAsync(entity);
            }
        }

        public async Task<OperationResponse> CreateMarketInfo(MarketInfoUpdateMessage request)
        {
            try
            {
                var aboutLess = $"{request.Asset}-about-less".ToLower();
                var aboutMore = $"{request.Asset}-about-more".ToLower();

                var entity = new MarketInfo
                {
                    Asset = request.Asset,
                    BrokerId = request.BrokerId,
                    WhitepaperUrl = request.WhitepaperUrl,
                    OfficialWebsiteUrl = request.OfficialWebsiteUrl,
                    MarketCap = request.MarketCap,
                    Supply = request.Supply,
                    Volume24 = request.Volume24,
                    Price = request.Price,
                    AboutLessTemplateId = aboutLess,
                    AboutMoreTemplateId = aboutMore
                };

                await _marketInfoWriter.InsertOrReplaceAsync(MarketInfoNoSqlEntity.Create(entity));
                await EnsureTemplateAreCreated(aboutLess);
                await EnsureTemplateAreCreated(aboutMore);

                return new OperationResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When adding market info for asset {asset}", request.Asset);
                return new OperationResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<OperationResponse> DeleteMarketInfo(MarketInfoDeleteRequest request)
        {
            var entity = await _marketInfoWriter.GetAsync(MarketInfoNoSqlEntity.GeneratePartitionKey(request.BrokerId),
                MarketInfoNoSqlEntity.GenerateRowKey(request.Asset));
            if (entity != null)
            {
                await _marketInfoWriter.DeleteAsync(entity.PartitionKey, entity.RowKey);
                return new OperationResponse()
                {
                    IsSuccess = true
                };
            }

            return new OperationResponse()
            {
                IsSuccess = false,
                ErrorMessage = "Info entity not found"
            };
        }

        private async Task EnsureTemplateAreCreated(string templateId)
        {
            var templates = await _templateService.GetAllTemplates();
            if (templates.Templates.All(t => t.TemplateId != templateId))
            {
                await _templateService.CreateNewTemplate(new Template()
                {
                    TemplateId = templateId,
                    DefaultBrand = "Simple",
                    DefaultLang = "En"
                });
            }
        }
    }
}
