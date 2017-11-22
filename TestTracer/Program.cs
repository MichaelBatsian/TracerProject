using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using FormatterContract;
using TracerLib.TracerImpl;
using  Result;
using TracerLib.Model;


namespace TestTracer
{
    class Program
    {
        private static Tracer tracer = Tracer.GetInstance();

        [STAThread]
        static void Main(string[] args)
        {
            var p = new Program();
            p.RunTestMethods();
            //p.ConsoleInterface(args);
            FormatResult<TracedMethodInfo> result = new FormatResult<TracedMethodInfo>(tracer.GetTree(), tracer);
            Console.ReadKey();
        }

        private void RunTestMethods()
        {
            MethodLevel1();
            MethodLevel3(5);
            MethodLevel2(5,3);
        }

        public void ConsoleInterface(string [] args)
        {
            FormatResult<TracedMethodInfo> result = new FormatResult<TracedMethodInfo>(tracer.GetTree(),tracer);
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--f":
                        bool exit = true;
                        while (exit)
                        {
                            Console.WriteLine("Choose format:");
                            Console.WriteLine("1 - console view");
                            Console.WriteLine("2 - xml format");
                            Console.WriteLine("3 - json");
                            Console.WriteLine("4 - yaml");
                            Console.WriteLine("5 - exit");
                            string format = Console.ReadLine();
                            switch (format)
                            {
                                case "1":
                                    result.ToConsole();
                                    Console.WriteLine("Done.");
                                    break;
                                case "2":
                                    result.ToXml();
                                    Console.WriteLine("Done.");
                                    break;
                                case "3":
                                    result.ToJson();
                                    Console.WriteLine("Done.");
                                    break;
                                case "4":
                                    result.ToYaml();
                                    Console.WriteLine("Done.");
                                    break;
                                case "5":
                                    exit = false;
                                    break;
                                default:
                                    Console.WriteLine("Choose right command");
                                break;
                            }
                        }
                        break;
                    case "--h":
                        Console.WriteLine("--f - format to introduce tracer results");
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

        public void ConsoleInterface2(string[] args)
        {
            FormatResult<TracedMethodInfo> result = new FormatResult<TracedMethodInfo>(tracer.GetTree(), tracer);
            var plugins = result.GetPlugins();
            var formatsNames = result.GetFormatsNames(plugins);
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--f":
                        string format = Console.ReadLine();
                        result.ToSpecialFormat(plugins, format);
                        if (args.Length == 4)
                        {
                            
                        }
                        break;
                    case "--h":
                        Console.WriteLine("--f - format to introduce tracer results");
                        Console.WriteLine("--f [format] --o [path\\file name] - set the path to output in file");
                        Console.WriteLine("formats:");
                        foreach (var name in formatsNames)
                        {
                            Console.WriteLine(name);
                        }
                        break;
                    default:
                        Console.WriteLine("This command is not defined for this application.");
                        Console.WriteLine("To view the list of commands choose key --h");
                        break;
                }
            }
        }
        /* Methods to test tracer work*/
        public void MethodLevel1()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            MethodLevel2(1, 2);
            MethodLevel3(5);
            MethodLevel3(5);
            MethodLevel2(1, 2);
            MethodLevel2(1, 2);
            tracer.StopTrace();
        }

        public void MethodLevel2(int a, int b)
        {
            tracer.StartTrace();
            Thread.Sleep(250);
            MethodLevel3(5);
            tracer.StopTrace();
        }
        public void MethodLevel3(int a)
        {
            tracer.StartTrace();
            Thread.Sleep(150);
            tracer.StopTrace();
        }
    }
}
