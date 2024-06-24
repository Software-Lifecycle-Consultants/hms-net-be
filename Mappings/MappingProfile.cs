using AutoMapper;
using HMS.DTOs;
using HMS.DTOs.Admin;
using HMS.Models;
using HMS.Models.Admin;
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

            //Admin
            CreateMap<AdminRoomDTO,AdminRoom>().ReverseMap();
            CreateMap<AdminContactDTO,AdminContact>().ReverseMap();
          
        }
    }
}
