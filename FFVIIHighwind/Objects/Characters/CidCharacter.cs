using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class CidCharacter : Character
    {
        public static new string NAME = "Cid";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 223;
        public static new ushort START_MP = 15;

        public static new ushort HP_MAX = 9284;
        public static new ushort MP_MAX = 821;
        public static new ushort MP_MAX_ABS = 822;

        public static new byte SAFETY_LEVEL = 45;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 23, 200, 54, 10),
                                                                                    new HPMPGradientBase(12, 21, 44, -40, 75, -12),
                                                                                    new HPMPGradientBase(22, 31, 73, -640, 83, -26),
                                                                                    new HPMPGradientBase(32, 41, 107, -1640, 87, -38),
                                                                                    new HPMPGradientBase(42, 51, 125, -2360, 94, -66),
                                                                                    new HPMPGradientBase(52, 61, 129, -2560, 104, -116),
                                                                                    new HPMPGradientBase(62, 81, 115, -1720, 89, -24),
                                                                                    new HPMPGradientBase(82, 99, 93, 0, 69, 140)};

        public CidCharacter()
        {

        }

        public override string Name { get { return CidCharacter.NAME; } }

        public override byte StartLevel { get { return CidCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return CidCharacter.START_HP; } }
        public override ushort StartMP { get { return CidCharacter.START_MP; } }

        public override ushort HPMax { get { return CidCharacter.HP_MAX; } }
        public override ushort MPMax { get { return CidCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return CidCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return CidCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return CidCharacter.HPMP_TABLE; } }
    }
}
