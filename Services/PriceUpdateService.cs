using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoDashboard.Api.Hubs;

namespace CryptoDashboard.Api.Services  // Ensure you have the correct namespace
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IHubContext<CryptoHub> _cryptoHubContext;
        private readonly HttpClient _httpClient;

        public PriceUpdateService(IHubContext<CryptoHub> cryptoHubContext, HttpClient httpClient)
        {
            _cryptoHubContext = cryptoHubContext;
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendPriceUpdateAsync();
                await Task.Delay(2000, stoppingToken); // Wait for 30 seconds
            }
        }

        private async Task SendPriceUpdateAsync()
        {
            var requestUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                await _cryptoHubContext.Clients.All.SendAsync("ReceivePriceUpdate", content);
            }
            else
            {
                // Handle error
            }
        }
    }
}