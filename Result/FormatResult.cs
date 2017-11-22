using System;
using System.IO;
using System.Reflection;
using FormatterContract;
using TracerLib.TracerImpl;
using XmlFormatter;

namespace Result
{
    public class FormatResult
    {
        private Tracer _tracer;
 
        public FormatResult(Tracer tracer)
        {
            _tracer = tracer;
        }

        public void ToJson(string  path)
        {
            InvokePluginFormatter(GetPath(path));
        }

        public void ToConsole()
        {
            _tracer.Traverse(_tracer, 0, true);
        }

        public void ToXml()
        {
           new FormatterXml().Format(_tracer,0,true);
        }

        public void ToYaml(string path)
        {
            InvokePluginFormatter(GetPath(path));
        }

        private IFormatter GetPlugin(string path)
        {
           return FormatterPluginLoader.PluginLoader.LoadPlugin(path);
        }

        private void InvokePluginFormatter(string path)
        {
            IFormatter formatter = GetPlugin(path);
            formatter.Format(_tracer, 0, true);
        }

        //method to identify path to plugin
        static string GetPath(string pathTodll)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] path = assemblyPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            string[] newPath = new string[path.Length - 3];
            Array.Copy(path, newPath, path.Length - 3);
            return String.Join("\\", newPath) + pathTodll;
        }
    }
}
