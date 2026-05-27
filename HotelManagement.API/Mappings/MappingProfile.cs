using AutoMapper;
using HotelManagement.API.DTOs.Hotels;
using HotelManagement.API.DTOs.Payments;
using HotelManagement.API.DTOs.Reservations;
using HotelManagement.API.DTOs.Rooms;
using HotelManagement.API.DTOs.Users;
using HotelManagement.API.Models;

namespace HotelManagement.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Hotel, HotelDto>();
        CreateMap<CreateHotelRequest, Hotel>();
        CreateMap<UpdateHotelRequest, Hotel>();

        CreateMap<Room, RoomDto>()
            .ForMember(
                destination => destination.HotelName,
                options => options.MapFrom(source => source.Hotel != null ? source.Hotel.Name : null))
            .ForMember(
                destination => destination.RoomTypeName,
                options => options.MapFrom(source => source.RoomType != null ? source.RoomType.TypeName : null))
            .ForMember(
                destination => destination.PricePerNight,
                options => options.MapFrom(source => source.RoomType != null ? source.RoomType.PricePerNight : null));
        CreateMap<CreateRoomRequest, Room>();
        CreateMap<UpdateRoomRequest, Room>();

        CreateMap<Reservation, ReservationDto>()
            .ForMember(
                destination => destination.RoomNumber,
                options => options.MapFrom(source => source.Room != null ? source.Room.RoomNumber : null));
        CreateMap<CreateReservationRequest, Reservation>()
            .ForMember(destination => destination.ReservationStatus, options => options.MapFrom(_ => "Confirmed"));
        CreateMap<UpdateReservationRequest, Reservation>();

        CreateMap<Payment, PaymentDto>();

        CreateMap<User, UserDto>()
            .ForMember(
                destination => destination.RoleName,
                options => options.MapFrom(source => source.Role != null ? source.Role.RoleName : null));
    }
}
