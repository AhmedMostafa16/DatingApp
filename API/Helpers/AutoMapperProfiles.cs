using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

// This organizes your mapping configurations is with profiles.

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                                src.Photos.FirstOrDefault(x => x.IsMain).Url))
                            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO,AppUser>();
        }
    }
}