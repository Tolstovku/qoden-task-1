using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Qoden.Util;
using Qoden.Validation;

namespace WebApplication1.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, HashSet<string>> _usersOfRoom = new ConcurrentDictionary<string, HashSet<string>>();
        private static ConcurrentDictionary<string, HashSet<string>> _userInRooms = new ConcurrentDictionary<string, HashSet<string>>();
        private static ConcurrentDictionary<string, HashSet<string>> _messagesInRoom = new ConcurrentDictionary<string, HashSet<string>>();

        public override async Task OnConnectedAsync()
        {
            var username = UserInfoProvider.GetUsername(Context);

            var roomsOfUser = _userInRooms.GetOrAdd(username, new HashSet<string>());
            foreach (var roomId in roomsOfUser)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await SendAllRoomMessagesToCaller(roomId);
            }

            await base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var hasJoinedMsg = $"{username} has joined the room.\n";
            var connId = Context.ConnectionId;

            var roomsOfUser = _userInRooms.GetValueOrDefault(username);
            if (roomsOfUser.Count == 50)
            {
                await Clients.Caller.SendAsync("roomLimitExceeded");
                return;
            }

            roomsOfUser.Add(roomId);
            _usersOfRoom.GetOrAdd(roomId, new HashSet<string>()).Add(username);
            await Groups.AddToGroupAsync(connId, roomId);
            await Clients.Group(roomId).SendAsync("sendMessage", hasJoinedMsg);
            await SendAllRoomMessagesToCaller(roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var hasLeftMsg = $"{username} has left the room.\n";
            var connId = Context.ConnectionId;

            var usersRooms = _userInRooms.GetValueOrDefault(username);
            Check.Value(usersRooms.Contains(roomId)).IsTrue("User is not present in specified room");
            usersRooms.Remove(roomId);
            _usersOfRoom.GetValueOrDefault(roomId).Remove(username);
            await Groups.RemoveFromGroupAsync(connId, roomId);
            await Clients.Group(roomId).SendAsync("sendMessage", hasLeftMsg);
        }

        public async Task SendMessage(string msg, string roomId)
        {
            var username = UserInfoProvider.GetUsername(Context);
            var msgWithAuthor = $"{username}: {msg}\n";
            Check.Value(_userInRooms[username].Contains(roomId)).EqualsTo(true, "User is not present in specified room");

            await Clients.Group(roomId).SendAsync("sendMessage", msgWithAuthor);
            _messagesInRoom.GetValueOrDefault(roomId).Add(msg);
        }

        public async Task GetAvailableRooms()
        {
            var availableRooms = new List<string>();
            foreach (var pair in _usersOfRoom)
            {
                availableRooms.Add(pair.Key);
            }
            await Clients.Caller.SendAsync("getAvailableRooms", availableRooms);
        }

        public async Task GetUsersInRoom(string roomId)
        {
            var users = _usersOfRoom.GetOrAdd(roomId, new HashSet<string>());
            await Clients.Caller.SendAsync("getUsersInRoom", users);
        }

        private async Task SendAllRoomMessagesToCaller(string roomId)
        {
            var roomMessages = _messagesInRoom.GetOrAdd(roomId, new HashSet<string>());
            if (roomMessages.Count>0)
            {
                await Clients.Caller.SendAsync("roomMessages", roomMessages);
            }
        }
    }
}
