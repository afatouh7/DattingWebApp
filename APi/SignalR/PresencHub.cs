using APi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace APi.SignalR
{
    [Authorize]
    public class PresencHub :Hub
    {
        private readonly PresenceTracker _tracker;

        public PresencHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.GetUsername(),Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            var currentUSers= await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUSers);
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline",Context.User.GetUsername());
            var currentUSers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUSers);

            await base.OnDisconnectedAsync(ex);
        }
    }
}
