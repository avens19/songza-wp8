using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Songza_WP8
{
    public class Station
    {
        public string Name{get;set;}
        public int SongCount{get;set;}
        private string coverurl;
        public string CoverUrl{
            get
            {
                return coverurl + "?size=300";
            }
            set
            {
                coverurl = value;
            }
        }
        public long Id{get;set;}
        public string Description{get;set;}
        public List<Track.Artist> FeaturedArtists { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}