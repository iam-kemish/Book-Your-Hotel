using AutoMapper;
using BookHotel_Frontend.Models.DTOs;

namespace BookHotel_Frontend.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //for hotel
           
          
            CreateMap<HotelsDTO,HotelCreateDTO>().ReverseMap();
            CreateMap<HotelsDTO,HotelUpdateDTO>().ReverseMap();

            //for hotel numbers.
         
            CreateMap<HotelNoDTO, HotelNoCreateDTO>().ReverseMap();
            CreateMap<HotelNoDTO, HotelNoUpdateDTO>().ReverseMap();

            
        }
    }
}
