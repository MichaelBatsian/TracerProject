using System;
using System.IO;
using System.Linq;
using System.Threading;
using TracerLib.TracerImpl;
using Result;
using TracerLib.Model;


namespace TestTracer
{

    public class Program
    {
        private static Tracer tracer = Tracer.GetInstance();


        static void Main(string[] args)
        {
            bool run = true;
            bool first = true;
            var p = new Program();
            p.RunTestMethods();
            new Thread(()=>p.RunTestMethods()).Start();

            while (run)
            {
                string line =Console.ReadLine();
                var inputArgs = first ? args : line.Split(' ');
                p.ConsoleInterface(inputArgs);
                Console.WriteLine("Continue y/n?");
                Console.WriteLine();
                if (Console.ReadLine().Equals("n"))
                {
                    run = false;
                }
                first = false;
                Console.WriteLine("Enter command:");
            }
        }

        private void RunTestMethods()
        {
            MethodLevel1();
            MethodLevel3(5);
            MethodLevel2(5,3);
        }

        public void ConsoleInterface(string[] args)
        {
            FormatResult<TracedMethodInfo> result = new FormatResult<TracedMethodInfo>(tracer.GetTree(), tracer);
            var plugins = result.GetPlugins();
            var formatsNames = result.GetFormatsNames(plugins);
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--f":
                        string[] outputParams = args.Skip(1).Take(3).ToArray();
                        if (outputParams.Length==0)
                        {
                            DefaultErrorView();
                            break;
                        }
                        try
                        {
                            if (outputParams.Length == 1 && outputParams[0].Equals("console"))
                            {
                                result.ToConsole();
                                break;
                            }
                            if (outputParams.Length == 3 && outputParams[1].Equals("--o"))
                            {
                                if (result.ToSpecialFormat(plugins, outputParams[0], outputParams[2]) == null)
                                {
                                    DefaultErrorView();
                                }
                                break;
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine("Wrong way to save  file");
                        }
                        DefaultErrorView();
                        break;
                    case "--h":
                        Console.WriteLine("--f - format to introduce tracer results");
                        Console.WriteLine("--f [format] --o [path\file name] - set the path to output in file");
                        Console.WriteLine("formats:");
                        foreach (var name in formatsNames)
                        {
                            Console.WriteLine(name);
                        }
                        break;
                    default:
                        DefaultErrorView();
                        break;
                }
            }
        }

        public void DefaultErrorView()
        {
            Console.WriteLine("Wrong command or format.");
            Console.WriteLine("To view the list of commands choose key --h");
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
