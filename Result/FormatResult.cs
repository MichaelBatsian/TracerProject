using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        public void ToJson()
        {
            InvokePluginFormatter(PathToJson);
        }

        public void ToConsole()
        {
            _tree.Traverse(_tree, 0, true);
        }

        public void ToXml()
        {
           new FormatterXml<T>().Format(_tree, _tracer, 0, true);
        }

        public void ToYaml()
        {
            InvokePluginFormatter(PathToYaml);
        }
        //разобраться с xml
        public string ToSpecialFormat(ICollection<IFormatter<T>> plugins, string formatName)
        {
            switch (formatName)
            {
                case "console":
                    ToConsole();
                    break;
                case "xml":
                    ToXml();
                    break;
                default:
                    string pathToPlugins = FormatResult<T>.GetAbsolutePath("Plugins");
                    foreach (var plugin in plugins)
                    {
                        Type type = plugin.GetType();
                        if (type.Name.Contains(formatName))
                        {
                            plugin.Format(_tree, _tracer, 0, true);
                            return formatName;
                        }
                    }
                    break;
            }
            return null;
        }

        public List<string> GetFormatsNames(ICollection<IFormatter<T>> plugins)
        {
            List<string> names = new List<string> {"xml"};
            foreach (var plugin in plugins)
            {
                if (plugin.GetType().Name.Contains("json"))
                {
                    names.Add("json");
                }
                if (plugin.GetType().Name.Contains("yaml"))
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

        private void InvokePluginFormatter(string path)
        {
            IFormatter<T> formatter = GetPlugin(GetAbsolutePath(path));
            formatter.Format(_tree, _tracer, 0, true);
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
