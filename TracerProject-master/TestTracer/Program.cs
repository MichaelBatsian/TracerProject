using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading;
using TracerLib;
using TracerLib.TracerImpl;
using XmlFormatter;
using JsonFormatter;
using  YamlFormatter;


namespace TestTracer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
           //TracerResult tr = new TracerResult();
            //tr.ToJson2();
            Console.ReadKey();

        }

        public void Run()
        {
            Tracer rootNode = Tracer.GetTracer();
            MethodLevel1(rootNode.AddChild());
            MethodLevel3(5, rootNode.AddChild());
            //rootNode.GetTraceResult();
            //FormatterXml xml = new FormatterXml();
            //xml.Format(rootNode, 0,true);
            //FormatterJson json = new FormatterJson();
            //json.Format(rootNode,0,true);
            FormatterYaml yaml = new FormatterYaml();
            yaml.Format(rootNode, 0, true);


        }

        public void MethodLevel1(Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(100);
            MethodLevel2(1, 2, parent.AddChild());
            MethodLevel3(5, parent.AddChild());
            MethodLevel2(1, 2, parent.AddChild());
            MethodLevel2(1, 2, parent.AddChild());
            parent.StopTrace(parent);
        }

        public void MethodLevel2(int a, int b, Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(250);
            MethodLevel3(5, parent.AddChild());
            parent.StopTrace(parent);
        }
        public void MethodLevel3(int a, Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(150);
            parent.StopTrace(parent);
        }
    }
}
