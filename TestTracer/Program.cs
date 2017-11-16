using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            var p = new Program();
            Tracer tracer = Tracer.GetTracer();
            p.RunTestMethods(tracer);
            p.ConsoleInterface(tracer,args);
            Console.ReadKey();
        }

        public void RunTestMethods(Tracer rootNode)
        {
            MethodLevel1(rootNode.AddChild());
            MethodLevel3(5, rootNode.AddChild());
        }

        public void ConsoleInterface(Tracer tracer,string []args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--o":
                        break;
                    case "--f":
                        Console.WriteLine("Choose format:");
                        Console.WriteLine("1-console view");
                        Console.WriteLine("2-xml format");
                        Console.WriteLine("3-json");
                        string format = Console.ReadLine();
                        switch (format)
                        {
                            case "1":
                                tracer.GetTraceResult().ToConsole();
                                break;
                            case "2":
                                tracer.GetTraceResult().ToXml();
                                break;
                            case "3":
                                tracer.GetTraceResult().ToJson();
                                break;
                            case "4":
                                tracer.GetTraceResult().ToYaml();
                                break;
                            default:
                                Console.WriteLine("Choose right command");
                            break;
                        }
                        break;
                    case "--h":
                        Console.WriteLine("--f - format to introduce tracer results");
                        Console.WriteLine("--o - set the path to save results into file");
                        Console.WriteLine("results:");
                        Console.WriteLine("1-console view");
                        Console.WriteLine("2-xml format");
                        Console.WriteLine("3-json");
                        Console.WriteLine("4-json");

                        break;
                    default:
                        Console.WriteLine("This command is not defined for this application.");
                        Console.WriteLine("To view the list of commands choose key --h");
                        break;
                }
            }
        }
            
        /* Methods to test tracer work*/
        public void MethodLevel1(Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(100);
            MethodLevel2(1, 2, parent.AddChild());
            MethodLevel3(5, parent.AddChild());
            MethodLevel2(1, 2, parent.AddChild());
            MethodLevel2(1, 2, parent.AddChild());
            parent.StopTrace();
        }

        public void MethodLevel2(int a, int b, Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(250);
            MethodLevel3(5, parent.AddChild());
            parent.StopTrace();
        }
        public void MethodLevel3(int a, Tracer parent)
        {
            parent.StartTrace();
            Thread.Sleep(150);
            parent.StopTrace();
        }
    }
}
