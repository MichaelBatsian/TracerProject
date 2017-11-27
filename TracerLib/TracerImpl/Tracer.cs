using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            var info = new TracedMethodInfo
            {
               Time = Stopwatch.StartNew()
            };
            _current = _current.AddChild(info);
            _methodsStack.Push(_current);
        }

        public void StopTrace()
        {
            var st = new StackTrace();
            var invokationFrame = (st.GetFrames())?[1];
            var info = _methodsStack.Pop().Data;
            if (invokationFrame != null)
            {
                var declaringType = invokationFrame.GetMethod().DeclaringType;
                if (declaringType != null)
                {
                    info.ClassName = declaringType.Name;
                }
                info.MethodName = invokationFrame.GetMethod().Name;
                info.ParamCountInMethod = invokationFrame
                    .GetMethod()
                    .GetParameters()
                    .Count();
            }
            info.Time.Stop();
            _current = _methodsStack.Count == 0 ? _tree : _methodsStack.Peek();
        }

        public TraceResult GetTraceResult()
        {
            var tr = new TraceResult {ThreadId = Thread.CurrentThread.ManagedThreadId};
            Traverse(_tree,tr,0,true);
            return tr;
        }

        private void Traverse(TreeNode<TracedMethodInfo> tn,TraceResult tr,int level, bool isRoot)
        {
            if (!isRoot)
            {
                if (tn.Data == null)
                {
                    return;
                }
                if (level == 0)
                {
                    tr.ThreadTime += (int)tn.Data.Time.ElapsedMilliseconds;
                }
                level++;
            }
            foreach (var kid in tn.Children)
            {
                Traverse(kid,tr,level,false);
            }
        }
    }
}
