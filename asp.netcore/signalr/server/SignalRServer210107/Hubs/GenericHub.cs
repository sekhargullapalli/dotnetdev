using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRServer210107.Hubs 
{
    public class GenericHub:Hub 
    {
        public async Task SendMessage(string user, string message, bool logatserver)
        {
            if(logatserver)
            {
                string stamp = $"> {DateTime.Now.ToString("G")}: ";
                await Clients.All.SendAsync("payloadLogMessage", stamp, $"{user} ", message);
            }
            await Clients.All.SendAsync("broadcastMessage", user, message);
        }
        

       



    }

}