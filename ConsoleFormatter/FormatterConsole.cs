using System;
using System.IO;
using System.Text;
using FormatterContract;
using TracerLib.TracerContract;
using TracerLib.TracerImpl;

namespace ConsoleFormatter
{
    public class FormatterConsole<T> : IFormatter<T>
    {
        public void Format(TreeNode<T> tree, ITracer tracer, int level, bool isRoot, string savePath)
        {
            StringBuilder result = new StringBuilder();
            Traverse(tree, level,true,result);
            result.Append(Environment.NewLine);
            result.AppendFormat(
                $"ThreadId {tracer.GetTraceResult().ThreadId} Executing time {tracer.GetTraceResult()}");
            if (savePath != null)
            {
                Save(result.ToString(),savePath);
                return;
            }
            Console.WriteLine(result.ToString());
        }

        public void Traverse(TreeNode<T> tn, int level, bool isRoot, StringBuilder result)
        {
            if (!isRoot)
            {
                if (tn.Data == null)
                {
                    return;
                }
                for (int i = 0; i < level; i++)
                {
                    result.Append(" ");
                }
                result.Append(tn.Data.ToString());
                level++;
            }
            foreach (var kid in tn.Children)
            {
                result.Append(Environment.NewLine);
                Traverse(kid, level, false,result);
            }
        }

        public void Save(string str,string path)
        {
            using (var stream = new StreamWriter(path, false))
            {
                stream.Write(str);
            }
        }
    }
   
}
