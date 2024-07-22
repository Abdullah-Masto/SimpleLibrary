using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Domain.Modles
{
    public class Base
    {
        public int Id { get; protected set; }
        public bool IsDeleted { get; set; }
    }
}
