using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class NodeLink
    {
        public Node Parent, Child;

        public byte HPRNG, MPRNG, Prob;

        public Quality Quality;

        public double ResetTolerance;

        public NodeLink(Node parent, Node child, byte hprng, byte mprng, byte prob)
        {
            Parent = parent;
            Child = child;
            HPRNG = hprng;
            MPRNG = mprng;
            Prob = prob;
            Quality = Objects.Quality.Unknown;
        }

        public override string ToString()
        {
            string result = String.Format("Lv{0} ({1} / {2})", Child.Level, Child.HP, Child.MP);

            if (Child.MinPath != null && Child.MaxPath != null)
            {
                result += String.Format(" {0:0.00}% chance [{1:0.00} - {2:0.00} resets]", (Prob == 255 ? 256 : Prob) * 100 / 256.0, Child.SmartResets, Child.ProbableResets);

                /*if (Child.SimulatedResets > 0)
                {
                    result += String.Format(" {0:0.00}% chance [{1:0.00} - ({2:0.00}) - {3:0.00} resets]", (Prob == 255 ? 256 : Prob) * 100 / 256.0, Child.MinPath.Resets, Child.SimulatedResets, Child.MaxPath.Resets);
                }
                else
                {
                    result += String.Format(" {0:0.00}% chance [{1:0.00} - {2:0.00} resets]", (Prob == 255 ? 256 : Prob) * 100 / 256.0, Child.MinPath.Resets, Child.MaxPath.Resets);
                }*/
            }

            return result;
        }
    }

    public enum Quality
    {
        Good,
        Questionable,
        Bad,
        Unknown
    }
}
