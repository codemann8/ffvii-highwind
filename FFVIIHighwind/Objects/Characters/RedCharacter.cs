using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class RedCharacter : Character
    {
        public static new string NAME = "Red XIII";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 221;
        public static new ushort START_MP = 17;

        public static new ushort HP_MAX = 9556;
        public static new ushort MP_MAX = 863;
        public static new ushort MP_MAX_ABS = 866;

        public static new byte SAFETY_LEVEL = 59;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 21, 200, 58, 12),
                                                                                    new HPMPGradientBase(12, 21, 45, -40, 75, -6),
                                                                                    new HPMPGradientBase(22, 31, 75, -640, 86, -28),
                                                                                    new HPMPGradientBase(32, 41, 105, -1520, 97, -60),
                                                                                    new HPMPGradientBase(42, 51, 126, -2360, 108, -104),
                                                                                    new HPMPGradientBase(52, 61, 134, -2760, 112, -126),
                                                                                    new HPMPGradientBase(62, 81, 119, -1840, 94, -16),
                                                                                    new HPMPGradientBase(82, 99, 97, -80, 66, 210)};

        public RedCharacter()
        {

        }

        public override string Name { get { return RedCharacter.NAME; } }

        public override byte StartLevel { get { return RedCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return RedCharacter.START_HP; } }
        public override ushort StartMP { get { return RedCharacter.START_MP; } }

        public override ushort HPMax { get { return RedCharacter.HP_MAX; } }
        public override ushort MPMax { get { return RedCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return RedCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return RedCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return RedCharacter.HPMP_TABLE; } }
    }
}
