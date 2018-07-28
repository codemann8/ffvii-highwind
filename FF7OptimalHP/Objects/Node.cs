using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class Node
    {
        public byte Level;
        public ushort HP;
        public ushort MP;

        /*public byte HPRNG { get { return ParentNodes[0].Item2; } }
        public byte MPRNG { get { return ParentNodes[0].Item3; } }
        //public System.Numerics.BigInteger Probability;
        public double ResetsLikely { get { return ParentNodes[0].Item4; } }

        public Node ParentNode { get { return ParentNodes[0].Item1; } }*/

        public List<Tuple<Node, byte, byte, byte>> ParentNodes;

        public List<Node> ChildNodes;

        public Path MinPath, MaxPath;

        //public string Name { get { if (ParentNodes[0] != null) return this.ToString(); else return ""; } }

        public Node()
        {

        }

        public Tuple<Node, byte, byte, byte> FindParent(Node node)
        {
            foreach (Tuple<Node, byte, byte, byte> parent in ParentNodes)
            {
                if (node.HP == parent.Item1.HP && node.MP == parent.Item1.MP)
                {
                    return parent;
                }
            }

            return null;
        }

        public override string ToString()
        {
            string result = String.Format("Lv{0} ({1} / {2})", Level, HP, MP);

            if (MinPath != null && MaxPath != null)
            {
                result += String.Format(" {0:0.00}% chance [{1:0.00} - {2:0.00} resets]", (MinPath.Chances == 255 ? 256 : MinPath.Chances) * 100.0 / 256.0, MinPath.Resets, MaxPath.Resets);
            }

            return result;
        }

        public byte GetProbSafe()
        {
            ushort prob = 0;

            foreach (Node n in ChildNodes)
            {
                prob += n.FindParent(this).Item4;
            }

            return (byte)(prob > 255 ? 255 : prob);
        }

        public void Delete()
        {
            //remove this child reference from all parents
            if (ParentNodes != null)
            {
                foreach (Tuple<Node, byte, byte, byte> t in ParentNodes)
                {
                    if (t.Item1 != null)
                    {
                        foreach (Node n in t.Item1.ChildNodes)
                        {
                            if (n.HP == HP && n.MP == MP)
                            {
                                t.Item1.ChildNodes.Remove(n);
                                break;
                            }
                        }
                    }
                }
            }

            //remove this parent reference from all children
            if (ChildNodes != null)
            {
                foreach (Node n in ChildNodes)
                {
                    if (n != null)
                    {
                        foreach (Tuple<Node, byte, byte, byte> t in n.ParentNodes)
                        {
                            if (t.Item1.HP == HP && t.Item1.MP == MP)
                            {
                                n.ParentNodes.Remove(t);
                                break;
                            }
                        }
                    }
                }
            }

            ParentNodes = new List<Tuple<Node, byte, byte, byte>>();
            ChildNodes = new List<Node>();
        }
    }
}
