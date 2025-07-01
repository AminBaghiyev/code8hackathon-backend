using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Profiles.RoomProfiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomCreateDTO>().ReverseMap();
        CreateMap<Room, RoomUpdateDTO>().ReverseMap();
        CreateMap<Room, RoomListDTO>().ReverseMap();
        CreateMap<Room, RoomTableDTO>().ReverseMap();
    }
}
