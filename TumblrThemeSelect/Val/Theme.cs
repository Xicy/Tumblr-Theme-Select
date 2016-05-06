using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RestSharp;
using TumblrThemeSelect.Api;

namespace TumblrThemeSelect.Val
{
    [DataContract]
    public class Theme
    {
        protected internal static Tumblr tumblr;

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "author_user_id")]
        public string AuthorUserId { get; set; }
        [DataMember(Name = "wide_thumbnail_key")]
        public string WideThumbnailKey { get; set; }
        [DataMember(Name = "premium")]
        public string Premium { get; set; }
        [DataMember(Name = "default_params")]
        public Dictionary<string, object> DefaultParams { get; set; }
        [DataMember(Name = "author_name")]
        public string AuthorName { get; set; }
        [DataMember(Name = "author_blog")]
        public string AuthorBlog { get; set; }
        [DataMember(Name = "theme")]
        public string ThemeCode { get; set; }

        [IgnoreDataMember]
        private ImageSource _ImageSource;
        [IgnoreDataMember]
        public ImageSource ImageSource => _ImageSource ?? (_ImageSource = new BitmapImage(new Uri($"https://33.media.tumblr.com/themes/wide/{WideThumbnailKey}.png")) { CacheOption = BitmapCacheOption.OnLoad, DecodePixelHeight = 173, DecodePixelWidth = 260 });

        public Task<CustomizeApiValue> ApplyTheme(bool CurrentTheme)
        {
            return Task<CustomizeApiValue>.Factory.StartNew(() =>
            {
                if (!CurrentTheme)
                {
                    tumblr.Cav.CustomTheme =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<Theme>(
                            tumblr.restClient.Post(
                                new RestRequest($"customize_api/theme/{tumblr.Cav.Name}/{Id}").AddHeader("referer",
                                    "https://www.tumblr.com/customize/")).Content).ThemeCode;

                }
                tumblr.restClient.Post(new RestRequest($"customize_api/blog/{tumblr.Cav.Name}").AddHeader("referer", $"https://www.tumblr.com/customize/{tumblr.Cav.Name}").AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(tumblr.Cav), ParameterType.RequestBody));
                return tumblr.Cav;
            });
        }
    }
}