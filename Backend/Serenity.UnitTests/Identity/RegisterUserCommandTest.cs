using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Serenity.Common;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;
using Serenity.Modules.Identity.Handlers;
using Xunit;
using FluentAssertions;

public class RegisterUserCommandTest
{
    [Fact]
    public async void RegisterUser_WithDto_ReturnsSuccess()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
        var mapper = config.CreateMapper();

        var store = new Mock<IUserStore<User>>();
        var manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        manager.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        var handler = new RegisterUserCommandHandler(manager.Object, mapper);

        var dto = new RegisterUserDto()
        {

        };

        var result = await handler.Handle(new RegisterUserCommand(dto), new CancellationToken());
        result.Success.Should().BeTrue();
    }
}