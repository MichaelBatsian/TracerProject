using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerImpl;

namespace FormatterContract
{
    public interface IFormatter
    {
        void Format(Tracer tracer, int level,bool isRoot);
    }
}
