using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;

namespace Book_Your_Hotel.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //for hotel
            CreateMap<Hotels, HotelsDTO>().ReverseMap();
          
            CreateMap<Hotels,HotelCreateDTO>().ReverseMap();
            CreateMap<Hotels,HotelUpdateDTO>().ReverseMap();

            //for hotel numbers.
            CreateMap<HotelNumbers, HotelNoDTO>().ReverseMap();
            CreateMap<HotelNumbers, HotelNoCreateDTO>().ReverseMap();
            CreateMap<HotelNumbers, HotelNoUpdateDTO>().ReverseMap();

            CreateMap<RegisterationRequestDTO, AppUser>().ReverseMap();

            CreateMap<AppUser, UserDTO>().ReverseMap();

            
        }
    }
}
