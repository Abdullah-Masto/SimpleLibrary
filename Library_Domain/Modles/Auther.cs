namespace Library_Domain.Modles
{
    public class Auther : Base
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Biography { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}