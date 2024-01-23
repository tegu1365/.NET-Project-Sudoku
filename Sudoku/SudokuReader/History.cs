using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace SudokuReader
{
    public class History
    {
        public List<Sudoku> Puzzles { get; set; }

        public void SaveToXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(History));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(xmlWriter, this);
            }
        }

        public static History LoadFromXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(History));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (XmlReader xmlReader = XmlReader.Create(fileStream))
            {
                return (History)serializer.Deserialize(xmlReader);
            }
        }
    }
}
