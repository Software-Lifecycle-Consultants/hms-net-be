using AutoMapper;
using HMS.DTOs;
using HMS.Models;

namespace HMS.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<ContactDTO, Contact>();

            CreateMap<Room, RoomDTO>();
            CreateMap<RoomDTO, Room>();
           
        }
    }
}
