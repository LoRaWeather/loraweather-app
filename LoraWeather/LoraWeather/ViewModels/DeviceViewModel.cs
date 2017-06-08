using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using LoraWeather.Views;
using Newtonsoft.Json;
using Xamarin.Forms;
using Device = LoraWeather.Models.Device;

namespace LoraWeather.ViewModels
{
    public class DeviceViewModel : BaseViewModel
    {
        private string _deviceId;
        private string _battery;
        private string _ttnId;
        private string _description;
        private string _firstUp;
        private string _lastSeen;
        private string _packagesSend;
        private bool _isBusy;
        private bool _isEnabled;
        private int _Id;
        private bool _isVisible;

        public string DeviceId
        {
            get => _deviceId;
            set
            {
                _deviceId = value;
                OnPropertyChanged();
            }
        }

        public string Battery
        {
            get => _battery;
            set
            {
                _battery = value;
                OnPropertyChanged();
            }
        }

        public string TTNId
        {
            get => _ttnId;
            set
            {
                _ttnId = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string FirstUp
        {
            get => _firstUp;
            set
            {
                _firstUp = value;
                OnPropertyChanged();
            }
        }

        public string LastSeen
        {
            get => _lastSeen;
            set
            {
                _lastSeen = value;
                OnPropertyChanged();
            }
        }

        public string PackagesSend
        {
            get => _packagesSend;
            set
            {
                _packagesSend = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                IsEnabled = _isBusy;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => !_isBusy;
            set
            {
                _isEnabled = !_isBusy;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public Command ShowDataView => new Command(StartDataView);

        public DeviceViewModel(string deviceId)
        {
            _Id = Convert.ToInt32(Regex.Match(deviceId, @"\d+").Value);
            SetDeviceData(_Id);
        }

        private async void SetDeviceData(int deviceId)
        {
            IsBusy = true;

            HttpClient client = new HttpClient();
            HttpResponseMessage responseHttp = await client.GetAsync("[API url]" + deviceId);


            HttpContent responseContent = responseHttp.Content;
            string output;

            using (var readerStream = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                output = await readerStream.ReadToEndAsync();
            }

            if (output == "{\"results\":[{\"statement_id\":0}]}\n")
            {
                return;
            }

            var device = JsonConvert.DeserializeObject<Device>(output);

            DeviceId = device.id.ToString();
            TTNId = device.device_id_ttn;
            Description = device.device_description;
            if (device.first_up == null && device.last_seen == null)
            {
                IsVisible = false;
                PackagesSend = "More data will come when first message is send.";
            }
            else
            {
                IsVisible = true;
                Battery = device.battery == 0 ? "Good" : "Bad";
                FirstUp = Convert.ToDateTime(device.first_up).ToString();
                LastSeen = Convert.ToDateTime(device.last_seen).ToString();
                PackagesSend = device.packages_send.ToString();
            }
            IsBusy = false;
        }

        public void StartDataView()
        {
            var tempMaxDate = Convert.ToDateTime(LastSeen);
            var tempMinDate = Convert.ToDateTime(FirstUp);
            var dataView = new DataView { BindingContext = new DataViewModel(_Id, tempMaxDate, tempMinDate) };
            Application.Current.MainPage.Navigation.PushAsync(dataView);
        }
    }
}
