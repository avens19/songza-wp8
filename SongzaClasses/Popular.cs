using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongzaClasses
{
    public class Popular
    {
        public string Name { get; set; }
        public string Tag { get; set; }

        public Popular() { }

        public Popular(string name, string tag)
        {
            Name = name;
            Tag = tag;
        }
    }
}
