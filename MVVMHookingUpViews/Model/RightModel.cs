using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;

namespace MVVMHookingUpViews.Model
{
    public class RightModel:INotifyPropertyChanged
    {
        private double x;
        public double X
        {
            get 
            { return x; 
            }
            set 
            { x = value;
                OnPropertyChange("X");
            }
        }

        private double y;
        public double Y
        {
            get 
            { return y; 
            }
            set 
            {
                y = value;
                OnPropertyChange("Y");
            }
        }

        private double rectX;
        public double RectX
        {
            get
            {
                return rectX;
            }
            set
            {
                rectX = value;
                OnPropertyChange("RectX");
            }
        }

        private double rectY;
        public double RectY
        {
            get
            {
                return rectY;
            }
            set
            {
                rectY = value;
                OnPropertyChange("RectY");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
}
