using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class AerisCharacter : Character
    {
        public static new string NAME = "Aeris";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 177;
        public static new ushort START_MP = 23;

        public static new ushort HP_MAX = 8816;
        public static new ushort MP_MAX = 994;
        public static new ushort MP_MAX_ABS = 994;

        public static new byte SAFETY_LEVEL = 50;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 17, 160, 70, 16),
                                                                                    new HPMPGradientBase(12, 21, 36, 0, 84, 0),
                                                                                    new HPMPGradientBase(22, 31, 65, -560, 99, -30),
                                                                                    new HPMPGradientBase(32, 41, 93, -1400, 112, -68),
                                                                                    new HPMPGradientBase(42, 51, 114, -2240, 124, -116),
                                                                                    new HPMPGradientBase(52, 61, 126, -2880, 120, -96),
                                                                                    new HPMPGradientBase(62, 81, 113, -2080, 105, -6),
                                                                                    new HPMPGradientBase(82, 99, 93, -400, 82, 188)};

        public AerisCharacter()
        {

        }

        public override string Name { get { return AerisCharacter.NAME; } }

        public override byte StartLevel { get { return AerisCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return AerisCharacter.START_HP; } }
        public override ushort StartMP { get { return AerisCharacter.START_MP; } }

        public override ushort HPMax { get { return AerisCharacter.HP_MAX; } }
        public override ushort MPMax { get { return AerisCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return AerisCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return AerisCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return AerisCharacter.HPMP_TABLE; } }
    }
}
