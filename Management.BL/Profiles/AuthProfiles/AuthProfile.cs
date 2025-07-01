using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Profiles.AuthProfiles;
public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<LoginDTO, AppUser>();
        CreateMap<RegisterDTO, AppUser>();
        CreateMap<ProfileUpdateDTO, AppUser>().ReverseMap();
    }
}
