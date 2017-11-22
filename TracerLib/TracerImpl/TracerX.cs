using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TracerLib.Model;
using TracerLib.TracerContract;
using System.Threading;


namespace TracerLib.TracerImpl
{
    public class TracerX:ITracer
    {
        private static TracerX _tracer;

        private Stopwatch _timer;
        private TraceResult traceResult = new TraceResult();
        public List<TracerX> Children { get; }

        public TracedMethodInfo MethodInfo { get; set; }
        
        private TracerX()
        {
            Children = new List<TracerX>();
        }

        public static TracerX GetTracer()
        {
            return _tracer ?? (_tracer = new TracerX());
        }

        public TracerX AddChild()
        {
            var node = new TracerX();
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
                //MsTime = (int)_timer.ElapsedMilliseconds
            };
        }

        public void Traverse(TracerX tn, int level, bool isRoot)
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
                    //traceResult.ThreadTime += tn.MethodInfo.MsTime;
                }

                Console.WriteLine(result);
                level++;
            }
            foreach (TracerX kid in tn.Children)
            {
                Traverse(kid, level, false);
            }
            if (isRoot)
            {
                Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}, Thread time in ms:{traceResult.ThreadTime}");
            }
        }

        public void Traverse(TracerX tn, bool isRoot)
        {
            if (!isRoot)
            {
                if (tn.MethodInfo == null)
                {
                    return;
                }
                //traceResult.ThreadTime += tn.MethodInfo.MsTime;
            }
            foreach (TracerX kid in tn.Children)
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
