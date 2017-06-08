using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraWeather.Models
{
    public class Alldata
    {
        private float _temperature;
        private float _pressure;
        private float _humidity;
        public int version { get; set; }
        public string time { get; set; }

        public float temperature
        {
            get { return _temperature; }
            set { _temperature = value; Value = _temperature.ToString(); }
        }

        public float pressure
        {
            get { return _pressure; }
            set { _pressure = value; Value = _pressure.ToString(); }
        }

        public float humidity
        {
            get { return _humidity; }
            set { _humidity = value; Value = _humidity.ToString(); }
        }

        public string Value { get; set; }
    }
}
