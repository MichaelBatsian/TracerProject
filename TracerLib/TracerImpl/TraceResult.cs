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
        private Tracer _tracer;
        private bool isRoot;
        private int level;

        public TraceResult(Tracer tracer, bool isRoot, int level = 0)
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
            Action<Tracer,int, bool> action = (t, l, isR) =>
            {
                if (isR)
                {
                    return;
                }
                if (t.MethodInfo == null)
                {
                    return;
                }
                string result = "";

                for (int i = 0; i < l; i++)
                {
                    result += " ";
                }
                result += t.MethodInfo.ToString();
                Console.WriteLine(result);
                l++;
            };

            _tracer.Traverse(_tracer,0,true,action);
        }

        public void ToXml()
        {
            
        }

        public void ToYaml()
        {
            
        }



    }
}
