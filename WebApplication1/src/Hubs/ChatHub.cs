using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Qoden.Util;

namespace WebApplication1.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, List<string>> UserInRooms = new ConcurrentDictionary<string, List<string>>();
        private static ConcurrentDictionary<string, List<string>> MessagesInRoom = new ConcurrentDictionary<string, List<string>>();

        public override async Task OnConnectedAsync()
        {
            var username = UserInfoProvider.GetUsername(Context);

            var roomsOfUser = UserInRooms.GetOrAdd(username, new List<string>());
            foreach (var roomId in roomsOfUser)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await SendRoomMessagesToCaller(roomId);
            }

            await base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var hasJoinedMsg = $"{username} has joined the room.\n";
            var connId = Context.ConnectionId;

            var roomsOfUser = UserInRooms.GetValue(username);
            if (roomsOfUser.Count == 10)
            {
                await Clients.Caller.SendAsync("roomLimitExceeded");
                return;
            }

            roomsOfUser.Add(roomId);
            await Groups.AddToGroupAsync(connId, roomId);
            await Clients.Group(roomId).SendAsync("sendMessage", hasJoinedMsg);
            await SendRoomMessagesToCaller(roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var hasLeftMsg = $"{username} has left the room.\n";
            var connId = Context.ConnectionId;

            UserInRooms.GetValue(username).Remove(roomId);
            await Groups.RemoveFromGroupAsync(connId, roomId);
            await Clients.Group(roomId).SendAsync("sendMessage", hasLeftMsg);
        }

        public async Task SendMessage(string msg, string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var msgWithAuthor = $"{username}: {msg}\n";
            var connId = Context.ConnectionId;

            await Clients.Group(roomId).SendAsync("sendMessage", msgWithAuthor);
            MessagesInRoom.GetValue(roomId).Add(msg);
        }

        private async Task SendRoomMessagesToCaller(string roomId)
        {
            var roomMessages = MessagesInRoom.GetOrAdd(roomId, new List<string>());
            if (roomMessages.Count>0)
            {
                await Clients.Caller.SendAsync("roomMessages", roomMessages);
            }
        }
    }
}
