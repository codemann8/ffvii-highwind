using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public abstract class Character
    {
        public static string NAME;

        public static byte START_LEVEL;
        public static ushort START_HP;
        public static ushort START_MP;

        public static ushort HP_MAX;
        public static ushort MP_MAX;
        public static ushort MP_MAX_ABS;

        public static byte SAFETY_LEVEL;

        public static HPMPGradientBase[] HPMP_TABLE;

        public abstract string Name { get; }

        public abstract byte StartLevel { get; }
        public abstract ushort StartHP { get; }
        public abstract ushort StartMP { get; }

        public abstract ushort HPMax { get; }
        public abstract ushort MPMax { get; }
        public abstract ushort MPMaxAbs { get; }

        public abstract byte SafetyLevel { get; }

        public abstract HPMPGradientBase[] HPMPTable { get; }

        public HPMPGradientBase GetTable(byte level)
        {
            foreach (HPMPGradientBase t in HPMPTable)
            {
                if (t.START_LEVEL <= level && t.END_LEVEL >= level)
                {
                    return t;
                }
            }

            return null;
        }

        public string GetFilenamePrefix()
        {
            return Name.ToLower().Replace(" ", string.Empty);
        }
    }
}
