using AutoMapper;
using Trustesse_Assessment.Dto;
using Trustesse_Assessment.Model;

namespace Trustesse_Assessment.Configuration
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
        }
    }
}
