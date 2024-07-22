using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Domain.Modles
{
    public class Book : Base
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Auther? Auther { get; set; }
        public int AutherId { get; set; }
        public int Pages { get; set; }
        public string Language { get; set; }
        public double Price { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }

    }
}
