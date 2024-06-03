using BotMarket2.Common;
using BotMarket2.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BotMarket2.Client.Repositories.StockData
{
    public class StockDataRepo(IHttpClientFactory httpFactory, NavigationManager manager) : IStockDataRepo
    {
        private readonly HttpClient _http = httpFactory.CreateClient();
        private readonly string _hostUrl = manager.BaseUri;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<IEnumerable<HistoricalStockDataDTO>> GetStockDataAll(string symbol)
        {
            var response = await _http.GetAsync($"{_hostUrl}api/StockData/{symbol}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<HistoricalStockDataDTO>>(content) ?? new List<HistoricalStockDataDTO>();
            }
            else
            {
                var error = JsonSerializer.Deserialize<ErrorModelDTO>(await response.Content.ReadAsStringAsync());
                var message = error?.Message;
                if (!string.IsNullOrEmpty(message))
                    throw new ApiException(response.StatusCode, message);
                else
                    throw new ApiException(response);
            }
        }

        public async Task<IEnumerable<HistoricalStockDataDTO>> GetStockData(string symbol, int yrsFromEndDate)
        {
            var response = await _http.GetAsync($"{_hostUrl}api/StockData/{symbol}/{yrsFromEndDate}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<HistoricalStockDataDTO>>(content, jsonSerializerOptions) ?? new List<HistoricalStockDataDTO>();
            }
            else
            {
                var error = JsonSerializer.Deserialize<ErrorModelDTO>(await response.Content.ReadAsStringAsync());
                var message = error?.Message;
                if (!string.IsNullOrEmpty(message))
                    throw new ApiException(response.StatusCode, message);
                else
                    throw new ApiException(response);
            }
        }
    }
}
