using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class Path
    {
        public LinkedList<NodeLink> PathList;

        public double Resets;

        public ushort Chances;

        public Path()
        {
            PathList = new LinkedList<NodeLink>();
        }
    }
}
