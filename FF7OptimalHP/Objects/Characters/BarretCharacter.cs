using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class BarretCharacter : Character
    {
        public static new string NAME = "Barret";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 222;
        public static new ushort START_MP = 15;

        public static new ushort HP_MAX = 9999;
        public static new ushort MP_MAX = 793;
        public static new ushort MP_MAX_ABS = 796;

        public static new byte SAFETY_LEVEL = 54;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 22, 200, 57, 10),
                                                                                    new HPMPGradientBase(12, 21, 45, 0, 67, 0),
                                                                                    new HPMPGradientBase(22, 31, 82, -760, 77, -20),
                                                                                    new HPMPGradientBase(32, 41, 118, -1840, 90, -60),
                                                                                    new HPMPGradientBase(42, 51, 143, -2840, 102, -108),
                                                                                    new HPMPGradientBase(52, 61, 143, -2840, 100, -96),
                                                                                    new HPMPGradientBase(62, 81, 115, -1160, 84, 0),
                                                                                    new HPMPGradientBase(82, 99, 95, 600, 63, 170)};

        public BarretCharacter()
        {

        }

        public override string Name { get { return BarretCharacter.NAME; } }

        public override byte StartLevel { get { return BarretCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return BarretCharacter.START_HP; } }
        public override ushort StartMP { get { return BarretCharacter.START_MP; } }

        public override ushort HPMax { get { return BarretCharacter.HP_MAX; } }
        public override ushort MPMax { get { return BarretCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return BarretCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return BarretCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return BarretCharacter.HPMP_TABLE; } }
    }
}
