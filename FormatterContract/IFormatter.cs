using TracerLib.TracerImpl;

namespace FormatterContract
{
    public interface IFormatter
    {
        void Format(Tracer tracer, int level,bool isRoot);
    }
}
