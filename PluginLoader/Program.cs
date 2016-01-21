using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PluginLoader
{
    class Program
    {
        private static readonly string PluginFolder = "plugin";

        static void Main(string[] args)
        {
            LoadPlugins(PluginFolder);
            Console.Read();
        }

        static void LoadPlugins(string pluginFolder)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + pluginFolder;
            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    // Load this assembly
                    var loadedAssembly = Assembly.LoadFile(dll);

                    // Does this assembly contains a server ?
                    foreach (
                        var pluginType in
                            loadedAssembly.GetTypes().Where(t => t != typeof(IPlugin) && typeof(IPlugin).IsAssignableFrom(t)))
                    {
                        Console.Write("Plugin detected : " + loadedAssembly);
                        ThreadPool.QueueUserWorkItem(LoadPlugin, pluginType);
                    }
                }
                catch (FileLoadException)
                {
                    // The Assembly has already been loaded.
                    // Eat it
                    continue;
                }
                catch (BadImageFormatException)
                {
                    // If a BadImageFormatException exception is thrown, the file is not an assembly.
                    // Eat it
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during assembly loading " + ex);
                }
            }
        }

        static void LoadPlugin(object pluginType)
        {
            try
            {
                // Instanciate this server
                var plugin = (IPlugin)Activator.CreateInstance((Type)pluginType);

                Console.WriteLine("Server " + plugin + " successfully loaded");

                // Do work
                plugin.Start();
                plugin.Stop();

                Console.WriteLine("Plugin job done\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during server loading " + ex);
            }
        }
    }
}
