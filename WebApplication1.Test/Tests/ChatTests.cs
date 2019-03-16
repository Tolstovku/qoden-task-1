using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Tests
{
    [Collection("ApiFixture")]
    public class ChatTests
    {
        private HubConnection Hub { get; set; }

        public ChatTests(ApiFixture api)
        {
            Hub = api.Hub;
        }

        const string author = "User";
        const string msg = "Hello world!";

        [Fact]
        public async Task UserCanSendMessage()
        {
            const int roomId = 2;

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
            const int roomId = 42;
            Hub.On<List<string>>("roomMessages",
                (messages) => messages.First().Should().NotBeNull());
            await Hub.StartAsync();
            await Hub.InvokeAsync("joinRoom",  roomId);
            await Hub.StopAsync();
        }
    }
}