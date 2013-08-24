using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongzaClasses
{
    public class Favorite
    {
        public string Title { get; set; }
        public List<int> StationIds { get; set; }
        public long Id { get; set; }
    }
}
