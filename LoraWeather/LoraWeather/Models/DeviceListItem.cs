namespace LoraWeather.Models
{
    public class DevicesListItem
    {
        public int version { get; set; }
        public int id { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public string device_id_ttn { get; set; }
    }
}
