using TracerLib.TracerImpl;


namespace TracerLib.Model
{
    public class ThreadInfo<T>
    {
        public TreeNode<T> Root { get; set; }
        public TreeNode<T> Current { get; set; }
        public int ThreadId { get; set; }
        public int ExecutingTime { get; set; }
    }
}
