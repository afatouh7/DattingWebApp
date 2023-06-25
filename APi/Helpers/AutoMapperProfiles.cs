using Api.Dtos;
using APi.Dtos;
using APi.Entities;
using APi.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;

namespace APi.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemeberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalaculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto , AppUser>();
            CreateMap<RgisterDto, AppUser>()
          .ForMember(dest => dest.KnownAs, opt => opt.MapFrom(src => src.KnownAs));


        }

    }
}
