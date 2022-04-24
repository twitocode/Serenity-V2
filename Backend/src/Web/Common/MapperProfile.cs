using AutoMapper;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;
using Serenity.Modules.Identity.Dto;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Common;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(x => x.Bio, x => x.NullSubstitute(""))
            .ForMember(x => x.Avatar, x => x.NullSubstitute(""))
            .ForMember(x => x.DiscordProfile, x => x.NullSubstitute(""))
            .ForMember(x => x.InstagramProfile, x => x.NullSubstitute(""))
            .ForMember(x => x.FollowedTags, x => x.NullSubstitute(new List<string>()));

        CreateMap<CreatePostDto, Post>()
            .ForMember(x => x.Title, x => x.DoNotAllowNull())
            .ForMember(x => x.Content, x => x.DoNotAllowNull())
            .ForMember(x => x.Tags, x => x.NullSubstitute(new List<string>()));

        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, Comment>();
    }
}
