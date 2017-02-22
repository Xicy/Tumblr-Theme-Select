using System;
using System.IO;
using System.Linq;
using System.Reflection;
using RestSharp.Extensions;

namespace TumblrThemeSelect
{
    public class Loader
    {
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            App.Main();
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs e)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(new AssemblyName(e.Name).Name + ".dll"));

            if (!resources.Any()) return null;

            try
            {
                return Assembly.Load(thisAssembly.GetManifestResourceStream(resources.First()).ReadAsBytes());
            }
            catch (IOException)
            {
                return null;
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }
    }
}