using System;
using FormatterContract;
using TracerLib.TracerImpl;
using XmlFormatter;

namespace Result
{
    public class TraceResult
    {
        private Tracer _tracer;
 
        public TraceResult(Tracer tracer)
        {
            _tracer = tracer;
        }

        public void ToJson(string path)
        {
            InvokePluginFormatter(path);
        }

        public void ToConsole()
        {
            Action<Tracer, int, bool> action = (t, l, isR) =>
            {
                if (isR)
                {
                    return;
                }
                if (t.MethodInfo == null)
                {
                    return;
                }
                string result = "";

                for (int i = 0; i < l; i++)
                {
                    result += " ";
                }
                result += t.MethodInfo.ToString();
                Console.WriteLine(result);
                l++;
            };
            _tracer.Traverse(_tracer, 0, true, action);
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
