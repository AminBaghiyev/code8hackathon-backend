using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Profiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<ReservationCreateDTO, Reservation>().ReverseMap();
        CreateMap<ReservationCreateByUserDTO, Reservation>().ReverseMap();
        CreateMap<ReservationUpdateDTO, Reservation>().ReverseMap();
        CreateMap<Reservation, ReservationListItemDTO>();
        CreateMap<Reservation, ReservationTableItemDTO>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number));

        CreateMap<Reservation, ReservationTableItemForUserDTO>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number))
            .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.Room.Status))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.Type))
            .ForMember(dest => dest.RoomPricePerNight, opt => opt.MapFrom(src => src.Room.PricePerNight));
    }
}
