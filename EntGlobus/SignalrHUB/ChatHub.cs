using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.SignalrHUB
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string Number, string Message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Number, Message);
        }
    }
}
