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
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<UserDTO, JWTCreateDTO>().ReverseMap();
            CreateMap<RoleDTO, IdentityRole>().ReverseMap();
            CreateMap<RoleDTO, RoleCreateDTO>().ReverseMap();
            CreateMap<RoleDTO, RoleUpdateDTO>().ReverseMap();
            CreateMap<RoleCreateDTO, IdentityRole>().ReverseMap();
            CreateMap<RoleUpdateDTO, IdentityRole>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, ReviewUpdateDTO>().ReverseMap();
            CreateMap<Review, ReviewCreateDTO>().ReverseMap();
            CreateMap<ReviewDTO, ReviewCreateDTO>().ReverseMap();
            CreateMap<Package, PackageDTO>().ReverseMap();
            CreateMap<Package, PackageUpdateDTO>().ReverseMap();
            CreateMap<PackageDTO, PackageCreateDTO>().ReverseMap();
            CreateMap<Package, PackageCreateDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentUpdateDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentCreateDTO>().ReverseMap();
            CreateMap<AppointmentDTO, AppointmentCreateDTO>().ReverseMap();
        }
    }
}
