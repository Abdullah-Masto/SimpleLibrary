using AutoMapper;
using Library.Models.ViewModels;
using Library_Domain.Modles;

namespace Library.Models.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookWithoutAuther>();
        }
    }
}
