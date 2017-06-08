namespace LoraWeather.Custom
{
    public class CustomLocation
    {
        public double Latidude { get; set; }
        public double Longitude { get; set; }
        public float ZoomLevel { get; set; }

        public CustomLocation(double latitude, double longitude, float zoomLevel)
        {
            Latidude = latitude;
            Longitude = longitude;
            ZoomLevel = zoomLevel;
        }
    }
}
