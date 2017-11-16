﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormatterContract;
using System.Xml.Linq;
using System.Xml.Serialization;
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
                document.Add(GetXml(tracer,0,true));
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