using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace WPAppStudio.Controls.Maps
{
    public partial class MapControl : UserControl
    { 
        /// <summary>
        /// Show descriptions when zoom level is equal or over
        /// </summary>
        private const double ZoomToShowDescriptions = 14;

        /// <summary>
        /// Show pitch slider when zoom level is equal or over
        /// </summary>
        private const double ZoomToHidePitch = 7;

        /// <summary>
        /// Indicates coordinate property from ItemsSource elements to be used as pushpin in the map.
        /// </summary>
        public string PushPinProperty { get; set; }

        /// <summary>
        /// Indicates label property from ItemsSource elements to be used as pushpin in the map.
        /// </summary>
        public string PushPinLabelProperty { get; set; }

        /// <summary>
        /// Indicates description property from ItemsSource elements to be used as pushpin in the map.
        /// </summary>
        public string PushPinDescriptionProperty { get; set; }

        /// <summary>
        /// Indicates image property from ItemsSource elements to be used as pushpin in the map.
        /// </summary>
        public string PushPinImageProperty { get; set; }

        /// <summary>
        /// Indicates template to be used as pushpin in the map.
        /// </summary>
        public string PushPinTemplate { get; set; }

        private ObservableCollection<MapPushPin> _pushPins;
        /// <summary>
        /// Collection of map pushpins.
        /// </summary>
        public ObservableCollection<MapPushPin> PushPins
        {
            get { return _pushPins; }
        }

        /// <summary>
        /// Identifies the ItemsSource dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(MapControl), new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            mapControl.PushPins.Clear();

            if (e.NewValue is IEnumerable)
            {
                var data = e.NewValue as IEnumerable;

                foreach (var dataItem in data)
                {
                    AddPushPinItem(dataItem, mapControl);
                }
            }
            else
            {
                AddPushPinItem(e.NewValue, mapControl);
            }
        }

        /// <summary>
        /// Gets or sets the source of map pushpins.
        /// </summary>
        public ObservableCollection<GeoCoordinate> ItemsSource
        {
            get { return (ObservableCollection<GeoCoordinate>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets available map views.
        /// </summary>
        public ObservableCollection<MapMode> Modes { get; set; }

        /// <summary>
        /// Identifies the SelectedMode dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedModeProperty =
            DependencyProperty.Register("SelectedMode", typeof(MapMode), typeof(MapControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the current map view.
        /// </summary>
        public MapMode SelectedMode
        {
            get { return (MapMode)GetValue(SelectedModeProperty); }
            set { SetValue(SelectedModeProperty, value); }
        }

        /// <summary>
        /// Identifies the Pitch dependency property.
        /// </summary>
        public static readonly DependencyProperty PitchProperty =
            DependencyProperty.Register("Pitch", typeof(double), typeof(MapControl), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the map pitch level.
        /// </summary>
        public double Pitch
        {
            get { return (double)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }

        /// <summary>
        /// Identifies the Zoom dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MapControl), new PropertyMetadata(1.0, OnZoomChanged));

        /// <summary>
        /// Gets or sets the map zoom level.
        /// </summary>
        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapControl = d as MapControl;

            var zoom = (double) e.NewValue;

            mapControl.PitchLevel.Visibility = zoom < ZoomToHidePitch ? Visibility.Collapsed : Visibility.Visible;

            CalculatePushpinDescriptionsVisibility(mapControl, zoom);
        }

        /// <summary>
        /// Identifies the UserLocation dependency property.
        /// </summary>
        public static readonly DependencyProperty UserLocationProperty =
            DependencyProperty.Register("UserLocation", typeof(GeoCoordinate), typeof(MapControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the user location.
        /// </summary>
        public GeoCoordinate UserLocation
        {
            get { return (GeoCoordinate)GetValue(UserLocationProperty); }
            set { SetValue(UserLocationProperty, value); }
        }

        /// <summary>
        /// Geolocator used to stablish user's current position.
        /// </summary>
        private readonly Geolocator _geolocator;

        /// <summary>
        /// Identifies the SelectedItem dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(MapControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the map selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public MapControl()
        {
            InitializeComponent();

            PushPinProperty = string.Empty;
            PushPinLabelProperty = string.Empty;
            PushPinDescriptionProperty = string.Empty;
            PushPinImageProperty = string.Empty;

            Modes = new ObservableCollection<MapMode>
                {
                  new MapMode {CartographicMode = MapCartographicMode.Road, Name = "Road"},
                  new MapMode {CartographicMode = MapCartographicMode.Aerial, Name = "Aerial"},
                  new MapMode {CartographicMode = MapCartographicMode.Hybrid, Name = "Hybrid"},
                  new MapMode {CartographicMode = MapCartographicMode.Terrain, Name = "Terrain"}
                };

            SelectedMode = Modes.First();

            _pushPins = new ObservableCollection<MapPushPin>();
            _pushPins.CollectionChanged += (sender, args) => CalculateZoomAndCenter();

            // Locator initializing
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracy = PositionAccuracy.Default;
            _geolocator.MovementThreshold = 100;
            SetLocationMarkerVisibility(Visibility.Collapsed);
        }

        /// <summary>
        /// Starts user location detection.
        /// </summary>
        public void StartUserLocation()
        {
            _geolocator.PositionChanged -= GeolocatorOnPositionChanged;
            _geolocator.PositionChanged += GeolocatorOnPositionChanged;
        }

        private void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
			if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
				if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
				{
					// The user has opted out of Location.
					return;
				}

				Deployment.Current.Dispatcher.BeginInvoke(() =>
				{
					UserLocation = ConvertGeocoordinate(args.Position.Coordinate);
					SetLocationMarkerVisibility(Visibility.Visible);
				});
			}
        }

        private void SetLocationMarkerVisibility(Visibility visibility)
        {
            UserLocationMarker locationMarker =
                    MapExtensions.GetChildren(Map).OfType<UserLocationMarker>().FirstOrDefault();

            if (locationMarker != null)
                locationMarker.Visibility = visibility;
        }

        /// <summary>
        /// Converts from Geocoordinate to GeoCoordinate.
        /// </summary>
        /// <param name="geocoordinate">The Geocoordinate.</param>
        /// <returns>A converted GeoCoordinate.</returns>
        private GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }

        private void CalculateZoomAndCenter()
        {
            // Bounding rectangle
            double maxLat = PushPins.Any() ? PushPins.Max(p => p.Coordinate.Latitude) : 0;
            double minLat = PushPins.Any() ? PushPins.Min(p => p.Coordinate.Latitude) : 0;
            double maxLong = PushPins.Any() ? PushPins.Max(p => p.Coordinate.Longitude) : 0;
            double minLong = PushPins.Any() ? PushPins.Min(p => p.Coordinate.Longitude) : 0;

            double latDiff = Math.Abs(maxLat - minLat);
            double longDiff = Math.Abs(maxLong - minLong);

            // Center of bounding rectangle is center of map view
            var center = new GeoCoordinate
            {
                Latitude = (maxLat + minLat) / 2.0,
                Longitude = (maxLong + minLong) / 2.0
            };

            // Optimal zoom calculations
            int buffer = 100;
            double widthZoom = longDiff == 0.0
                ? 14
                : Math.Log(360.0/256.0*(Map.Width - 2*buffer)/longDiff)/Math.Log(2);

            double heightZoom = latDiff == 0.0
                ? 14
                : Math.Log(180.0 / 256.0 * (Map.Height - 2 * buffer) / latDiff) / Math.Log(2);

            double zoomLevel = (widthZoom < heightZoom) ? widthZoom : heightZoom;

            if (zoomLevel < 1)
                zoomLevel = 1;

            Zoom = zoomLevel;
                
            Deployment.Current.Dispatcher.BeginInvoke(() => Map.SetView(center, zoomLevel, 0, Map.Pitch, MapAnimationKind.None));
        }

        /// <summary>
        /// Adds a pushpin in the map.
        /// </summary>
        /// <param name="data">Pushpin info object container.</param>
        /// <param name="mapControl">The map.</param>
        private static void AddPushPinItem(object data, MapControl mapControl)
        {
            if(data == null) return;
			
            try
            {
				var pushPinProp = data.GetType().GetProperty(mapControl.PushPinProperty);
				var pushPinLabelProp = data.GetType().GetProperty(mapControl.PushPinLabelProperty);
				var pushPinDescriptionProp = data.GetType().GetProperty(mapControl.PushPinDescriptionProperty);
				var pushPinImageProp = data.GetType().GetProperty(mapControl.PushPinImageProperty);

				var mapPushPin = new MapPushPin
				{
					Label = pushPinLabelProp != null ? (string)pushPinLabelProp.GetValue(data) : null,
					Coordinate = pushPinProp != null ? pushPinProp.GetValue(data) as GeoCoordinate : null,
					Description = pushPinDescriptionProp != null ? (string)pushPinDescriptionProp.GetValue(data) : null,
					Image = pushPinImageProp != null ? (string)pushPinImageProp.GetValue(data) : null,
					Data = data,
					Template = mapControl.PushPinTemplate
				};

				if(mapPushPin.Coordinate == null) return;

				mapControl.PushPins.Add(mapPushPin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
		
        private static void CalculatePushpinDescriptionsVisibility(MapControl mapControl, double zoom)
        {
            var allPushpins = mapControl.FindDescendants<Pushpin>();

            if (allPushpins.Any())
            {
                var contentControls = allPushpins.SelectMany(p => p.FindDescendants<ContentControl>());

                foreach (var content in contentControls)
                {
                    var stackPanel = content.FindDescendant<StackPanel>();
                    var descriptionControl =
                        stackPanel.FindDescendants<FrameworkElement>().FirstOrDefault(c => c.Name == "DescriptionContainer");

                    if(descriptionControl == null) continue;

                    var tag = ((FrameworkElement)descriptionControl.Parent).Tag as MapPushPin;

                    if(tag == null) continue;

                    bool hasEmptyDescription = HasEmptyDescription(tag, mapControl.PushPinTemplate);

                    descriptionControl.Visibility =
                        !hasEmptyDescription && zoom > ZoomToShowDescriptions
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
        }

        private static bool HasEmptyDescription(MapPushPin tag, string currentTemplate)
        {
            switch (currentTemplate)
            {
                case "ImageLabel":
                    return string.IsNullOrEmpty(tag.Label);
                default:
                    return string.IsNullOrEmpty(tag.Description);
            }
        }

        private void PushPin_OnTap(object sender, GestureEventArgs e)
        {
            var control = sender as FrameworkElement;
            var pushpin = control.Tag as MapPushPin;

            SelectedItem = pushpin.Data;
        }
    }
}
