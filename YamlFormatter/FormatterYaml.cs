using System;
using System.Text;
using FormatterContract;
using TracerLib.TracerImpl;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using TracerLib.TracerContract;

namespace YamlFormatter

{
    public class FormatterYaml<T>:IFormatter<T>
    {
  
        public void Format(TreeNode<T> tree,ITracer tracer, int level, bool isRoot)
        {
            StringBuilder result = new StringBuilder("root:");
            GetYaml(tree, 0, true, result);
            Save(result);
        }
        private void GetYaml(TreeNode<T> tree, int level, bool isRoot, StringBuilder result)
        {
            var countSpaces = new StringBuilder();
            if (!isRoot)
            {
                if (tree.Data == null)
                {
                    return;
                }

                for (int i = 0; i < level; i++)
                {
                    countSpaces.Append("   ");
                }

                PropertyInfo[] props = tree.Data.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                result.Append("\r\n");
                result.Append(countSpaces);
                result.Append(" - Method:");

                foreach (var prop in props)
                {
                    Type currentType = prop.PropertyType;
                    var itemValue = prop.GetValue(tree.Data, null);
                    result.AppendFormat("\r\n");
                    result.Append(countSpaces.ToString());
                    result.AppendFormat("       {0} : {1}", prop.Name,
                        itemValue);
                }
            }
            level++;

            foreach (var kid in tree.Children)
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
