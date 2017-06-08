using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LoraWeather.Custom
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty AllPinsProperty =
            BindableProperty.Create("AllPins", typeof(IEnumerable<Pin>), typeof(CustomMap));

        public static readonly BindableProperty ToRegionProperty =
            BindableProperty.Create("ToRegion", typeof(CustomLocation), typeof(CustomMap));

        public IEnumerable<Pin> AllPins
        {
            get => (IEnumerable<Pin>)GetValue(AllPinsProperty);
            set => SetValue(AllPinsProperty, value);
        }

        public CustomLocation ToRegion {
            get => (CustomLocation)GetValue(ToRegionProperty);
            set => SetValue(ToRegionProperty, value);
        }
    }
}
