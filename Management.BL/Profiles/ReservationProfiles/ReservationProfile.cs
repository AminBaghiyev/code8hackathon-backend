using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Profiles.ReservationProfiles
{
    public class ReservationProfile:Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationCreateDTO, Reservation>().ReverseMap();
            CreateMap<ReservationUpdateDTO, Reservation>().ReverseMap();
            CreateMap<Reservation, ReservationListItemDTO>();
            CreateMap<Reservation, ReservationTableItemDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number));
        }
    }
}
