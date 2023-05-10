using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NextOptimization.Business.DTOs;
using NextOptimization.Data.Models;

namespace NextOptimization.Business.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, JWTCreateDTO>().ReverseMap();
            CreateMap<UserDTO, JWTCreateDTO>().ReverseMap();
            CreateMap<RoleDTO, IdentityRole>().ReverseMap();
            CreateMap<RoleDTO, RoleCreateDTO>().ReverseMap();
            CreateMap<RoleDTO, RoleUpdateDTO>().ReverseMap();
            CreateMap<RoleCreateDTO, IdentityRole>().ReverseMap();
            CreateMap<RoleUpdateDTO, IdentityRole>().ReverseMap();
        }
    }
}
