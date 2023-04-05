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
            Point pos = e.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;    
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= MouseMovePosition;
        }

    }
}
