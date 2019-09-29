using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PoliticsRisk
{
    public class GameHub : Hub
    {
        public Task ShowAllOnlinePlayers(string player)
        {
            return Clients.All.SendAsync("List of online player", player);
        }
    }
}