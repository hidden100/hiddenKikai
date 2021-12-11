using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shinhayasa.Hubs
{
    public class GameHUB: Hub
    {

        public async Task Test(string user)
        {
            await Clients.All.SendAsync("Test", user);
        }
    }
}
