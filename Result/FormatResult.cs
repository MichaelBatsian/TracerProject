using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ConsoleFormatter;
using FormatterContract;
using TracerLib.TracerContract;
using TracerLib.TracerImpl;
using XmlFormatter;

namespace Result
{
    public class FormatResult<T>
    {
        private TreeNode<T> _tree;
        private ITracer _tracer;

        public const string PathToJson = "\\JsonFormatter\\bin\\Debug\\JsonFormatter.dll"; 
        public const string PathToYaml = "\\YamlFormatter\\bin\\Debug\\YamlFormatter.dll";
        public const string PathToPlugins = "\\Plugins";

        public FormatResult(TreeNode<T> tree, ITracer tracer)
        {
            _tree = tree;
            _tracer = tracer;
        }

        public void ToJson(string savePath)
        {
            InvokePluginFormatter(PathToJson, savePath);
        }

        public void ToConsole(string output=null)
        {
            new FormatterConsole<T>().Format(_tree, _tracer, 0, true, output);
        }

        public void ToXml(string savePath)
        {
           new FormatterXml<T>().Format(_tree, _tracer, 0, true, savePath);
        }

        public void ToYaml(string savePath)
        {
            InvokePluginFormatter(PathToYaml, savePath);
        }
       
        public string ToSpecialFormat(ICollection<IFormatter<T>> plugins, string formatName, string output)
        {
            string result = null;
            switch (formatName)
            {
                case "console":
                    ToConsole(output);
                    result = formatName;
                    break;
                case "xml":
                    ToXml(output);
                    result = formatName;
                    break;
                default:
                    string pathToPlugins = FormatResult<T>.GetAbsolutePath("Plugins");
                    if (plugins.Count == 0)
                    {
                        break;
                    }
                    foreach (var plugin in plugins)
                    {
                        Type type = plugin.GetType();
                        if (type.Name.ToLower().Contains(formatName))
                        {
                            plugin.Format(_tree, _tracer, 0, true, output);
                            return formatName;
                        }
                    }
                    break;
            }
            return result;
        }

        public List<string> GetFormatsNames(ICollection<IFormatter<T>> plugins)
        {
            List<string> names = new List<string> {"xml"};
            foreach (var plugin in plugins)
            {
                if (plugin.GetType().Name.Contains("Json"))
                {
                    names.Add("json");
                }
                if (plugin.GetType().Name.Contains("Yaml"))
                {
                    names.Add("yaml");
                }
            }
            return names;
        }


        private IFormatter<T> GetPlugin(string path)
        {
            return FormatterPluginLoader.PluginLoader.LoadPlugin<T>(path);
        }

        public ICollection<IFormatter<T>> GetPlugins()
        {
            return FormatterPluginLoader.PluginLoader.LoadPlugins<T>(GetAbsolutePath(PathToPlugins));
        }

        private void InvokePluginFormatter(string path,string savePath)
        {
            IFormatter<T> formatter = GetPlugin(GetAbsolutePath(path));
            formatter.Format(_tree, _tracer, 0, true, savePath);
        }

        public static string GetAbsolutePath(string relativePath)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] path = assemblyPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            string[] newPath = new string[path.Length - 3];
            Array.Copy(path, newPath, path.Length - 3);
            return String.Join("\\", newPath) + relativePath;
        }
    }
}
