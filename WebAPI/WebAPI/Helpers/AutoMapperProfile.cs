using AutoMapper;
using WebAPI.Dtos;
using WebAPI.Entities;

namespace WebAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateBidirectionalMap<UserToRegister, UserToRegisterDto>();
            CreateBidirectionalMap<Application, ApplicationDto>();
        }

        private void CreateBidirectionalMap<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
}
