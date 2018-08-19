using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFVIIHighwind.Objects
{
    public class YuffieCharacter : Character
    {
        public static new string NAME = "Yuffie";

        public static new byte START_LEVEL = 1;
        public static new ushort START_HP = 100;
        public static new ushort START_MP = 5;

        public static new ushort HP_MAX = 8993;
        public static new ushort MP_MAX = 838;
        public static new ushort MP_MAX_ABS = 841;

        public static new byte SAFETY_LEVEL = 53;

        public static new HPMPGradientBase[] HPMP_TABLE = new HPMPGradientBase[8] { new HPMPGradientBase(2, 11, 18, 200, 58, 10),
                                                                                    new HPMPGradientBase(12, 21, 37, 0, 72, -2),
                                                                                    new HPMPGradientBase(22, 31, 64, -560, 80, -20),
                                                                                    new HPMPGradientBase(32, 41, 89, -1320, 93, -58),
                                                                                    new HPMPGradientBase(42, 51, 111, -2160, 106, -110),
                                                                                    new HPMPGradientBase(52, 61, 127, -2960, 110, -130),
                                                                                    new HPMPGradientBase(62, 81, 120, -2560, 85, 20),
                                                                                    new HPMPGradientBase(82, 99, 96, -520, 72, 126)};

        public YuffieCharacter()
        {

        }

        public override string Name { get { return YuffieCharacter.NAME; } }

        public override byte StartLevel { get { return YuffieCharacter.START_LEVEL; } }
        public override ushort StartHP { get { return YuffieCharacter.START_HP; } }
        public override ushort StartMP { get { return YuffieCharacter.START_MP; } }

        public override ushort HPMax { get { return YuffieCharacter.HP_MAX; } }
        public override ushort MPMax { get { return YuffieCharacter.MP_MAX; } }
        public override ushort MPMaxAbs { get { return YuffieCharacter.MP_MAX_ABS; } }

        public override byte SafetyLevel { get { return YuffieCharacter.SAFETY_LEVEL; } }

        public override HPMPGradientBase[] HPMPTable { get { return YuffieCharacter.HPMP_TABLE; } }
    }
}
