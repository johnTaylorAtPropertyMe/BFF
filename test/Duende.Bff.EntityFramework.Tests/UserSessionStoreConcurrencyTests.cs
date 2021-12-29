// // Copyright (c) Duende Software. All rights reserved.
// // See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Duende.Bff.EntityFramework.Tests
{
    public class UserSessionStoreConcurrencyTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly SessionDbContext _db;
        private readonly IUserSessionStore _subject;

        public UserSessionStoreConcurrencyTests()
        {
            var services = new ServiceCollection();

            var testScopedDbName = Guid.NewGuid().ToString();
            services.AddBff()
                .AddEntityFrameworkServerSideSessions(options => options.UseInMemoryDatabase(testScopedDbName));

            _serviceProvider = services.BuildServiceProvider();
            _db = _serviceProvider.GetRequiredService<SessionDbContext>();
            _subject = _serviceProvider.GetRequiredService<IUserSessionStore>();
        }

        [Fact]
        public async Task SanityCheck()
        {
            EnsureThatNoUserSessionsExist();

            using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var scopedDb = scope.ServiceProvider.GetRequiredService<SessionDbContext>();

            await _subject.CreateUserSessionAsync(GenerateUserSession());

            _db.UserSessions.Count().Should().Be(1);
            scopedDb.UserSessions.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeleteUserSessionAsync_CalledConcurrentlyForValidKey_Should_Succeed()
        {
            EnsureThatNoUserSessionsExist();

            var userSession = GenerateUserSession();

            await _subject.CreateUserSessionAsync(userSession);

            Action act = () => Parallel.Invoke(
                () =>
                {
                    using var requestScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var subject = requestScope.ServiceProvider.GetRequiredService<IUserSessionStore>();

                    subject.DeleteUserSessionAsync(userSession.Key).GetAwaiter().GetResult();
                },
                () =>
                {
                    using var requestScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var subject = requestScope.ServiceProvider.GetRequiredService<IUserSessionStore>();

                    subject.DeleteUserSessionAsync(userSession.Key).GetAwaiter().GetResult();
                }
            );

            act.Should().NotThrow<DbUpdateConcurrencyException>();

            EnsureThatNoUserSessionsExist();
        }

        private void EnsureThatNoUserSessionsExist()
        {
            _db.UserSessions.Count().Should().Be(0);
        }

        private static UserSession GenerateUserSession()
        {
            return new UserSession
            {
                Key = "key123",
                SessionId = "sid",
                SubjectId = "sub",
                Created = new DateTime(2020, 3, 1, 9, 12, 33, DateTimeKind.Utc),
                Renewed = new DateTime(2021, 4, 2, 10, 13, 34, DateTimeKind.Utc),
                Expires = new DateTime(2022, 5, 3, 11, 14, 35, DateTimeKind.Utc),
                Ticket = "ticket"
            };
        }
    }
}