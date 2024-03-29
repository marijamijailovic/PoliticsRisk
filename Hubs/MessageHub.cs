using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PoliticsRisk
{
    public class MessageHub : Hub
    {
        public Task SendMessageToAll(string player, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage",player, message);
        }
    }
}