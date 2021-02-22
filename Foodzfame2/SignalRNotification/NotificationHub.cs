using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foodzfame2.SignalRNotification
{
    public class NotificationHub:Hub
    {
        public async Task SendMessage(string client, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", client, message);
        }
    }
}
