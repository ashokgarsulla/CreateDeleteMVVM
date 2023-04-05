using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMHookingUpViews.Views;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using MVVMHookingUpViews.Model;
using System.Windows;
using System.Windows.Shapes;
using System.ComponentModel;



namespace MVVMHookingUpViews.ViewModel
{
    public class RightViewModel : INotifyPropertyChanged
    {
        private bool captured = false;
        private double currentPositionY;
        private int direction;
        private double degree;
        public ICommand PreviewMouseMoveCommand { get; set; }
        public ICommand LeftMouseButtonDownCommand { get; set; }
        public ICommand LeftMouseButtonUpCommand { get; set; }
        public RightViewModel()
        {
            PreviewMouseMoveCommand = new MyICommand(PreviewMouseMove);
            LeftMouseButtonDownCommand = new MyICommand(LeftMouseButtonDown);
            LeftMouseButtonUpCommand = new MyICommand(LeftMouseButtonUp);

        }
        private double x;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChange("X");
            }
        }

        private double y;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChange("Y");
            }
        }

        private double rectX = 100;
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

        private double rectY = 100;
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

        private double scale = 50;
        public double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                if(value < 10)
                {
                    scale = 10;
                }
                OnPropertyChange("Scale");
            }
        }
        private double angle;
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                OnPropertyChange("Angle");
            }
        }
        private double centerOfSquareX;
        public double CenterOfSquareX
        {
            get
            {
                return centerOfSquareX;
            }
            set
            {
                centerOfSquareX = value;
                OnPropertyChange("CenterOfSquareX");
            }
        }
        private double centerOfSquareY;
        public double CenterOfSquareY
        {
            get
            {
                return centerOfSquareY;
            }
            set
            {
                centerOfSquareY = value;
                OnPropertyChange("CenterOfSquareY");
            }
        }

        private double originX;
        public double OriginX
        {
            get
            {
                return originX;
            }
            set
            {
                originX = value;
                OnPropertyChange("OriginX");
            }
        }

        private double originY;
        public double OriginY
        {
            get
            {
                return originY;
            }
            set
            {
                originY = value;
                OnPropertyChange("OriginY");
            }
        }
        public void ZoomInOutByWheel(object sender, MouseWheelEventArgs e)
        {
            
            if (e.Delta > 0 && Scale < 700)
            {
                Scale += 10;
            }

            if (e.Delta < 0)
            {
                Scale -= 10;
                if (Scale < 10)
                {
                    Scale = 10;
                }
            }
        }
        public void MoveSquareWithMouse(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(X - Scale / 2 > 0)
                {
                    RectX = X - Scale / 2;
                    RectY = Y - Scale / 2;
                }
                
            }
        }
        public void RotateSquare(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                double deltaDirection = currentPositionY - Y;
                direction = deltaDirection > 0 ? 1 : -1;
                currentPositionY = Y;
                if (direction == 1)
                {
                    if (degree < 360)
                    {
                        CenterOfSquareX = Scale / 2;
                        CenterOfSquareY = Scale / 2;
                        degree += 2;
                        Angle = degree;
                    }

                }
                if (direction == -1)
                {
                    if (degree > -360)
                    {
                        CenterOfSquareX = Scale / 2;
                        CenterOfSquareY = Scale / 2;
                        degree -= 2;
                        Angle = degree;
                    }

                }
            }
            else
            {
                currentPositionY = Y;
            }
        }
        public void PreviewMouseMove(object parameter)
        {
            if (captured && X - 5.0 > 0)
            {
                RectX = X - 5.0 ;
                RectY = Y - 5.0;
            }
        }
        public void LeftMouseButtonDown(object parameter)
        {
            captured = true;
            
        }
        public void LeftMouseButtonUp(object parameter)
        {
            captured = false;
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
