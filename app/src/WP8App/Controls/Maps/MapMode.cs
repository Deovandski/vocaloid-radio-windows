using System.Collections;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;

namespace WPAppStudio.Controls.Maps
{
    public class MapMode
    {
        public string Name { get; set; }
        public MapCartographicMode CartographicMode { get; set; }
    }
}