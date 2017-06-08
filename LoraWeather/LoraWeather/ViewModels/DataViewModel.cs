using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using LoraWeather.Models;
using Newtonsoft.Json;

namespace LoraWeather.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        private ObservableCollection<Alldata> _items;
        private IEnumerable<string> _dataItems;
        private int _selectedIndex;
        private DateTime _fromDate;
        private readonly int _deviceId;
        private DateTime _maxToDate;
        private DateTime _minFromDate;
        private DateTime _toDate;
        private TimeSpan _fromTime;
        private TimeSpan _toTime;
        private DateTime _maxFromDate;
        private DateTime _minToDate;
        private bool sameday;
        private bool _isVisible;
        private bool _isBusy;

        public ObservableCollection<Alldata> Items
        {
            get => _items;
            set
            {
                if (IsBusy)
                    return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> DataItems
        {
            get => _dataItems;
            set
            {
                if (IsBusy)
                    return;
                _dataItems = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (IsBusy)
                    return;
                _selectedIndex = value;
                OnPropertyChanged();
                SetDeviceData();
            }
        }

        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                if (IsBusy)
                    return;
                _fromDate = value;
                OnPropertyChanged();
                ChangeMaxMinDate();
                SetDeviceData();
            }
        }

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                if (IsBusy)
                    return;
                _toDate = value;
                OnPropertyChanged();
                ChangeMaxMinDate();
                SetDeviceData();
            }
        }

        public DateTime MaxToDate
        {
            get => _maxToDate;
            set
            {
                if (IsBusy)
                    return;
                _maxToDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime MaxFromDate
        {
            get => _maxFromDate;
            set
            {
                if (IsBusy)
                    return;
                _maxFromDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime MinFromDate
        {
            get => _minFromDate;
            set
            {
                if (IsBusy)
                    return;
                _minFromDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime MinToDate
        {
            get => _minToDate;
            set
            {
                if (IsBusy)
                    return;
                _minToDate = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan FromTime
        {
            get { return _fromTime; }
            set
            {
                if (IsBusy)
                    return;
                if (_fromTime != value)
                {
                    _fromTime = value;
                    OnPropertyChanged();
                    SetDeviceData();
                }
            }
        }

        public TimeSpan ToTime
        {
            get { return _toTime; }
            set
            {
                if (IsBusy)
                    return;
                if (_toTime != value)
                {
                    _toTime = value;
                    OnPropertyChanged();
                    SetDeviceData();
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (IsBusy)
                    return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public DataViewModel(int deviceId, DateTime maxDate, DateTime minDate)
        {
            _deviceId = deviceId;
            Items = new ObservableCollection<Alldata>();
            DataItems = new List<string> { "Temperature", "Humidity", "Pressure" };

            if (maxDate.Day == minDate.Day)
            {
                IsVisible = false;
                sameday = true;
                _maxFromDate = minDate.AddDays(-2);
                _maxToDate = maxDate;
                _minFromDate = minDate.AddDays(-3);
                _minToDate = maxDate.AddDays(-1);
                _toDate = maxDate;
                _toTime = new TimeSpan(maxDate.Hour, maxDate.Minute, 0);
                _fromDate = minDate.AddDays(-1);
                _fromTime = new TimeSpan(minDate.Hour, minDate.Minute, 0);
            }
            else
            {
                IsVisible = true;
                sameday = false;
                var tempMaxDate = maxDate.Date;
                _maxFromDate = tempMaxDate.AddDays(-1);
                _maxToDate = tempMaxDate;
                _toDate = _maxToDate;
                _toTime = new TimeSpan(0, 0, 0);

                var tempMinDate = minDate.Date;
                _minFromDate = tempMinDate;
                _minToDate = tempMinDate.AddDays(1);
                _fromDate = _maxToDate.AddDays(-1);
                _fromTime = new TimeSpan(0, 0, 0);

                ChangeMaxMinDate();
            }
            SetDeviceData();
        }

        private async void SetDeviceData()
        {
            IsBusy = true;
            var client = new HttpClient();
            var request = "[API url]" + _deviceId + "/" + _dataItems.ElementAt(_selectedIndex).ToLower()
                + "/range?from=" + _fromDate.ToString("dd-MM-yyyy") + "%20" + _fromTime.Hours + "%3A" + _fromTime.Minutes +
                "&to=" + _toDate.ToString("dd-MM-yyyy") + "%20" + _toTime.Hours + "%3A" + _toTime.Minutes;

            HttpResponseMessage responseHttp = await client.GetAsync(request);

            HttpContent responseContent = responseHttp.Content;
            string output;

            using (var stream = await responseContent.ReadAsStreamAsync())
            {
                using (var readerStream = new StreamReader(stream))
                {
                    output = await readerStream.ReadToEndAsync();
                }
            }

            if (output == "{\"results\":[{\"statement_id\":0}]}\n")
            {
                return;
            }

            var data = JsonConvert.DeserializeObject<DataModel>(output);
            Items.Clear();

            foreach (var obj in data.allData)
            {
                obj.time = Convert.ToDateTime(obj.time).ToUniversalTime().ToString();
                Items.Add(obj);
            }
            IsBusy = false;
        }

        private void ChangeMaxMinDate()
        {
            if (!sameday)
            {
                MaxFromDate = _toDate.AddDays(-1);
                MinToDate = _fromDate.AddDays(1);
            }
        }
    }
}
