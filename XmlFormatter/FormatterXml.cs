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

        public void Format(TreeNode<T> _tree,ITracer tracer, int level, bool isRoot, string pathToSave)
        {
            XDocument document = new XDocument();
            XElement traceResult = new XElement("thread");
            traceResult.Add(new XAttribute("id", tracer
                                                .GetTraceResult()
                                                .ThreadId));
            traceResult.Add(new XAttribute("time", tracer
                                                .GetTraceResult()
                                                .ThreadTime));
            traceResult.Add(GetXml(_tree, 0, true));
            document.Add(traceResult);
            document.Save(pathToSave);
          
        }

        private XElement GetXml(TreeNode<T> _tree, int level, bool isRoot)
        {
            XElement element = new XElement("root"); 
            if (!isRoot)
            {
                if (_tree.Data == null)
                {
                    return null;
                }
                PropertyInfo[] props = _tree.Data.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                element = new XElement("method");
                foreach (var prop in props)
                {
                    element.Add(new XAttribute(prop.Name, prop.GetValue(_tree.Data)));
                }
            }
            foreach (TreeNode<T> kid in _tree.Children)
            {
                element.Add(GetXml(kid, level, false));
            }
            return element;
        }
    }
}
