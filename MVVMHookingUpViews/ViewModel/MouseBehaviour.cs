using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
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
            //e.Handled = true;
            Canvas cv = (Canvas)sender;
            Point pos = e.GetPosition(cv);
            MouseX = pos.X;
            MouseY = pos.Y;
            if(e.RightButton == MouseButtonState.Pressed)
            {
               Point B = e.GetPosition(cv);
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
