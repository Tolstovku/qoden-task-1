using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using Tests.Fixtures;
using Xunit;

namespace Tests.Tests
{
    [Collection("ApiFixture")]
    public class ChatTests
    {
        private HubConnection Hub { get; set; }

        public ChatTests(ApiFixture api)
        {
            Hub = api.Hub;
        }

        private const int roomId = 42;
        const string author = "User";
        const string msg = "Hello world!";

        [Fact]
        public async Task UserCanSendMessage()
        {
            Hub.On<string>("sendMessage",
                (msgWithAuthor) => msgWithAuthor.Should().BeEquivalentTo($"{author}: {msg}\n"));

            await Hub.StartAsync();
            await Hub.InvokeAsync("joinRoom", roomId);
            await Hub.InvokeAsync("sendMessage", msg, roomId);
            await Hub.StopAsync();
        }

        [Fact]
        public async Task UserCannotSendMessageToRoomHeIsNotIn()
        {
            try
            {
                await Hub.InvokeAsync("sendMessage", msg, -100);
            }
            catch (Exception ex)
            {
                ex.Message.Should().NotBeNullOrEmpty();
                await Hub.StopAsync();
            }
        }

        [Fact]
        public async Task UserCanGetAllPreviousRoomMessages()
        {
            Hub.On<HashSet<string>>("roomMessages",
                (messages) => messages.First().Should().NotBeNull());
            await Hub.StartAsync();
            await Hub.InvokeAsync("joinRoom",  roomId);
            await Hub.StopAsync();
        }

        [Fact]
        public async Task UserCanGetAvailableRooms()
        {
            Hub.On<HashSet<string>>("getAvailableRooms",
                (rooms) => rooms.First().Should().Be("42"));
            await Hub.StartAsync();
            await Hub.InvokeAsync("joinRoom",  roomId);
            await Hub.InvokeAsync("getAvailableRooms");
            await Hub.StopAsync();
        }

        [Fact]
        public async Task UserCanGetUsersInRoom()
        {
            Hub.On<HashSet<string>>("getUsersInRoom",
                (users) => users.First().Should().Be("User"));
            await Hub.StartAsync();
            await Hub.InvokeAsync("joinRoom",  roomId);
            await Hub.InvokeAsync("getUsersInRoom", roomId);
            await Hub.StopAsync();
        }

    }
}