﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TracerLib.Model;
using TracerLib.TracerContract;

namespace TracerLib.TracerImpl
{
    public class Tracer:ITracer
    {
        public TracedMethodInfo MethodInfo { get; set; }
        private Stopwatch _timer;
        public List<Tracer> Children { get; }

        private static Tracer _tracer;

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

        //private void Traverse(Tracer tn, int level, bool isRoot)
        //{
        //    if (!isRoot)
        //    {
        //        if (tn.MethodInfo == null)
        //        {
        //            return;
        //        }
        //        string result = "";

        //        for (int i = 0; i < level; i++)
        //        {
        //            result += " ";
        //        }
        //        result += tn.MethodInfo.ToString();
        //        Console.WriteLine(result);
        //        level++;
        //    }
        //    foreach (Tracer kid in tn.Children)
        //    {
        //        Traverse(kid, level, false);
        //    }
        //}

        public void Traverse(Tracer tn, int level, bool isRoot, Action<Tracer,int, bool> action)
        {
            action(tn,level,isRoot);
            foreach (Tracer kid in tn.Children)
            {
                Traverse(kid, level, false, action);
            }
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_tracer,true);
        }
    }
}
