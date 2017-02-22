using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace TumblrThemeSelect
{
    public static class Loader
    {
        [STAThread]
        public static void Main()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblies = executingAssembly.GetManifestResourceNames().Where(n => n.EndsWith(".dll")).ToDictionary(n => n, n => Assembly.Load(executingAssembly.GetManifestResourceStream(n).ReadAsBytes()));

            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                return assemblies.FirstOrDefault(kv => kv.Key == $"{new AssemblyName(e.Name).Name}.dll").Value;
            };

            App.Main();
        }

        public static byte[] ReadAsBytes(this Stream input)
        {
            var array = new byte[16384];
            byte[] result;
            using (var memoryStream = new MemoryStream())
            {
                int count;
                while ((count = input.Read(array, 0, array.Length)) > 0)
                {
                    memoryStream.Write(array, 0, count);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

    }
}