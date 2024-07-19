﻿using AutoMapper;
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
            CreateMap<AdminBlog, AdminBlogDTO>();
            CreateMap<AdminBlogDTO, AdminBlog>();
            CreateMap<RoleDTO, IdentityRole>().ReverseMap();
            CreateMap<ImageDTO, Image>().ReverseMap();
            CreateMap<ImageReturnDTO, Image>().ReverseMap();

            //Admin
            CreateMap<AdminRoomDTO, AdminRoom>().ReverseMap();
            CreateMap<AdminRoomReturnDTO, AdminRoom>().ReverseMap();            
            CreateMap<AdminCategoryDTO, AdminCategory>().ReverseMap();
            CreateMap<AdminCategoryValueDTO, CategoryValue>().ReverseMap();
            CreateMap<AdminCategoryReturnValueDTO, CategoryValue>().ReverseMap();            
            CreateMap<AdminServiceAddonDTO, AdminServiceAddon>().ReverseMap();
            //CreateMap<AdminAdditionalInfoDTO, AdminAdditionalInfo>().ReverseMap();
            CreateMap<AdminRoomSummaryDTO, AdminRoom>().ReverseMap();            
            CreateMap<AdminContactDTO,AdminContact>().ReverseMap();
            CreateMap<AdminGenaralCatagoryDTO, AdminGenaralCatagoryDTO>().ReverseMap();
           

        }
    }
}
