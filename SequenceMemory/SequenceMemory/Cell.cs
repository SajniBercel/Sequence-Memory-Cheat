using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceMemory
{
    public  class Cell
    {
        public Point point { get; set; }
        public bool Avaible { get; set; }
        public Color LastColor { get; set; }
        public Cell(Point _point, bool avaible)
        {
            point = _point;
            Avaible = avaible;
        }
    }
}
