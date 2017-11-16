using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormatterContract;
using TracerLib.TracerImpl;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace JsonFormatter
{
    public class FormatterJson:IFormatter
    {

        public void Format(Tracer tracer, int level, bool isRoot)
        {
            StringBuilder result = new StringBuilder("{\r\n \"root\": [\r\n  {");
            GetJson(tracer, 0, true, result);
            result.Append("\r\n]");
            Save(result);

            
        }

        private void GetJson(Tracer tracer, int level, bool isRoot,StringBuilder result)
        {
            var countSpaces = new StringBuilder("   ");
            if (!isRoot)
            {
                if (tracer.MethodInfo == null)
                {
                    return;
                }
                
                for (int i = 0; i < level; i++)
                {
                    countSpaces.Append(" ");
                }
                PropertyInfo[] props = tracer.MethodInfo.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                result.Append("\r\n");
                result.Append(countSpaces);
                result.Append(" \"Method\":[");
                result.AppendFormat("\r\n{0}", countSpaces);
                result.Append(" {");
                result.Append(countSpaces);
                foreach (var prop in props)
                {
                    Type currentType = prop.PropertyType;
                    var itemValue = prop.GetValue(tracer.MethodInfo, null);
                    bool prim = currentType.IsPrimitive;
                    result.AppendFormat("\r\n");
                    result.Append(countSpaces.ToString());
                    result.AppendFormat(prop.PropertyType.Name.Equals("Int32")
                        ||prop.PropertyType.Name.Equals("Double")
                        ||prop.PropertyType.Name.Equals("Single")
                        ||prop.PropertyType.Name.Equals("Decimal") ? "  \"{0}\": {1}" : "  \"{0}\": \"{1}\"", prop.Name,
                        itemValue);
                }
                
            }
        
            level++;
            foreach (Tracer kid in tracer.Children)
            {
                GetJson(kid, level, false,result);
            }
            result.AppendFormat("\r\n{0}",countSpaces);
            result.Append(" },");

            result.AppendFormat("\r\n{0} ]",countSpaces);
        }

        private void Save(StringBuilder obj)
        {
            SaveFileDialog saveFileDial = new SaveFileDialog
            {
                Filter = "Json Files (*.json)|*.json|All Files (*.*)|*.*",
                FileName = "Json"
            };

            if (saveFileDial.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            using (var sw = new StreamWriter(saveFileDial.FileName))
            {
                sw.Write(obj.ToString());
            }
        }
    }
}
