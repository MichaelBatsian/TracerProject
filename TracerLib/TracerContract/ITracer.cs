using TracerLib.TracerImpl;

namespace TracerLib.TracerContract
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }
}
