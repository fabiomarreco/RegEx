using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegEx
{
    class XmlExporter
    {

        public void Executa(Regex reg, MatchCollection result, string arquivoXML)
        {
            using (XmlTextWriter tw = new XmlTextWriter(arquivoXML, Encoding.Default))
            {
                tw.Formatting = Formatting.Indented;
                tw.Indentation = 2;
                tw.IndentChar = ' ';
                ConverteRegex(reg, result, tw);
            }
        }

        private void ConverteRegex(Regex reg, MatchCollection matches, XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("matches");
            foreach (Match match in matches)
            {
                writer.WriteStartElement("match");
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    Group grupo = match.Groups[i];
                    string nomeGrupo = reg.GroupNameFromNumber(i);

                    foreach (Capture capture in grupo.Captures)
                    {
                        writer.WriteStartElement(nomeGrupo);
                        writer.WriteString(capture.Value);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

    }
}
