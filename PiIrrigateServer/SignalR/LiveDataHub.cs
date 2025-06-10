using Microsoft.AspNetCore.SignalR;
using PiIrrigateServer.Models;
using System.Collections.Concurrent;

namespace PiIrrigateServer.SignalR
{
    public class LiveDataHub : Hub
    {
        private readonly IHubContext<LiveDataHub> hubContext;

        public LiveDataHub(IHubContext<LiveDataHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        private static readonly ConcurrentDictionary<string, string> zoneIdLiveData = new ConcurrentDictionary<string, string>();

        public async Task CreateZoneGroup()
        {
            var zoneId = Guid.NewGuid().ToString();
            // Add the connection to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, zoneId);
            // Store the connection ID and zone ID in the dictionary
            zoneIdLiveData.TryAdd(Context.ConnectionId, zoneId);
            await Clients.Caller.SendAsync("ZoneGroupCreated", zoneId);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("Method1", $"{Context.ConnectionId} has joined");
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
