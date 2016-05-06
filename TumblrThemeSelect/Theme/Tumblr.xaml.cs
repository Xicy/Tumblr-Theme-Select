using System.Windows;
using System.Windows.Input;

namespace TumblrThemeSelect.Theme
{
    public partial class Tumblr
    {
        private static Window _window;
        private void Window_Loaded(dynamic sender, RoutedEventArgs e)
        {
            _window = sender; 
        }

        private void Window_DragMove(object sender, MouseButtonEventArgs e)
        {
            _window.DragMove();
        }

        private void Window_Minimize(object sender, RoutedEventArgs e)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}