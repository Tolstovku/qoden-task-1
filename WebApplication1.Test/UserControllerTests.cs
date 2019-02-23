using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using WebApplication1.Database.Entities;
using WebApplication1.Requests;
using WebApplication1.Responses;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class UserControllerTests : IClassFixture<ApiFixture>
    {
        private ApiFixture Api { get; set; }
        private readonly ITestOutputHelper _testOutputHelper;

        public UserControllerTests(ApiFixture api, ITestOutputHelper testOutputHelper)
        {
            Api = api;
            _testOutputHelper = testOutputHelper;
        }


        [Theory]
        [InlineData(123)]
        [InlineData(124)]
        [InlineData(125)]
        public async Task UserCanGetExistingUserProfile(int id)
        {
            var response = await Api.RegularUser.GetAsync($"api/v1/user/{id}");
            var body = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileResponse>(body);

            response.StatusCode.Should().BeEquivalentTo(200);
            profile.Email.Should().NotBeNullOrEmpty();
        }


        [Theory]
        [InlineData(-10)]
        [InlineData(10000)]
        public async Task UserCannotGetNonExistingUserProfile(int id)
        {
            var response = await Api.RegularUser.GetAsync($"api/v1/user/{id}");
            var body = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileResponse>(body);

            response.StatusCode.Should().BeEquivalentTo(400);
        }

        [Fact]
        public async Task AdminCanModifyUser()
        {
            var req = new ModifyUserRequest
            {
                FirstName = "Test", Lastname = "Test", Description = "Test", Email = "Test@email.ru",
                NickName = "Test", Patronymic = "Test", PhoneNumber = "79119394404"
            };
            const int id = 124;

            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await Api.AdminUser.PutAsJsonAsync($"api/v1/user/{id}", req);

                response.StatusCode.Should().BeEquivalentTo(200);
                var updatedUser = Api.Db.Users.Single(u => u.Id == id);
                updatedUser.NickName.Should().Be(req.NickName);
                updatedUser.Email.Should().Be(req.Email);
                updatedUser.FirstName.Should().Be(req.FirstName);
                updatedUser.Lastname.Should().Be(req.Lastname);
                updatedUser.Description.Should().Be(req.Description);
                updatedUser.Patronymic.Should().Be(req.Patronymic);
                updatedUser.PhoneNumber.Should().Be(req.PhoneNumber);
            }
        }

        [Fact]
        public async Task AdminCanAssignManager()
        {
            await CheckAssigning(true);
        }

        [Fact]
        public async Task AdminCanUnAssignManager()
        {
            await CheckAssigning(false);
        }
        
        [Fact]
        public async Task AdminCanCreateUser()
        {
            var req = new CreateUserRequest
            {
                FirstName = "Test", Lastname = "Test", Description = "Test", Email = "Test@email.ru",
                NickName = "Test", Patronymic = "Test", PhoneNumber = "79119394404", DepartmentId = -1,
                Password = "1234567890", UserRoleId = -3
            };
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfUsersBefore = Api.Db.Users.Count();
                var response = await Api.AdminUser.PostAsJsonAsync("api/v1/user", req);
                
                response.StatusCode.Should().BeEquivalentTo(200);
                var amountOfUsersAfter = Api.Db.Users.Count();
                amountOfUsersAfter.Should().BeGreaterThan(amountOfUsersBefore);
                var user = Api.Db.Users.Single(u => u.Email==req.Email && u.FirstName==req.FirstName &&
                                                    u.Lastname==req.FirstName && u.NickName==req.NickName &&
                                                    u.DepartmentId==req.DepartmentId && u.UserRoleId==req.UserRoleId);
                user.Should().NotBeNull();
            }
        }

        private async Task CheckAssigning(bool doAssign)
        {
            var endPath = doAssign ? "assign" : "unassign";

            var req = new AssignManagerRequest
            {
                ManagerId = 125,
                UserId = doAssign ? 123 : 124
            };
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfUserManagerRelationshipsBefore = Api.Db.UserManagers.Count();
                var response = await Api.AdminUser.PostAsJsonAsync($"api/v1/user/{endPath}", req);
                _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

                response.StatusCode.Should().BeEquivalentTo(200);
                var amountOfUserManagerRelationshipsAfter = Api.Db.UserManagers.Count();
                if (doAssign)
                {
                    amountOfUserManagerRelationshipsAfter.Should()
                        .BeGreaterThan(amountOfUserManagerRelationshipsBefore);
                }
                else
                {
                    amountOfUserManagerRelationshipsAfter.Should().BeLessThan(amountOfUserManagerRelationshipsBefore);
                }
            }
        }
        
        
    }
}