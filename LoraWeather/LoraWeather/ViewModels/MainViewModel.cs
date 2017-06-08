using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LoraWeather.Custom;
using LoraWeather.Models;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace LoraWeather.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Pin> _myPins;
        private CustomLocation _toRegionCustomLocation;
        private static HttpClient client = new HttpClient();

        public ObservableCollection<Pin> MyPins
        {
            get => _myPins;
            set
            {
                _myPins = value;
                OnPropertyChanged();
            }
        }

        public CustomLocation ToRegionCustomLocation
        {
            get => _toRegionCustomLocation;
            set
            {
                _toRegionCustomLocation = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
            MyPins = await SetPinsOnMap();
        }

        private async Task<ObservableCollection<Pin>> SetPinsOnMap()
        {

            client.Timeout = new TimeSpan(0, 0, 10);
            var responseHttp = await client.GetAsync("[API url]");

            if (responseHttp == null)
                return null;

            HttpContent responseContent = responseHttp.Content;
            string output;

            using (var readerStream = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                output = await readerStream.ReadToEndAsync();
            }

            if (output == "{\"results\":[{\"statement_id\":0}]}\n")
            {
                return default(ObservableCollection<Pin>);
            }

            var deviceList = JsonConvert.DeserializeObject<DeviceList>(output);

            //await Task.Delay(1000);
            ToRegionCustomLocation = new CustomLocation(52.132633, 5.291266, 7);
            var tempList = new ObservableCollection<Pin>();
            foreach (var obj in deviceList.devicesList)
            {
                tempList.Add(new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(obj.latitude, obj.longitude),
                    Label = "TTN ID: " + obj.id,
                    Address = "Device ID: " + obj.device_id_ttn
                });
            }
            return tempList;
        }
    }
}
