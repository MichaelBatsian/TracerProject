using System;
using FormatterContract;
using System.IO;
using System.Reflection;

namespace FormatterPluginLoader
{
    public class PluginLoader
    {
        public static IFormatter LoadPlugin(string path)
        {
            FileInfo file = new FileInfo(path);
            Type interfaceType = typeof(IFormatter);
            Type pluginType = null;
            if (file.Exists)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(file.FullName);
                Assembly assembly = Assembly.Load(an);
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }

                    if (type.GetInterface(interfaceType.FullName) != null)
                    {
                        pluginType = type;
                        break;
                    }
                }
                if (pluginType != null)
                {
                    return (IFormatter)Activator.CreateInstance(pluginType);
                }
            }
            return null;
        }
    }
}
