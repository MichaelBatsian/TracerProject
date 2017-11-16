using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerImpl;

namespace TracerLib.TracerContract
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace(ITracer tracer);
        TraceResult GetTraceResult(ITracer tn, bool isRoot,int level=0);
    }
}
