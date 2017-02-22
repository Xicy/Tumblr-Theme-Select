using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TumblrThemeSelect.Api;
using TumblrThemeSelect.Val;

namespace TumblrThemeSelect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tumblr t;
        private ICollectionView ListBoxThemeFilter;
        private bool CurrentTheme;

        public MainWindow()
        {
            InitializeComponent();
            EllipseLoading.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2)) { RepeatBehavior = RepeatBehavior.Forever });
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text) || string.IsNullOrWhiteSpace(TextBoxPassword.Password) || string.IsNullOrWhiteSpace(TextBoxBlogName.Text))
            {
                GridHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(1, 1.1, TimeSpan.FromMilliseconds(200)) { AutoReverse = true });
                return;
            }

            GridLoading.Visibility = Visibility.Visible;
            t = new Tumblr(TextBoxUserName.Text, TextBoxPassword.Password, TextBoxBlogName.Text);

            if (!await t.Login())
            {
                GridLoading.Visibility = Visibility.Collapsed;
                GridHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(1, 1.1, TimeSpan.FromMilliseconds(200)) { AutoReverse = true });
                return;
            }

            await t.GetThemes().ContinueWith(c => { ListBoxThemeFilter = CollectionViewSource.GetDefaultView(c.Result); });
            ListBoxTheme.ItemsSource = ListBoxThemeFilter;
            ListBoxTheme.SelectedIndex = 0;

            GridLoading.Visibility = Visibility.Collapsed;
            BeginAnimation(HeightProperty, new DoubleAnimation(Height, 550, TimeSpan.FromSeconds(1)));

            GridLogin.Visibility = Visibility.Collapsed;
            GridThemes.Visibility = Visibility.Visible;
        }
        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            ListBoxTheme.ItemsSource = null;
            StackPanelDefaultParam.Children.Clear();
            BeginAnimation(HeightProperty, new DoubleAnimation(Height, 150, TimeSpan.FromSeconds(1)));
            GridLogin.Visibility = Visibility.Visible;
            GridThemes.Visibility = Visibility.Collapsed;
            GridCustomize.Visibility = Visibility.Collapsed;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentTheme = false;
            GridLoading.Visibility = Visibility.Visible;
            GridThemes.Visibility = Visibility.Visible;
            GridCustomize.Visibility = Visibility.Collapsed;
            GridLoading.Visibility = Visibility.Collapsed;
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            GridLoading.Visibility = Visibility.Visible;
            t.Cav.CustomParams.Clear();
            foreach (dynamic param in StackPanelDefaultParam.Children) { t.Cav.CustomParams[param.ID] = param.Value; }
            await ((Val.Theme)ListBoxTheme.SelectedItem)?.ApplyTheme(CurrentTheme);
            BeginAnimation(WidthProperty, new DoubleAnimation(Width, 306, TimeSpan.FromSeconds(1)));
            GridLoading.Visibility = Visibility.Collapsed;
        }
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            LoadEditing(((Val.Theme)ListBoxTheme.SelectedItem)?.DefaultParams);
        }
        private void ButtonCurrentThemeSettings_Click(object sender, RoutedEventArgs e)
        {
            LoadEditing(t.Cav.CustomParams);
        }
        private void LoadEditing(IDictionary<string, object> data)
        {
            GridLoading.Visibility = Visibility.Visible;
            StackPanelDefaultParam.Children.Clear();
            foreach (var param in data)
            {
                if (param.Key.StartsWith("select_lists")) { foreach (dynamic select in (IEnumerable)param.Value) { StackPanelDefaultParam.Children.Add(new ParametrePanel(select.Name, select.Value, t, GridLoading)); } }
                else if (param.Key.StartsWith("select:")) { }
                else { StackPanelDefaultParam.Children.Add(new ParametrePanel(param.Key, param.Value, t, GridLoading)); }
            }
            CurrentTheme = true;
            GridThemes.Visibility = Visibility.Collapsed;
            GridCustomize.Visibility = Visibility.Visible;
            GridLoading.Visibility = Visibility.Collapsed;
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBoxThemeFilter.Filter = t => ((Val.Theme)t).Title.ToLowerInvariant().Contains(TextBoxSearch.Text.ToLowerInvariant());
        }

    }
}
