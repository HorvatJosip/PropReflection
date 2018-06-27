using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Point Point { get; set; }
        public Guid ID { get; set; }
        //public LinkedList<int> Items { get; set; }
        public List<int> Items { get; set; }
    }
}
