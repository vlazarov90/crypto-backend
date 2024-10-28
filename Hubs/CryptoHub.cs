using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace CryptoDashboard.Api.Hubs
{
    [Authorize]
    public class CryptoHub : Hub
    {
        public async Task SendPriceUpdate(string price)
        {
            await Clients.All.SendAsync("ReceivePriceUpdate", price);
        }
    }
}