using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Qoden.Validation;
using Tests.Fixtures;
using WebApplication1.Database.Entities;
using WebApplication1.Database.Repositories;
using WebApplication1.Requests;
using WebApplication1.Responses;
using Xunit;
using Xunit.Abstractions;
using static WebApplication1.Database.Schemas.UserSchema;

namespace Tests
{
    [Collection("ApiFixture")]
    public class UserControllerTests
    {
        private ApiFixture Api { get; set; }
        private readonly ITestOutputHelper _testOutputHelper;

        public UserControllerTests(ApiFixture api, ITestOutputHelper testOutputHelper)
        {
            Api = api;
            _testOutputHelper = testOutputHelper;
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
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
            const int id = 1;

            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await Api.AdminUser.PutAsJsonAsync($"api/v1/user/{id}", req);

                response.StatusCode.Should().BeEquivalentTo(200);

                var updatedUser = await Api.ConnectionFactory.GetUserById(id);
                updatedUser.NickName.Should().Be(req.NickName);
                updatedUser.Email.Should().Be(req.Email);
                updatedUser.FirstName.Should().Be(req.FirstName);
                updatedUser.Lastname.Should().Be(req.Lastname);
                updatedUser.Description.Should().Be(req.Description);
                updatedUser.Patronymic.Should().Be(req.Patronymic);
                updatedUser.PhoneNumber.Should().Be(req.PhoneNumber);
            }
        }

        [Theory]
        [InlineData("DefinitelyNotAnEmail" )]

        public async Task AdminCannotModifyUserWithInvalidEmail(string email)
        {
            var req = new ModifyUserRequest
            {
                 Email = email
            };
            const int id = 124;
            var response = await Api.AdminUser.PutAsJsonAsync($"api/v1/user/{id}", req);
            response.StatusCode.Should().Be(400);
        }

        [Theory]
        [InlineData(-100)]
        public async Task AdminCannotModifyNonExistingUser(int id)
        {
            var req = new ModifyUserRequest
            {
                FirstName = "Test"
            };
            var response = await Api.AdminUser.PutAsJsonAsync($"api/v1/user/{id}", req);
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task AdminCanAssignManager()
        {
            await CheckAssigning(true, 1, 3, false);
        }

        [Fact]
        public async Task AdminCanUnAssignManager()
        {
            await CheckAssigning(false, 2, 3, false);
        }

        [Theory]
        [InlineData(-100, 125)]
        [InlineData(123, -100)]
        public async Task AdminCannotAssignManagerDueToInvalidIds(int userId, int managerId)
        {
            await CheckAssigning(true, userId, managerId, true);

        }

        [Fact]
        public async Task AdminCanCreateUser()
        {
            var req = new CreateUserRequest
            {
                FirstName = "Test", Lastname = "Test", Description = "Test", Email = "Test@email.ru",
                NickName = "Test", Patronymic = "Test", PhoneNumber = "79119394404", DepartmentId = 1,
                Password = "1234567890"
            };
                var amountOfUsersBefore = (await Api.ConnectionFactory.GetAllUsers()).Count();
                var response = await Api.AdminUser.PostAsJsonAsync("api/v1/user", req);

                response.StatusCode.Should().BeEquivalentTo(200);
                var usersAfter = await Api.ConnectionFactory.GetAllUsers();
                usersAfter.Count.Should().BeGreaterThan(amountOfUsersBefore);
                var user = usersAfter.Single(u => u.Email==req.Email && u.FirstName==req.FirstName &&
                                                    u.Lastname==req.FirstName && u.NickName==req.NickName &&
                                                    u.DepartmentId==req.DepartmentId);
                user.Should().NotBeNull();
        }

        private async Task CheckAssigning(bool doAssign, int userId, int managerId, bool shouldFail)
        {
            var endPath = doAssign ? "assign" : "unassign";

            var req = new AssignManagerRequest
            {
                ManagerId = managerId,
                UserId = userId
            };
            using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var amountOfUserManagerRelationshipsBefore = (await Api.ConnectionFactory.GetAllUserManagers()).Count();
                var response = await Api.AdminUser.PostAsJsonAsync($"api/v1/user/{endPath}", req);
                var amountOfUserManagerRelationshipsAfter = (await Api.ConnectionFactory.GetAllUserManagers()).Count();

                if (!shouldFail)
                {
                    response.StatusCode.Should().BeEquivalentTo(200);
                    if (doAssign)
                    {
                        amountOfUserManagerRelationshipsAfter.Should()
                            .BeGreaterThan(amountOfUserManagerRelationshipsBefore);
                    }
                    else
                    {
                        amountOfUserManagerRelationshipsAfter.Should()
                            .BeLessThan(amountOfUserManagerRelationshipsBefore);
                    }
                }
                else
                {
                    response.StatusCode.Should().BeEquivalentTo(400);
                    amountOfUserManagerRelationshipsBefore.Should().Be(amountOfUserManagerRelationshipsAfter);
                }
            }
        }


    }
}