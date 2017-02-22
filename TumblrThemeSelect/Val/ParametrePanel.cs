using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using RestSharp;
using TumblrThemeSelect.Api;
using Xceed.Wpf.Toolkit;

namespace TumblrThemeSelect.Val
{
    public class ParametrePanel : StackPanel
    {
        public string ID;
        public object Value;

        private static string FromColor(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
        private static Color FromHex(string hex)
        {
            if (hex.StartsWith("#")) hex = hex.Substring(1);

            if (string.IsNullOrWhiteSpace(hex))
            {
                return Colors.Transparent;
            }
            else if (hex.Length == 3)
            {
                return Color.FromRgb(byte.Parse(hex.Substring(0, 1) + hex.Substring(0, 1), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(1, 1) + hex.Substring(1, 1), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(2, 1) + hex.Substring(2, 1), System.Globalization.NumberStyles.HexNumber));
            }
            else if (hex.Length == 6)
            {
                return Color.FromRgb(byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            }
            else if (hex.Length == 8)
            {
                return Color.FromArgb(byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
            }
            else return Colors.Transparent;
        }
        private static bool CheckBool(dynamic b) => b is bool ? b : (b is string ? b.Contains("1") : false);
        private static bool CheckUrlValid(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public ParametrePanel(string id, dynamic value, Tumblr t, UIElement loading)
        {
            Orientation = Orientation.Horizontal;
            Margin = new Thickness(0, 2, 0, 2);
            Height = 25;

            ID = id;
            Value = value;

            dynamic val = null;
            if (id.StartsWith("text")) { val = new TextBox { Text = value }; ((TextBox)val).TextChanged += delegate { Value = val.Text; }; }
            else if (id.StartsWith("font")) { val = new TextBox { Text = value }; ((TextBox)val).TextChanged += delegate { Value = val.Text; }; }
            else if (id.StartsWith("image"))
            {
                val = new StackPanel { Orientation = Orientation.Horizontal };
                var buttonUpload = new Button { Content = "Upload", FontSize = 12, Background = (SolidColorBrush)Application.Current.FindResource("ColorButtonGrey") };
                var buttonShow = new Button { Content = "Show", Margin = new Thickness(0, 0, 2, 0), FontSize = 12, Background = (SolidColorBrush)Application.Current.FindResource("ColorButtonGrey"), Visibility = CheckUrlValid((string)Value) ? Visibility.Visible : Visibility.Collapsed }; buttonShow.Click += delegate { Process.Start((string)Value); };
                var buttonRemove = new Button { Content = "X", Margin = new Thickness(2, 0, 0, 0), FontSize = 12, Background = (SolidColorBrush)Application.Current.FindResource("ColorButtonGrey"), Visibility = CheckUrlValid((string)Value) ? Visibility.Visible : Visibility.Collapsed }; buttonRemove.Click += delegate { Value = ""; buttonShow.Visibility = Visibility.Collapsed; buttonRemove.Visibility = Visibility.Collapsed; buttonUpload.Background = (SolidColorBrush)Application.Current.FindResource("ColorButtonGrey"); buttonUpload.Content = "Upload"; };
                buttonUpload.Click += async (s, e) =>
                {
                    var ofp = new OpenFileDialog();
                    loading.Visibility = Visibility.Visible;
                    ofp.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;";
                    ofp.Title = "Image Select";
                    var showDialog = ofp.ShowDialog();
                    if (showDialog.HasValue && showDialog.Value)
                    {
                        await Task.Factory.StartNew(() => { Value = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageContent>(t.restClient.Post(new RestRequest("svc/post/upload_photo").AddHeader("x-tumblr-form-key", t.Cav.UserFormKey).AddFile("photo", ofp.FileName)).Content).Response[0].Url; });
                        buttonUpload.Background = (SolidColorBrush)Application.Current.FindResource("ColorButtonGreen");
                        buttonUpload.Content = "Uploaded";
                        buttonShow.Visibility = Visibility.Visible;
                        buttonRemove.Visibility = Visibility.Visible;
                    }
                    loading.Visibility = Visibility.Collapsed;
                };
                val.Children.Add(buttonShow);
                val.Children.Add(buttonUpload);
                val.Children.Add(buttonRemove);
            }
            else if (id.StartsWith("color")) { val = new ColorPicker { SelectedColor = FromHex(value), ColorMode = ColorMode.ColorCanvas }; ((ColorPicker)val).SelectedColorChanged += delegate { Value = FromColor(val.SelectedColor); }; }
            else if (id.StartsWith("if")) { val = new CheckBox { IsChecked = CheckBool(value), VerticalAlignment = VerticalAlignment.Center }; ((CheckBox)val).Checked += delegate { Value = val.IsChecked ? "1" : "0"; }; }
            else if (id.StartsWith("select")) { val = new ComboBox { DisplayMemberPath = "[1]" }; ((ComboBox)val).SelectionChanged += delegate { Value = val.SelectedItem[0]; }; foreach (dynamic selectparam in value) { val.Items.Add(new object[] { selectparam.Name, selectparam.Value.Value }); } val.SelectedIndex = 0; }

            var settingName = id.Split(':')[1];
            var label = new TextBlock
            {
                Text = settingName,
                ToolTip = settingName,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 110,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Foreground = new SolidColorBrush(Colors.White),
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                FontWeight = FontWeight.FromOpenTypeWeight(300)
            };
            Children.Add(label);
            val.Width = 150;
            Children.Add(val);
        }
    }
}