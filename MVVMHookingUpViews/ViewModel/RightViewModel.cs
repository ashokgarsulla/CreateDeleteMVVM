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
using System.Numerics;



namespace MVVMHookingUpViews.ViewModel
{
    public class Matrix2X2
    {
        private readonly double M00;
        private readonly double M01;
        private readonly double M10;
        private readonly double M11;
        public Matrix2X2(double m00, double m01, double m10, double m11)
        {
            M00 = m00;
            M01 = m01;
            M10 = m10;
            M11 = m11;
        }
        public static Vector2 operator *(Matrix2X2 m, Vector2 v)
        {
            Vector2 vector2;
            vector2.X = (float)((m.M00 * v.X) + (m.M01 * v.Y));
            vector2.Y = (float)((m.M10 * v.X) + (m.M11 * v.Y));
            return vector2;
        }

    }
    public class RightViewModel : INotifyPropertyChanged
    {
        private int mouseClickedCount = 0;
        private Vector2 clickedPosition;
        private Vector2 mouseVector;
        private double angleForSquareRotation;
        public RightViewModel()
        {

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

        private double scale = 1;
        public double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
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
        private double centerOfSquareX = 100 / 2;
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
        private double centerOfSquareY = 100 / 2;
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
        public void ZoomInOutByWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && Scale < 2)
            {
                Scale += 0.1;
            }

            if (e.Delta < 0)
            {
                if (Scale > 0.5)
                {
                    Scale -= 0.1;
                }
            }
        }
        public void MoveSquareWithMouse(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bool isHMovable = X - (100 * Scale / 2) > 0 && X + (100 * Scale / 2) < 960;
                bool isVMovable = Y + (100 * Scale / 2) < 1055 && Y - (100 * Scale / 2) > 0;
                if (isHMovable && isVMovable)
                {
                    RectX = X - (100 * Scale / 2);
                    RectY = Y - (100 * Scale / 2);
                }
                if (!isHMovable)
                {
                    if (isVMovable)
                    {
                        RectY = Y - (100 * Scale / 2);
                    }
                }
                if (!isVMovable)
                {
                    if (isHMovable)
                    {
                        RectX = X - (100 * Scale / 2);
                    }
                }
            }
        }

        public void RotateSquare(object sender, MouseEventArgs e)
        {
            Vector2 currentPosition = new Vector2((float)X, (float)Y);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                double angleInRadian;
                Vector2 squareCorner;
                Vector2 OriginOfSquare = new Vector2((float)CenterOfSquareX, (float)CenterOfSquareY);
                double m00;
                double m01;
                double m10;
                double m11;
                if (mouseClickedCount == 0)
                {
                    clickedPosition.X = (float)X;
                    clickedPosition.Y = (float)Y;
                    mouseClickedCount++;
                }
                mouseVector = clickedPosition - currentPosition;
                currentPosition.X = (float)X;
                currentPosition.Y = (float)Y;
                angleForSquareRotation = 1.5 * Vector2.Dot(mouseVector, Vector2.UnitY);
                Console.WriteLine("Angle" + angleForSquareRotation);
                Console.WriteLine("OriginOfSquare:" + OriginOfSquare);
                Angle = angleForSquareRotation;
                angleInRadian = Angle * Math.PI / 180;
                m00 = Math.Cos(angleInRadian);
                m01 = -Math.Sin(angleInRadian);
                m10 = Math.Sin(angleInRadian);
                m11 = Math.Cos(angleInRadian);
                Matrix2X2 m2x2 = new Matrix2X2(m00, m01, m10, m11);
                Vector2 vector = new Vector2((float)(-100 * Scale / 2), (float)(100 * Scale / 2));
                squareCorner = (m2x2 * vector) + OriginOfSquare;
                Console.WriteLine(squareCorner);
                RectX = squareCorner.X;
                RectY = squareCorner.Y;
                Console.WriteLine(CenterOfSquareX);
                Console.WriteLine(CenterOfSquareY);
            }
            else
            {
                currentPosition.X = (float)X;
                currentPosition.Y = (float)Y;
                mouseClickedCount = 0;
            }
            Console.WriteLine("Mouse:" + X + "," + Y);
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
