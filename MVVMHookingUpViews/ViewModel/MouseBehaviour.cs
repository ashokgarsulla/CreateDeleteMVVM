using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Numerics;
namespace MVVMHookingUpViews.ViewModel
{
    class MouseBehaviour: Behavior<Canvas>
    {
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
           "MouseY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
           "MouseX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }
        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += MouseMovePosition;
        }
        private void MouseMovePosition(object sender, MouseEventArgs e)
        {
            Canvas cv = (Canvas)sender;
            Vector2 posVector = new Vector2((float)e.GetPosition(cv).X, (float)e.GetPosition(cv).Y);
            MouseX = posVector.X;
            MouseY = posVector.Y;
            if(e.RightButton == MouseButtonState.Pressed)
            {
                cv.CaptureMouse();
            }
            else if(e.RightButton == MouseButtonState.Released)
            {
                cv.ReleaseMouseCapture();
            }  
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= MouseMovePosition;
        }

    }
}
