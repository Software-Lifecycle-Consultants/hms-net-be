using AutoMapper;
using HMS.DTOs;
using HMS.Models;
using Microsoft.AspNetCore.Identity;

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

            CreateMap<RoleDTO, IdentityRole>().ReverseMap();
          
        }
    }
}
