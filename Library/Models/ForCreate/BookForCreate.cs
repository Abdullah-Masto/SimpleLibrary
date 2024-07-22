using Library_Domain.Modles;

namespace Library.Models.ForCreate
{
    public class BookForCreate
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Auther Auther { get; set; }
        public int AutherId { get; set; }
        public int Pages { get; set; }
        public string Language { get; set; }
        public double Price { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
    }
}
