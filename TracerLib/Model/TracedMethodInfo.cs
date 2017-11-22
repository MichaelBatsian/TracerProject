using System.Text;
using System.Diagnostics;

namespace TracerLib.Model
{
    public class TracedMethodInfo
    {
        public Stopwatch Time { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public int ParamCountInMethod { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("time (ms):{0}, class: {1}, method: {2}, count params: {3}",
                Time.ElapsedMilliseconds, ClassName, MethodName, ParamCountInMethod).ToString();
        }
    }
}
