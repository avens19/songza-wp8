using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Songza_WP8
{
    public class Track
    {
        public string ListenUrl { get; set; }
        public TrackDetails Song { get; set; }

        public class TrackDetails
        {
            public string Album { get; set; }
            public Artist Artist { get; set; }
            public string Title { get; set; }
            public string CoverUrl { get; set; }
            public string Genre { get; set; }
            public string Duration { get; set; }
            public string Id { get; set; }
        }

        public class Artist
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
