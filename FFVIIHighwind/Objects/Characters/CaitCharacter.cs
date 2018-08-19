using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class CaitCharacter : Character
    {
        public static new string NAME = "Cait Sith";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 224;
        public static new ushort START_MP = 18;

        public static new ushort HP_MAX = 9135;
        public static new ushort MP_MAX = 869;
        public static new ushort MP_MAX_ABS = 869;

        public static new byte SAFETY_LEVEL = 51;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 24, 200, 60, 12),
                                                                                    new HPMPGradientBase(12, 21, 51, -80, 75, -2),
                                                                                    new HPMPGradientBase(22, 31, 80, -640, 83, -20),
                                                                                    new HPMPGradientBase(32, 41, 111, -1560, 97, -60),
                                                                                    new HPMPGradientBase(42, 51, 141, -2760, 108, -104),
                                                                                    new HPMPGradientBase(52, 61, 138, -2600, 108, -104),
                                                                                    new HPMPGradientBase(62, 81, 99, -240, 94, -20),
                                                                                    new HPMPGradientBase(82, 99, 72, 2000, 70, 178)};

        public CaitCharacter()
        {

        }

        public override string Name { get { return CaitCharacter.NAME; } }

        public override byte StartLevel { get { return CaitCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return CaitCharacter.START_HP; } }
        public override ushort StartMP { get { return CaitCharacter.START_MP; } }

        public override ushort HPMax { get { return CaitCharacter.HP_MAX; } }
        public override ushort MPMax { get { return CaitCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return CaitCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return CaitCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return CaitCharacter.HPMP_TABLE; } }
    }
}
