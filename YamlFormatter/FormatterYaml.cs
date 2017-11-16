using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormatterContract;
using TracerLib.TracerImpl;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace YamlFormatter

{
    public class FormatterYaml:IFormatter
    {
  
        public void Format(Tracer tracer, int level, bool isRoot)
        {
            StringBuilder result = new StringBuilder("root:");
            GetYaml(tracer, 0, true, result);
            Save(result);
        }
        private void GetYaml(Tracer tracer, int level, bool isRoot, StringBuilder result)
        {
            var countSpaces = new StringBuilder();
            if (!isRoot)
            {
                if (tracer.MethodInfo == null)
                {
                    return;
                }

                for (int i = 0; i < level; i++)
                {
                    countSpaces.Append("   ");
                }

                PropertyInfo[] props = tracer.MethodInfo.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                result.Append("\r\n");
                result.Append(countSpaces);
                result.Append(" - Method:");

                foreach (var prop in props)
                {
                    Type currentType = prop.PropertyType;
                    var itemValue = prop.GetValue(tracer.MethodInfo, null);
                    result.AppendFormat("\r\n");
                    result.Append(countSpaces.ToString());
                    result.AppendFormat("       {0} : {1}", prop.Name,
                        itemValue);
                }
            }
            level++;

            foreach (Tracer kid in tracer.Children)
            {
                GetYaml(kid, level, false, result);
            }
        }

        private void Save(StringBuilder obj)
        {
            SaveFileDialog saveFileDial = new SaveFileDialog
            {
                Filter = "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*",
                FileName = "YamlFormat"
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
