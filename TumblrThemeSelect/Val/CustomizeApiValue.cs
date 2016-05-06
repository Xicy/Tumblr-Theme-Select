using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Controls;

namespace TumblrThemeSelect.Val
{
    [DataContract]
    public class CustomizeApiValue
    {
        [DataMember(Name = "custom_css")]
        public string CustomCss { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "enable_url_slugs")]
        public int EnableUrlSlugs { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "open_links_in_new_window")]
        public int OpenLinksInNewWindow { get; set; }
        [DataMember(Name = "posts_per_page")]
        public string PostsPerPage { get; set; }
        [DataMember(Name = "theme_id")]
        public string ThemeId { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "truncate_feed")]
        public int TruncateFeed { get; set; }
        [DataMember(Name = "global_params")]
        public Dictionary<string, object> GlobalParams { get; set; }
        [DataMember(Name = "custom_params")]
        public Dictionary<string, object> CustomParams { get; set; }
        [DataMember(Name = "custom_theme")]
        public string CustomTheme { get; set; }
        [DataMember(Name = "purchased_theme_ids")]
        public IList<object> PurchasedThemeIds { get; set; }
        [DataMember(Name = "enable_mobile_interface")]
        public bool EnableMobileInterface { get; set; }
        [DataMember(Name = "brag")]
        public bool Brag { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "theme_author_prompt")]
        public bool ThemeAuthorPrompt { get; set; }
        [DataMember(Name = "PortraitURL-16")]
        public string PortraitUrl16 { get; set; }
        [DataMember(Name = "PortraitURL-24")]
        public string PortraitUrl24 { get; set; }
        [DataMember(Name = "PortraitURL-30")]
        public string PortraitUrl30 { get; set; }
        [DataMember(Name = "PortraitURL-40")]
        public string PortraitUrl40 { get; set; }
        [DataMember(Name = "PortraitURL-48")]
        public string PortraitUrl48 { get; set; }
        [DataMember(Name = "PortraitURL-64")]
        public string PortraitUrl64 { get; set; }
        [DataMember(Name = "PortraitURL-96")]
        public string PortraitUrl96 { get; set; }
        [DataMember(Name = "PortraitURL-128")]
        public string PortraitUrl128 { get; set; }
        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "user_form_key")]
        public string UserFormKey { get; set; }
        [DataMember(Name = "secure_form_key")]
        public string SecureFormKey { get; set; }
    }
}