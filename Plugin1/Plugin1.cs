using System;
using PluginLoader;

namespace Plugin1
{
    public class Plugin1 : IPlugin
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
