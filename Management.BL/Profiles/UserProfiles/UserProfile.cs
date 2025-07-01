using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDTO, AppUser>().ReverseMap();
        CreateMap<UserUpdateDTO, AppUser>().ReverseMap();
        CreateMap<AppUser, UserListItemDTO>();
        CreateMap<AppUser, UserTableItemDTO>();
    }
}
