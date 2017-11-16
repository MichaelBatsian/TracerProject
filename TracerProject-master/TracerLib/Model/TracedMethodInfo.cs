using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerLib.Model
{
    public class TracedMethodInfo
    {
        public int MsTime { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public int ParamCountInMethod { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("time (ms):{0}, class: {1}, method: {2}, count params: {3}",
                MsTime, ClassName, MethodName, ParamCountInMethod).ToString();
        }
    }
}
