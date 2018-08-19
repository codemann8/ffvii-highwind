using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class Node
    {
        public byte Level;
        public ushort HP;
        public ushort MP;

        public List<NodeLink> ParentNodes;

        public List<NodeLink> ChildNodes;

        public Path MinPath, MaxPath;

        public double SimulatedResets;

        //public string Name { get { if (ParentNodes[0] != null) return this.ToString(); else return ""; } }

        public Node()
        {

        }

        public NodeLink FindParent(Node node)
        {
            foreach (NodeLink parent in ParentNodes)
            {
                if (node.HP == parent.Parent.HP && node.MP == parent.Parent.MP)
                {
                    return parent;
                }
            }

            return null;
        }

        public NodeLink FindChild(Node node)
        {
            foreach (NodeLink child in ChildNodes)
            {
                if (node.HP == child.Child.HP && node.MP == child.Child.MP)
                {
                    return child;
                }
            }

            return null;
        }

        public override string ToString()
        {
            string result = String.Format("Lv{0} ({1} / {2})", Level, HP, MP);

            if (MinPath != null && MaxPath != null)
            {
                if (SimulatedResets > 0)
                {
                    result += String.Format(" {0:0.00}% chance [{1:0.00} - ({2:0.00}) - {3:0.00} resets]", (MinPath.Chances == 255 ? 256 : MinPath.Chances) * 100.0 / 256.0, MinPath.Resets, SimulatedResets, MaxPath.Resets);
                }
                else
                {
                    result += String.Format(" {0:0.00}% chance [{1:0.00} - {2:0.00} resets]", (MinPath.Chances == 255 ? 256 : MinPath.Chances) * 100.0 / 256.0, MinPath.Resets, MaxPath.Resets);
                }
            }

            return result;
        }

        public byte GetProbSafe()
        {
            ushort prob = 0;

            foreach (NodeLink child in ChildNodes)
            {
                prob += child.Prob;
            }

            return (byte)(prob > 255 ? 255 : prob);
        }

        public void Delete()
        {
            //remove this child reference from all parents
            if (ParentNodes != null)
            {
                foreach (NodeLink childLink in ParentNodes)
                {
                    if (childLink.Parent != null)
                    {
                        foreach (NodeLink parentLink in childLink.Parent.ChildNodes.ToList())
                        {
                            if (parentLink.Child.HP == HP && parentLink.Child.MP == MP)
                            {
                                childLink.Parent.ChildNodes.Remove(parentLink);
                                break;
                            }
                        }
                    }
                }
            }

            //remove this parent reference from all children
            if (ChildNodes != null)
            {
                foreach (NodeLink parentLink in ChildNodes)
                {
                    if (parentLink.Child != null)
                    {
                        foreach (NodeLink childLink in parentLink.Child.ParentNodes.ToList())
                        {
                            if (childLink.Parent.HP == HP && childLink.Parent.MP == MP)
                            {
                                parentLink.Child.ParentNodes.Remove(childLink);
                                break;
                            }
                        }
                    }
                }
            }

            ParentNodes = new List<NodeLink>();
            ChildNodes = new List<NodeLink>();
        }
    }
}
