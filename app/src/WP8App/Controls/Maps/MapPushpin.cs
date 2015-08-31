using System.Device.Location;

namespace WPAppStudio.Controls.Maps
{
    public class MapPushPin
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public object Data { get; set; }
        public GeoCoordinate Coordinate { get; set; }
    }
}