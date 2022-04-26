using AutoMapper;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends;

public class FriendsService
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public FriendsService(DataContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
}