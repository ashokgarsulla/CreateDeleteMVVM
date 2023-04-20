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
        private double M00;
        private double M01;
        private double M10;
        private double M11;
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
        public static Matrix2X2 operator *(Matrix2X2 m1, Matrix2X2 m2)
        {
            Matrix2X2 result = new Matrix2X2
            {
                M00 = m1.M00 * m2.M00 + m1.M01 * m2.M10,
                M01 = m1.M00 * m2.M01 + m1.M01 * m2.M11,
                M10 = m1.M10 * m2.M00 + m1.M11 * m2.M10,
                M11 = m1.M10 * m2.M01 + m1.M11 * m2.M11
            };

            return result;
        }

    }
    public class RightViewModel : INotifyPropertyChanged
    {
        private double _x;
        private double _y;

        private double _sx;
        private double _sy;
        private double _ax;
        private double _ay;

        //private int _mouseClickedCount = 0;
        private Vector2 _clickedPosition;
        //private bool _capture = false;

        //private Vector2 _topLeftOfSquare;
        private double _rectX;
        private double _rectY;
        
        private double _scale = 1;
        private double _angle = 0;
        private Vector2 _S;
        private double _theta = 0;

        private double _sideLength = 100;

        private bool _isTranslateActive = false;
        private bool _isRotateActive = false;

        public RightViewModel()
        {
            _S = new Vector2(200, 200);
            Vector2 A_G = calculateTopLeftCornerOfSquare(_S, _theta, _scale * _sideLength);
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

        public double ClickPosX
        {
            get
            {
                return _clickedPosition.X;
            }
        }

        public double ClickPosY
        {
            get
            {
                return _clickedPosition.Y;
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

        public double SX
        {
            get
            {
                return _sx;
            }

            set
            {
                _sx = value;
                OnPropertyChange("SX");
            }
        }

        public double SY
        {
            get
            {
                return _sy;
            }

            set
            {
                _sy = value;
                OnPropertyChange("SY");
            }
        }

        public double AX
        {
            get
            {
                return _ax;
            }

            set
            {
                _ax = value;
                OnPropertyChange("AX");
            }
        }

        public double AY
        {
            get
            {
                return _ay;
            }

            set
            {
                _ay = value;
                OnPropertyChange("AY");
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

            Vector2 A_G = calculateTopLeftCornerOfSquare(_S, _theta, _sideLength * _scale);

            RectX = A_G.X;
            RectY = A_G.Y;

            SX = _S.X;
            SY = _S.Y;

            AX = A_G.X;
            AY = A_G.Y;
        }

        private static Vector2 calculateTopLeftCornerOfSquare(Vector2 center, double theta, double sideLength)
        {
            Matrix2X2 rotMat = new Matrix2X2(Math.Cos(theta), -Math.Sin(theta), Math.Sin(theta), Math.Cos(theta));
            Vector2 topLeft_SquareBasis = new Vector2(-(float)sideLength / 2, -(float)sideLength / 2);
            Vector2 topLeft_GlobalBasis = center + (rotMat * topLeft_SquareBasis);

            Console.WriteLine("Center:" + center);
            Console.WriteLine("Theta:"+theta);
            Console.WriteLine("sideLength: "+sideLength);
            return topLeft_GlobalBasis;
        }

        //public void MoveSquareWithMouse(object sender, MouseEventArgs e)
        //{
        //    if (_capture)
        //    {
        //        Vector2 currentPosition = new Vector2((float)X, (float)Y);
        //        Vector2 translation;
        //        translation = _topLeftOfSquare + (currentPosition - _clickedPosition);

        //        RectX = translation.X;
        //        RectY = translation.Y;
        //    }
        //}

        public void MoveSquareWithMouse(object sender, MouseEventArgs e)
        {
            if (_isTranslateActive)
                translateSquare();

            if (_isRotateActive)
                rotateSquare();
        }

        private void translateSquare()
        {
            _S = new Vector2((float)X, (float)Y);
            
            Vector2 A_G = calculateTopLeftCornerOfSquare(_S, _theta, _sideLength * _scale);
            RectX = A_G.X;
            RectY = A_G.Y;

            SX = _S.X;
            SY = _S.Y;

            AX = A_G.X;
            AY = A_G.Y;
        }

        //public void MouseLeftButtonDownSquare(object sender, MouseEventArgs e)
        //{
        //    _capture = true;
        //    _clickedPosition.X = (float)X;
        //    _clickedPosition.Y = (float)Y;
        //    //_topLeftOfSquare.X = (float)RectX;
        //    //_topLeftOfSquare.Y = (float)RectY;
        //}

        //public void MouseLeftButtonUpSquare(object sender, MouseEventArgs e)
        //{
        //    _capture = false;
        //}

        public void MouseLeftButtonDownSquare(object sender, MouseEventArgs e)
        {
            _isTranslateActive = true;
            _clickedPosition = new Vector2((float)X, (float)Y);
            OnPropertyChange("ClickPosX");
            OnPropertyChange("ClickPosY");
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

        //public void RotateSquare(object sender, MouseEventArgs e)
        //{
        //    if (e.RightButton == MouseButtonState.Pressed)
        //    {
        //        double m00;
        //        double m01;
        //        double m10;
        //        double m11;
        //        double angleInRadian;
        //        Vector2 squareCorner;
        //        Vector2 mouseVector;
        //        Vector2 OriginOfSquareOnCanvasBasis = new Vector2(150, 150);

        //        if (_mouseClickedCount == 0)
        //        {
        //            _clickedPosition.X = (float)X;
        //            _clickedPosition.Y = (float)Y;
        //            _mouseClickedCount++;
        //        }
        //        Vector2 currentPosition = new Vector2((float)X, (float)Y);
        //        mouseVector = _clickedPosition - currentPosition;
        //        Angle = 1.5 * Vector2.Dot(mouseVector, Vector2.UnitY); 
        //        angleInRadian = Angle * Math.PI / 180;

        //        m00 = Math.Cos(angleInRadian);
        //        m01 = -Math.Sin(angleInRadian);
        //        m10 = Math.Sin(angleInRadian);
        //        m11 = Math.Cos(angleInRadian);

        //        Matrix2X2 rotationMatrix = new Matrix2X2(m00, m01, m10, m11);
        //        Vector2 vector = new Vector2((float)(-100 * Scale / 2), (float)(-100 * Scale / 2));

        //        squareCorner = (rotationMatrix * vector) + OriginOfSquareOnCanvasBasis;
        //        RectX = (int)squareCorner.X;
        //        RectY = (int)squareCorner.Y;
        //        Console.WriteLine(squareCorner);
        //    }
        //    else
        //    {
        //        _mouseClickedCount = 0;
        //    }
        //}
        private void rotateSquare()
        {
            Vector2 currentPosition = new Vector2((float)X, (float)Y);
            Vector2 mouseVector = currentPosition - _clickedPosition;
            _theta = 0.01 * Vector2.Dot(mouseVector, Vector2.UnitY);
            
            Vector2 A_G = calculateTopLeftCornerOfSquare(_S, _theta, _sideLength * _scale);

            Angle = _theta * 180.0 / Math.PI;

            RectX = A_G.X;
            RectY = A_G.Y;

            SX = _S.X;
            SY = _S.Y;

            AX = A_G.X;
            AY = A_G.Y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
