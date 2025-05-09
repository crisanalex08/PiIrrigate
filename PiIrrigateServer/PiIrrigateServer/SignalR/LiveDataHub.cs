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

        // Clients join a group based on ZoneId
        public async Task JoinZoneGroup(Guid zoneId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, zoneId.ToString());
        }

        // Send live data to a specific zone group
        public async Task SendLiveDataToZone(SensorReading data)
        {
            await hubContext.Clients.Group(data.ZoneId.ToString()).SendAsync("ReceiveLiveData", data);
        }
    }

}
