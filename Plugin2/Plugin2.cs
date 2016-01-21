using System;
using PluginLoader;

namespace Plugin2
{
    public class Plugin2 : IPlugin
    {
        public void Start()
        {
            Console.WriteLine(nameof(this.GetType) + " started.");
        }

        public void Stop()
        {
            Console.WriteLine(nameof(this.GetType) + " stopped.");
        }
    }
}
