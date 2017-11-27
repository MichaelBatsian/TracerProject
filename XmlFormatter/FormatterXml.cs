using System.IO;
using FormatterContract;
using System.Xml.Linq;
using System.Reflection;
using TracerLib.TracerImpl;
using System.Windows.Forms;
using TracerLib.TracerContract;

namespace XmlFormatter
{
    public class FormatterXml<T>:IFormatter<T>
    {

        public void Format(TreeNode<T> tree,ITracer tracer, int level, bool isRoot, string pathToSave)
        {
            var document = new XDocument();
            var traceResult = new XElement("thread");
            traceResult.Add(new XAttribute("id", tracer
                                                .GetTraceResult()
                                                .ThreadId));
            traceResult.Add(new XAttribute("time", tracer
                                                .GetTraceResult()
                                                .ThreadTime));
            traceResult.Add(GetXml(tree, level, true));
            document.Add(traceResult);
            document.Save(pathToSave);
          
        }

        private XElement GetXml(TreeNode<T> tree, int level, bool isRoot)
        {
            var element = new XElement("root"); 
            if (!isRoot)
            {
                if (tree.Data == null)
                {
                    return null;
                }
                var props = tree.Data.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                element = new XElement("method");
                foreach (var prop in props)
                {
                    element.Add(new XAttribute(prop.Name, prop.GetValue(tree.Data)));
                }
            }
            foreach (var kid in tree.Children)
            {
                element.Add(GetXml(kid, level, false));
            }
            return element;
        }
    }
}
