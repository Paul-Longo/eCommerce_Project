using AutoMapper;
using eCommerceStarterCode.DataTransferObjects;
using eCommerceStarterCode.Models;

namespace eCommerceStarterCode.Managers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
