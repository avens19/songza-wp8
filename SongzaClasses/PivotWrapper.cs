using Songza_WP8;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongzaClasses
{
    public class PivotWrapper
    {
        public ObservableCollection<object> List { get; set; }
        public string TitleText{get;set;}
    }
}
