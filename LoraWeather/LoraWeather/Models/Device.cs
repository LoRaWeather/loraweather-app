namespace LoraWeather.Models
{
    public class Device
    {
        public int version { get; set; }
        public int id { get; set; }
        public int battery { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public string device_id_ttn { get; set; }
        public string device_description { get; set; }
        public string first_up { get; set; }
        public string last_seen { get; set; }
        public int packages_send { get; set; }
    }
}
