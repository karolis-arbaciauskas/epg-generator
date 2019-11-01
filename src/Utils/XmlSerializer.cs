using System.IO;
using System.Text;
using System.Xml.Linq;

namespace awscsharp.Utils
{
    public class XmlSerializer
    {
        public static class SerializeXml
        {
            public static string Convert(XDocument xmlDocument)
            {
                var builder = new StringBuilder();
                using (TextWriter writer = new StringWriterUtf8(builder))
                {
                    xmlDocument.Save(writer, SaveOptions.DisableFormatting);
                    return builder.ToString();
                }
            }
        }

        private class StringWriterUtf8 : StringWriter
        {
            public StringWriterUtf8(StringBuilder sb) : base(sb)
            {
            }

            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}