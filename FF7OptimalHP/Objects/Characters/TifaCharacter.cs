using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF7OptimalHP.Objects
{
    public class TifaCharacter : Character
    {
        public static new string NAME = "Tifa";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 219;
        public static new ushort START_MP = 16;

        public static new ushort HP_MAX = 9037;
        public static new ushort MP_MAX = 848;
        public static new ushort MP_MAX_ABS = 850;

        public static new byte SAFETY_LEVEL = 55;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 19, 200, 60, 10),
                                                                                    new HPMPGradientBase(12, 21, 38, 0, 70, 0),
                                                                                    new HPMPGradientBase(22, 31, 64, -520, 84, -28),
                                                                                    new HPMPGradientBase(32, 41, 96, -1520, 94, -58),
                                                                                    new HPMPGradientBase(42, 51, 121, -2520, 104, -98),
                                                                                    new HPMPGradientBase(52, 61, 131, -3000, 104, -98),
                                                                                    new HPMPGradientBase(62, 81, 117, -2160, 92, -26),
                                                                                    new HPMPGradientBase(82, 99, 92, -80, 72, 136)};

        public TifaCharacter()
        {

        }

        public override string Name { get { return TifaCharacter.NAME; } }

        public override byte StartLevel { get { return TifaCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return TifaCharacter.START_HP; } }
        public override ushort StartMP { get { return TifaCharacter.START_MP; } }

        public override ushort HPMax { get { return TifaCharacter.HP_MAX; } }
        public override ushort MPMax { get { return TifaCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return TifaCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return TifaCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return TifaCharacter.HPMP_TABLE; } }
    }
}
