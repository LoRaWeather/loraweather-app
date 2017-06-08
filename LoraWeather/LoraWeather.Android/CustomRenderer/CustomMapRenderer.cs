using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using LoraWeather.Custom;
using LoraWeather.Droid.CustomRenderer;
using LoraWeather.ViewModels;
using LoraWeather.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace LoraWeather.Droid.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        private GoogleMap _map;
        private IList<Pin> _customPins;
        private bool _isDrawn;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _map.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                Control.GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            _map.InfoWindowClick += OnInfoWindowClick;
            _map.SetInfoWindowAdapter(this);
        }

        private void SetMapMarkers()
        {
            _map.Clear();

            if (_customPins == null) return;
            foreach (var pin in _customPins)
            {
                AddMarker(pin, false);
            }
            _isDrawn = true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("ToRegion") && _map != null && !_isDrawn)
            {
                var mapa = (CustomMap)sender;
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(new LatLng(mapa.ToRegion.Latidude, mapa.ToRegion.Longitude));
                builder.Zoom(mapa.ToRegion.ZoomLevel);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                _map.MoveCamera(cameraUpdate);
            }
            else if (e.PropertyName.Equals("AllPins") && _map!=null && !_isDrawn)
            {
                var formsMap = (CustomMap)sender;
                _customPins?.Clear();
                _customPins = (IList<Pin>)formsMap.AllPins;
                SetMapMarkers();
            }
        }

        private void AddMarker(Pin pin, bool isSelected)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            var selectedMarker = _map.AddMarker(marker);

            if (isSelected)
                selectedMarker.ShowInfoWindow();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            //Catches Exception incase user needs to update google play services
            try
            {
                base.OnLayout(changed, l, t, r, b);

                if (changed)
                {
                    _isDrawn = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            if (!string.IsNullOrWhiteSpace(customPin.Address))
            {
                var deviceView = new DeviceView { BindingContext = new DeviceViewModel(customPin.Label) };
                Application.Current.MainPage.Navigation.PushAsync(deviceView);
            }
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater =
                Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            if (inflater != null)
            {
                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }


                var view = inflater.Inflate(Resource.Layout.ListItem, null);

                var clickMe = view.FindViewById<TextView>(Resource.Id.ClickMe);
                var deviceId = view.FindViewById<TextView>(Resource.Id.DeviceId);
                var ttnId = view.FindViewById<TextView>(Resource.Id.TTNId);

                if (clickMe != null)
                {
                    clickMe.Text = "Click me for more details and data.";
                }

                if (deviceId != null)
                {
                    deviceId.Text = marker.Title;
                }

                if (ttnId != null)
                {
                    ttnId.Text = marker.Snippet;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        private Pin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            return _customPins.FirstOrDefault(pin => pin.Position == position);
        }
    }
}