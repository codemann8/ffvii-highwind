using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class NodeLink
    {
        public Node Parent, Child;

        public byte HPRNG, MPRNG, Prob;

        public NodeLink(Node parent, Node child, byte hprng, byte mprng, byte prob)
        {
            Parent = parent;
            Child = child;
            HPRNG = hprng;
            MPRNG = mprng;
            Prob = prob;
        }
    }
}
