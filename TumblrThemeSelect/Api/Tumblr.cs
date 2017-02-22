using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TumblrThemeSelect.Val;

namespace TumblrThemeSelect.Api
{
    public class Tumblr
    {
        protected internal RestClient restClient;
        protected internal string uname;
        protected internal string pword;
        protected internal string bname;
        protected internal CustomizeApiValue Cav;

        public Tumblr(string username, string password, string blogname)
        {
            restClient = new RestClient(@"https://www.tumblr.com/") { Encoding = Encoding.UTF8,FollowRedirects = true, CookieContainer = new CookieContainer() };
            uname = username;
            pword = password;
            bname = blogname;

        }

        public Task<bool> Login()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var formKey =
                        new Regex("name=\"tumblr-form-key\".*?content=\"(.*?)\"",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline).Match(restClient.Get(new RestRequest(@"login").AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml")).Content).Groups[1].Value;
                    var customizePageContent =
                    restClient.Post(
                        new RestRequest(@"login").AddParameter("user[email]", uname)
                            .AddParameter("user[password]", pword)
                            .AddParameter("form_key", formKey)
                            .AddParameter("redirect_to", "/customize/" + bname).AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml")).Content;

                    Cav = JsonConvert.DeserializeObject<CustomizeApiValue>(
                            new Regex("Tumblr._init.blog =(.*?);\n", RegexOptions.IgnoreCase | RegexOptions.Multiline)
                                .Match
                                (
                                    customizePageContent).Groups[1].Value);
                    Cav.UserFormKey =
                        new Regex("Tumblr._init.user_form_key = '(.*?)';",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline)
                            .Match(customizePageContent).Groups[1].Value;
                    Cav.SecureFormKey =
                        new Regex("Tumblr._init.secure_form_key = '(.*?)';",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline)
                            .Match(customizePageContent).Groups[1].Value;
                    Cav.Id = Cav.Name;
                    Val.Theme.tumblr = this;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            });
        }

        public Task<List<Val.Theme>> GetThemes()
        {
            return Task<List<Val.Theme>>.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Val.Theme>>(restClient.Get(new RestRequest(@"customize_api/themes")).Content));
        }

    }
}
