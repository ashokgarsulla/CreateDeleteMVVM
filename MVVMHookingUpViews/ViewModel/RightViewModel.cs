﻿using System;
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
                    if(isVMovable)
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
            Vector2 currentPosition = new Vector2((float)X,(float)Y);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if(mouseClickedCount == 0)
                {
                    clickedPosition.X = (float)X;
                    clickedPosition.Y = (float)Y;
                    mouseClickedCount++;
                }
                mouseVector = clickedPosition - currentPosition;
                currentPosition.X = (float)X;
                currentPosition.Y = (float)Y;
                angleForSquareRotation = 1.5 * Vector2.Dot(mouseVector, Vector2.UnitY);
                Angle = angleForSquareRotation;
            }
            else
            {
                currentPosition.X = (float)X;
                currentPosition.Y = (float)Y;
                mouseClickedCount = 0;
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
