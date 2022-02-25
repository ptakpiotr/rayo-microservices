using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RayoAuth.Models;

namespace RayoAuth.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<RegisterModel,IdentityUser>().ForMember(dest=>dest.UserName,opts=>opts.MapFrom(src=>src.Email));
        }
    }
}
