using Microsoft.AspNetCore.SignalR;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.SignalR
{
    public class LiveDataHub : Hub
    {
        private readonly IHubContext<LiveDataHub> hubContext;

        public LiveDataHub(IHubContext<LiveDataHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task SendLiveData(SensorReading data)
        {
            await hubContext.Clients.All.SendAsync("ReceiveLiveData", data);
        }
    }
}
