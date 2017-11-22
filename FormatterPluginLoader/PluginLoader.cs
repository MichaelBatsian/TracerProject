using System;
using System.Collections.Generic;
using FormatterContract;
using System.IO;
using System.Reflection;


namespace FormatterPluginLoader
{
    public class PluginLoader
    {
        public static IFormatter<T> LoadPlugin<T>(string path)
        {
            FileInfo file = new FileInfo(path);
            Type interfaceType = typeof(IFormatter<>);
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

                    if (type.GetInterface(interfaceType.FullName, true) != null)
                    {
                        pluginType = type;
                        break;
                    }
                }
                if (pluginType != null)
                {
                    var t = pluginType.MakeGenericType(typeof(T));
                    //return Activator.CreateInstance(pluginType);
                    return (IFormatter<T>)Activator.CreateInstance(t); ;
                }
            }
            return null;
        }

        public static ICollection<IFormatter<T>> LoadPlugins<T>(string path)
        {
            string[] dllFileNames = null;

            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(IFormatter<>);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                ICollection<IFormatter<T>> plugins = new List<IFormatter<T>>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    var t = pluginType.MakeGenericType(typeof(T));
                    IFormatter <T> plugin = (IFormatter <T>)Activator.CreateInstance(t);
                    plugins.Add(plugin);
                }
                return plugins;
            }
            return null;
        }


    }
}
