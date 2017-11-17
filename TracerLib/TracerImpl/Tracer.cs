using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TracerLib.Model;
using TracerLib.TracerContract;
using System.Threading;


namespace TracerLib.TracerImpl
{
    public class Tracer:ITracer
    {
        private static Tracer _tracer;

        private Stopwatch _timer;
        private TraceResult traceResult = new TraceResult();
        public List<Tracer> Children { get; }

        public TracedMethodInfo MethodInfo { get; set; }
        
        private Tracer()
        {
            Children = new List<Tracer>();
        }

        public static Tracer GetTracer()
        {
            return _tracer ?? (_tracer = new Tracer());
        }

        public Tracer AddChild()
        {
            var node = new Tracer();
            Children.Add(node);
            return node;
        }

        public void StartTrace()
        {
            _timer = Stopwatch.StartNew();
        }

        public void StopTrace()
        {
            _timer.Stop();
            StackTrace st = new StackTrace();
            var invokationFrame = (st.GetFrames())[1];
            MethodInfo = new TracedMethodInfo
            {
                ClassName = invokationFrame.GetMethod().DeclaringType.Name,
                MethodName = invokationFrame.GetMethod().Name,
                ParamCountInMethod = invokationFrame
                    .GetMethod()
                    .GetParameters()
                    .Count(),
                MsTime = (int)_timer.ElapsedMilliseconds
            };
        }

        public void Traverse(Tracer tn, int level, bool isRoot)
        {
            if (!isRoot)
            {
                if (tn.MethodInfo == null)
                {
                    return;
                }
                string result = "";

                for (int i = 0; i < level; i++)
                {
                    result += " ";
                }
                result += tn.MethodInfo.ToString();
                if (level == 0)
                {
                    traceResult.ThreadTime += tn.MethodInfo.MsTime;
                }

                Console.WriteLine(result);
                level++;
            }
            foreach (Tracer kid in tn.Children)
            {
                Traverse(kid, level, false);
            }
            if (isRoot)
            {
                Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}, Thread time in ms:{traceResult.ThreadTime}");
            }
        }

        public void Traverse(Tracer tn, bool isRoot)
        {
            if (!isRoot)
            {
                if (tn.MethodInfo == null)
                {
                    return;
                }
                traceResult.ThreadTime += tn.MethodInfo.MsTime;
            }
            foreach (Tracer kid in tn.Children)
            {
                Traverse(kid, false);
            }
        }

        public TraceResult GetTraceResult()
        {
            Traverse(_tracer, true);
            traceResult.ThreadId = Thread.CurrentThread.ManagedThreadId;
            return traceResult;
        }
    }
}
