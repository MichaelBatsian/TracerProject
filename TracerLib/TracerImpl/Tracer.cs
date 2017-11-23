using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TracerLib.Model;
using TracerLib.TracerContract;

namespace TracerLib.TracerImpl
{
    public class Tracer:ITracer
    {
     
        private TreeNode<TracedMethodInfo> _tree;
        private TreeNode<TracedMethodInfo> _current;
        private Stack<TreeNode<TracedMethodInfo>> _methodsStack;

        private static Tracer _tracer;

        public static Tracer GetInstance()
        {
            if (_tracer == null)
            {
                return new Tracer();
            }
            return _tracer;
        }

        public TreeNode<TracedMethodInfo> GetTree()
        {
            return _tree;
        }


        private Tracer()
        {
            _tree = new TreeNode<TracedMethodInfo>();
            _methodsStack = new Stack<TreeNode<TracedMethodInfo>>();
            _current = _tree;
        }

        public void StartTrace()
        {
            StackTrace st = new StackTrace();
            var invokationFrame = (st.GetFrames())[1];
            var info = new TracedMethodInfo
            {
                ClassName = invokationFrame.GetMethod().DeclaringType.Name,
                MethodName = invokationFrame.GetMethod().Name,
                ParamCountInMethod = invokationFrame
                    .GetMethod()
                    .GetParameters()
                    .Count(),
                Time = Stopwatch.StartNew()
            };
            _current = _current.AddChild(info);
            _methodsStack.Push(_current);
        }

        public void StopTrace()
        {
            _methodsStack.Pop().Data.Time.Stop(); ;
            _current = _methodsStack.Count==0 ? _tree : _methodsStack.Peek();
        }

        public TraceResult GetTraceResult()
        {
            var tr = new TraceResult {ThreadId = Thread.CurrentThread.ManagedThreadId};
            Traverse(_tree,tr,true);
            return tr;
        }

        private void Traverse(TreeNode<TracedMethodInfo> tn,TraceResult tr, bool isRoot)
        {
            if (!isRoot)
            {
                if (tn.Data == null)
                {
                    return;
                }
                tr.ThreadTime += (int)tn.Data.Time.ElapsedMilliseconds;
            }
            foreach (var kid in tn.Children)
            {
                Traverse(kid,tr,false);
            }
        }
    }
}
