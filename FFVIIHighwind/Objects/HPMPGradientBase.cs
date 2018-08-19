using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class HPMPGradientBase
    {
        public byte START_LEVEL;
        public byte END_LEVEL;

        public ushort HP_GRADIENT;
        public short HP_BASE;

        public ushort MP_GRADIENT;
        public short MP_BASE;

        public HPMPGradientBase(byte startLevel, byte endLevel, ushort hpGradient, short hpBase, ushort mpGradient, short mpBase)
        {
            START_LEVEL = startLevel;
            END_LEVEL = endLevel;
            HP_GRADIENT = hpGradient;
            HP_BASE = hpBase;
            MP_GRADIENT = mpGradient;
            MP_BASE = mpBase;
        }
    }
}
