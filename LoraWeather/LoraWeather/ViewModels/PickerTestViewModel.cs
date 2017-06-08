using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraWeather.ViewModels
{
    public class PickerTestViewModel : BaseViewModel
    {
        private IEnumerable<string> _items;
        private int _selectedIndex;

        public IEnumerable<string> Items
        {
            get
            {
                return _items; 
                
            }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value; 
                OnPropertyChanged();
            }
        }

        public PickerTestViewModel()
        {
            Items = new List<string>{"test1", "test2", "test3"};
        }
    }
}
