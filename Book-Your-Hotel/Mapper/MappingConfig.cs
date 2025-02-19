using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;

namespace Book_Your_Hotel.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Hotels, HotelsDTO>().ReverseMap();
          
            CreateMap<Hotels,HotelCreateDTO>().ReverseMap();
            CreateMap<Hotels,HotelUpdateDTO>().ReverseMap();

            
        }
    }
}
