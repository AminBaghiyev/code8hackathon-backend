using AutoMapper;
using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Profiles;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<ServiceCreateDTO, Service>();
    }
}
