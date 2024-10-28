using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoDashboard.Api.Hubs;
using Microsoft.AspNetCore.Authorization;

namespace CryptoDashboard.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IHubContext<CryptoHub> _cryptoHubContext;

        public CryptoController(HttpClient httpClient, IHubContext<CryptoHub> cryptoHubContext)
        {
            _httpClient = httpClient;
            _cryptoHubContext = cryptoHubContext;
        }

        // Endpoint to get current Bitcoin prices and push updates to clients
        [HttpGet("prices")]
        public async Task<IActionResult> GetPrices()
        {
            var requestUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve prices");
            }

            var content = await response.Content.ReadAsStringAsync();

            // Instead of sending directly, call the hub method
            await _cryptoHubContext.Clients.All.SendAsync("ReceivePriceUpdate", content);

            return Ok(content);
        }

        // Endpoint to get historical Bitcoin prices and push updates to clients
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var requestUrl = "https://api.coindesk.com/v1/bpi/historical/close.json";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve historical data");
            }

            var content = await response.Content.ReadAsStringAsync();

            // Return the content directly to the client
            return Ok(content);
        }
    }
}