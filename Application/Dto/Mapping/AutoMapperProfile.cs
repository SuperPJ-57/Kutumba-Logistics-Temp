
using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.User;
using Application.Dto.Vehicles;
using AutoMapper;
using Domain.Logistic;

namespace Application.Dto.Mapping;

public  class AutoMapperProfile : Profile
{
    public AutoMapperProfile ()
    {
        CreateMap<TripDto, TripDto>()
            //.ForMember(dest => dest.ConsignmentDetail, opt => opt.MapFrom(src => src.Detail))
            .ReverseMap();
        //.ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle)).ReverseMap();

        CreateMap<Driver, DriverDto>()
            //.ForMember(dest => dest.Trips, opt => opt.MapFrom(src => src.Trip))
            //.ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.Vehicle))
            .ReverseMap();

        CreateMap<Vehicle, VehicleDto>()
            //.ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ReverseMap();

        CreateMap<Freight, FreightDto>().ReverseMap();

        //CreateMap<Consignment, ConsignmentDto>().ReverseMap();
        //.ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.Vehicle)).ReverseMap()
        //.ForMember(dest => dest.Freight, opt => opt.MapFrom(src => src.Freights)).ReverseMap();
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

        CreateMap<TripRequest, TripRequestDto>()
            .ForMember(dest => dest.TripDto, opt => opt.MapFrom(src => src.Trip))
            .ForMember(dest => dest.VehicleDto, opt => opt.MapFrom(src => src.Vehicle))
            .ForMember(dest => dest.ConsignmentDto, opt => opt.MapFrom(src => src.Consignment));
    }
}
