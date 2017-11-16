using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerImpl;
using TracerLib.TracerContract;

namespace TracerLib.TracerImpl
{
    public class TraceResult
    {
        private ITracer _tracer;
        private bool isRoot;
        private int level;

        public TraceResult(ITracer tracer, bool isRoot, int level = 0)
        {
            _tracer = tracer;
            this.isRoot = isRoot;
            this.level = 0;
        }

        public void ToJson()
        {
            
        }

        public void ToConsole()
        {
            
        }

        public void ToXml()
        {
            
        }

        public void ToYaml()
        {
            
        }



    }
}
