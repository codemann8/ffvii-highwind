using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FF7OptimalHP.Objects
{
    class Controller
    {
        public static byte[,] RNG_LIST = {  {2, 4, 5, 8, 1, 7, 5, 1},
                                            {2, 3, 6, 5, 2, 8, 2, 4},
                                            {6, 3, 2, 2, 4, 4, 2, 8},
                                            {9, 6, 2, 4, 1, 0, 6, 4},
                                            {3, 6, 2, 3, 6, 2, 5, 5},
                                            {3, 3, 5, 0, 7, 4, 2, 8},
                                            {4, 3, 4, 7, 3, 5, 4, 2},
                                            {3, 4, 6, 2, 8, 2, 7, 0}};

        public static byte[] HP_GAIN = { 40, 50, 50, 60, 70, 80, 90, 100, 110, 120, 130, 150 };
        public static byte[] MP_GAIN = { 20, 30, 30, 50, 70, 80, 90, 100, 110, 120, 140, 160 };

        public static byte MAX_LEVEL = 99;

        public static byte IO_LISTSTART = 0xFE;
        public static byte IO_LISTEND = 0xFF;

        public CharacterTree ActiveTree;

        private Dictionary<string, CharacterTree> CharacterTrees;

        public DateTime StartTime;

        public Controller()
        {
            CharacterTrees = new Dictionary<string, CharacterTree>();
        }

        public void InitCharacter(Character character)
        {
            if (!CharacterTrees.TryGetValue(character.GetFilenamePrefix(), out ActiveTree))
            {
                ActiveTree = new CharacterTree(character);
                CharacterTrees.Add(character.GetFilenamePrefix(), ActiveTree);
            }
        }

        public void Run()
        {
            StartTime = DateTime.Now;

            ActiveTree.AddLevelDepthFirst(ActiveTree.RootNode);
        }
    }
}
