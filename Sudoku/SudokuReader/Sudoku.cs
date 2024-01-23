using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SudokuReader
{
    public class Sudoku
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("level")]
        public Level Level { get; set; }

        public List<List<int>> Solution { get; set; }
        public List<List<int>> Initial { get; set; }
        public List<List<int>> State { get; set; }
    }
}
