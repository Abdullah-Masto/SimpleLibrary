using AutoMapper;
using Library.Models.ViewModels;
using Library_Domain.Modles;

namespace Library.Models.Profiles
{
    public class AutherProfile : Profile
    {
        public AutherProfile()
        {
            CreateMap<Auther, AutherWithoutBooks>();
        }
    }
}
