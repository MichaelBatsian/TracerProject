using FormatterContract;
using System.Xml.Linq;
using System.Reflection;
using TracerLib.TracerImpl;
using System.Windows.Forms;

namespace XmlFormatter
{
    public class FormatterXml:IFormatter
    {

        public void Format(Tracer tracer, int level, bool isRoot)
        {
           
            SaveFileDialog saveFileDial = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                FileName = "TracerXml"
            };

            if (saveFileDial.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            XDocument document = new XDocument();
            XElement traceResult = new XElement("thread");
            traceResult.Add(new XAttribute("id",tracer.GetTraceResult().ThreadId));
            traceResult.Add(new XAttribute("time", tracer.GetTraceResult().ThreadTime));
            traceResult.Add(GetXml(tracer, 0, true));
            document.Add(traceResult);
            document.Save(saveFileDial.FileName);
           
        }

        private XElement GetXml(Tracer tracer, int level, bool isRoot)
        {
            XElement element = new XElement("root"); 
            if (!isRoot)
            {
                if (tracer.MethodInfo == null)
                {
                    return null;
                }
                PropertyInfo[] props = tracer.MethodInfo.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                element = new XElement("method");
                foreach (var prop in props)
                {
                    element.Add(new XAttribute(prop.Name, prop.GetValue(tracer.MethodInfo)));
                }
            }
            foreach (Tracer kid in tracer.Children)
            {
                element.Add(GetXml(kid, level, false));
            }
            return element;
        }
    }
}
