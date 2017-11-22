﻿using System;
using System.Text;
using FormatterContract;
using TracerLib.TracerImpl;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using TracerLib.TracerContract;

namespace JsonFormatter
{
    public class FormatterJson<T>:IFormatter<T>
    {

        public void Format(TreeNode<T> tree,ITracer tracer, int level, bool isRoot)
        {
            StringBuilder result = new StringBuilder("{\r\n \"root\": [\r\n  {");
            GetJson(tree, 0, true, result);
            result.Append("\r\n]\r\n}");
            Save(result);

            
        }

        private void GetJson(TreeNode<T> tree, int level, bool isRoot,StringBuilder result)
        {
            var countSpaces = new StringBuilder("   ");
            if (!isRoot)
            {
                if (tree.Data == null)
                {
                    return;
                }
                
                for (int i = 0; i < level; i++)
                {
                    countSpaces.Append(" ");
                }
                PropertyInfo[] props = tree.Data.GetType()
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
                    var itemValue = prop.GetValue(tree.Data, null);
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
            foreach (var kid in tree.Children)
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
