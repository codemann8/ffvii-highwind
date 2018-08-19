using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class VincentCharacter : Character
    {
        public static new string NAME = "Vincent";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 178;
        public static new ushort START_MP = 18;

        public static new ushort HP_MAX = 8779;
        public static new ushort MP_MAX = 907;
        public static new ushort MP_MAX_ABS = 915;

        public static new byte SAFETY_LEVEL = 39;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 18, 160, 63, 12),
                                                                                    new HPMPGradientBase(12, 21, 41, -80, 80, -6),
                                                                                    new HPMPGradientBase(22, 31, 67, -600, 90, -26),
                                                                                    new HPMPGradientBase(32, 41, 86, -1160, 96, -44),
                                                                                    new HPMPGradientBase(42, 51, 110, -2120, 100, -60),
                                                                                    new HPMPGradientBase(52, 61, 123, -2800, 105, -86),
                                                                                    new HPMPGradientBase(62, 81, 120, -2640, 97, 38),
                                                                                    new HPMPGradientBase(82, 99, 92, -400, 84, 74)};

        public VincentCharacter()
        {

        }

        public override string Name { get { return VincentCharacter.NAME; } }

        public override byte StartLevel { get { return VincentCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return VincentCharacter.START_HP; } }
        public override ushort StartMP { get { return VincentCharacter.START_MP; } }

        public override ushort HPMax { get { return VincentCharacter.HP_MAX; } }
        public override ushort MPMax { get { return VincentCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return VincentCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return VincentCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return VincentCharacter.HPMP_TABLE; } }
    }
}
