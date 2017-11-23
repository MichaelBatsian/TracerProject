using TracerLib.TracerContract;
using TracerLib.TracerImpl;


namespace FormatterContract
{
    public interface IFormatter<T>
    {
        void Format(TreeNode<T> tree, ITracer tracer, int level,bool isRoot, string savePath);
    }
}
