using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class CloudCharacter : Character
    {
        public static new string NAME = "Cloud";

        public static new byte START_LEVEL = 6;
        public static new ushort START_HP = 314;
        public static new ushort START_MP = 54;

        public static new ushort HP_MAX = 9511;
        public static new ushort MP_MAX = 902;
        public static new ushort MP_MAX_ABS = 905;

        public static new byte SAFETY_LEVEL = 66;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 19, 200, 64, 12),
                                                                                    new HPMPGradientBase(12, 21, 42, -40, 78, 0),
                                                                                    new HPMPGradientBase(22, 31, 72, -640, 90, -26),
                                                                                    new HPMPGradientBase(32, 41, 100, -1440, 101, -58),
                                                                                    new HPMPGradientBase(42, 51, 121, -2280, 112, -102),
                                                                                    new HPMPGradientBase(52, 61, 137, -3080, 112, -102),
                                                                                    new HPMPGradientBase(62, 81, 120, -2040, 96, -4),
                                                                                    new HPMPGradientBase(82, 99, 98, -200, 73, 180)};

        public CloudCharacter()
        {

        }

        public override string Name { get { return CloudCharacter.NAME; } }

        public override byte StartLevel { get { return CloudCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return CloudCharacter.START_HP; } }
        public override ushort StartMP { get { return CloudCharacter.START_MP; } }

        public override ushort HPMax { get { return CloudCharacter.HP_MAX; } }
        public override ushort MPMax { get { return CloudCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return CloudCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return CloudCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return CloudCharacter.HPMP_TABLE; } }
    }
}
