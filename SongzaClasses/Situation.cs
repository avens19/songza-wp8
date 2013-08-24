using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Songza_WP8
{
    public class Situation
    {
        public string Id{get;set;}
        public string SelectedMessage{get;set;}
        public string Title{get;set;}
        public List<int> StationIds{get;set;}
    }
}
