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
        public Matrix2X2()
        {

        }
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
        private double _x;
        private double _y;

        private Vector2 _clickedPosition;

        private double _rectX;
        private double _rectY;
        
        private double _scale = 1;
        private double _angle = 0;
        private Vector2 _S;
        private double _sideLength = 100;

        private bool _isTranslateActive = false;
        private bool _isRotateActive = false;

        public RightViewModel()
        {
            _S = new Vector2(200, 200);
            Vector2 A_G = CalculateTopLeftCornerOfSquare(_S, _angle, _scale * _sideLength);
            RectX = A_G.X;
            RectY = A_G.Y;
        }

        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChange("X");
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChange("Y");
            }
        }

        public double RectX
        {
            get
            {
                return _rectX;
            }
            set
            {
                _rectX = value;
                OnPropertyChange("RectX");
            }
        }

        public double RectY
        {
            get
            {
                return _rectY;
            }
            set
            {
                _rectY = value;
                OnPropertyChange("RectY");
            }
        }

        public double Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _scale = value;
                OnPropertyChange("Scale");
            }
        }

        public double Side
        {
            get
            {
                return _sideLength;
            }
        }

        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                OnPropertyChange("Angle");
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

            Vector2 A_G = CalculateTopLeftCornerOfSquare(_S, _angle, _sideLength * _scale);

            RectX = A_G.X;
            RectY = A_G.Y;
        }

        private static Vector2 CalculateTopLeftCornerOfSquare(Vector2 center, double angleInDegrees, double sideLength)
        {
            double angleInRadians = angleInDegrees * Math.PI / 180.0;
            Matrix2X2 rotMat = new Matrix2X2(Math.Cos(angleInRadians), -Math.Sin(angleInRadians), Math.Sin(angleInRadians), Math.Cos(angleInRadians));
            Vector2 topLeft_SquareBasis = new Vector2(-(float)sideLength / 2, -(float)sideLength / 2);
            Vector2 topLeft_GlobalBasis = center + (rotMat * topLeft_SquareBasis);
            return topLeft_GlobalBasis;
        }

        public void MoveSquareWithMouse(object sender, MouseEventArgs e)
        {
            if (_isTranslateActive)
                TranslateSquare();

            if (_isRotateActive)
                RotateSquare();
        }

        private void TranslateSquare()
        {
            _S = new Vector2((float)X, (float)Y);
            
            Vector2 A_G = CalculateTopLeftCornerOfSquare(_S, _angle, _sideLength * _scale);
            RectX = A_G.X;
            RectY = A_G.Y;
        }

        public void MouseLeftButtonDownSquare(object sender, MouseEventArgs e)
        {
            _isTranslateActive = true;
            _clickedPosition = new Vector2((float)X, (float)Y);
        }

        public void MouseLeftButtonUpSquare(object sender, MouseEventArgs e)
        {
            _isTranslateActive = false;
        }

        public void MouseRightButtonDownSquare(object sender, MouseEventArgs e)
        {
            _clickedPosition = new Vector2((float)X, (float)Y);
            _isRotateActive = true;
        }

        public void MouseRightButtonUpSquare(object sender, MouseEventArgs e)
        {
            _isRotateActive = false;
        }

        private void RotateSquare()
        {
            double theta;
            Vector2 currentPosition = new Vector2((float)X, (float)Y);
            Vector2 mouseVector = currentPosition - _clickedPosition;
            theta = 0.01 * Vector2.Dot(mouseVector, Vector2.UnitY);
            
            Angle = theta * 180.0 / Math.PI;
            Vector2 A_G = CalculateTopLeftCornerOfSquare(_S, _angle, _sideLength * _scale);

            RectX = A_G.X;
            RectY = A_G.Y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
