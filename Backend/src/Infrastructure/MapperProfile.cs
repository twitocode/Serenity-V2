using Application.Dtos.Auth;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForAllMembers(opt => opt.NullSubstitute(""));
    }
}