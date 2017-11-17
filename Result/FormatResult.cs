using System;
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

        public void ToJson(string path)
        {
            InvokePluginFormatter(path);
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
            InvokePluginFormatter(path);
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
    }
}
