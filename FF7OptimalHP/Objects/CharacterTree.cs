using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FF7OptimalHP.Objects
{
    public class CharacterTree
    {
        public Character Character;

        public Node RootNode, SelectedNode;

        public SortedDictionary<int, Node>[] LevelIndex;

        public CharacterTree(Character character)
        {
            Character = character;

            LevelIndex = new SortedDictionary<int, Node>[Controller.MAX_LEVEL];
            for (int l = 0; l < LevelIndex.Length; l++)
            {
                LevelIndex[l] = new SortedDictionary<int, Node>();
            }

            RootNode = new Node();

            RootNode.Level = Character.StartLevel;
            RootNode.HP = Character.StartHP;
            RootNode.MP = Character.StartMP;
            RootNode.ChildNodes = new List<Tuple<Node, byte, byte, byte>>();

            LevelIndex[RootNode.Level - 1].Add(RootNode.HP * 1000 + RootNode.MP, RootNode);
        }

        public void AddLevelDepthFirst(Node parent)
        {
            if (parent.Level >= Controller.MAX_LEVEL)
            {
                return;
            }

            byte level = (byte)(parent.Level + 1);

            HPMPGradientBase table = Character.GetTable(level);

            short hpDiffBase = (short)((short)(100 * (table.HP_BASE + (level - 1) * table.HP_GRADIENT) / parent.HP) - 100),
                mpDiffBase = (short)((short)(100 * (table.MP_BASE + (short)((level - 1) * table.MP_GRADIENT / 10)) / parent.MP) - 100);

            ushort[] hps = new ushort[Controller.RNG_LIST.GetLength(0)],
                mps = new ushort[Controller.RNG_LIST.GetLength(1)];

            for (int h = 0; h < hps.Length; h++)
            {
                hps[h] = (ushort)Math.Min(parent.HP + (ushort)(table.HP_GRADIENT * Controller.HP_GAIN[Math.Min(Math.Max((int)((h + 1) + hpDiffBase), 0), 11)] / 100), 9999);
            }

            for (int m = 0; m < mps.Length; m++)
            {
                mps[m] = (ushort)Math.Min(parent.MP + (ushort)((((ushort)(level * table.MP_GRADIENT / 10) - (ushort)((level - 1) * table.MP_GRADIENT / 10)) * Controller.MP_GAIN[Math.Min(Math.Max((int)((m + 1) + mpDiffBase), 0), 11)]) / 100), 999);
            }

            byte[,] overallProb = new byte[hps.Length, mps.Length];

            Dictionary<int, ushort> probs = new Dictionary<int, ushort>();

            for (int h = 0; h < Controller.RNG_LIST.GetLength(0); h++)
            {
                for (int m = 0; m < Controller.RNG_LIST.GetLength(1); m++)
                {
                    if (!probs.ContainsKey(hps[h] * 1000 + mps[m]))
                    {
                        probs.Add(hps[h] * 1000 + mps[m], 0);
                    }
                }
            }

            for (int h = 0; h < Controller.RNG_LIST.GetLength(0); h++)
            {
                for (int m = 0; m < Controller.RNG_LIST.GetLength(1); m++)
                {
                    probs[hps[h] * 1000 + mps[m]] += Controller.RNG_LIST[h, m];
                }
            }

            for (int h = 0; h < Controller.RNG_LIST.GetLength(0); h++)
            {
                for (int m = 0; m < Controller.RNG_LIST.GetLength(1); m++)
                {
                    if (Controller.RNG_LIST[h, m] > 0)
                    {
                        ushort prob = probs[hps[h] * 1000 + mps[m]];

                        if (prob == 256)
                        {
                            //if 100% chance, store value as 255, which is byte max
                            //since no paths have a 255/256 chance, this special case of 255 will later be converted to 256
                            prob--;
                        }

                        overallProb[h, m] = (byte)prob;
                    }
                }
            }

            for (int h = 0; h < hps.Length; h++)
            {
                for (int m = 0; m < mps.Length; m++)
                {
                    if (overallProb[h, m] > 0)
                    {
                        //search in local nodes
                        bool found = false;
                        foreach (Tuple<Node, byte, byte, byte> child in parent.ChildNodes)
                        {
                            if (child.Item1 != null)
                            {
                                if (child.Item1.HP == hps[h] && child.Item1.MP == mps[m])
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (!found)
                        {
                            //search in global nodes
                            found = false;
                            Node n;
                            if (LevelIndex[level - 1].TryGetValue(hps[h] * 1000 + mps[m], out n))
                            {
                                if (n != null)
                                {
                                    parent.ChildNodes.Add(Tuple.Create<Node, byte, byte, byte>(n, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]));
                                    n.ParentNodes.Add(Tuple.Create<Node, byte, byte, byte>(parent, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]));

                                    found = true;
                                }
                            }

                            if (!found)
                            {
                                Node node = new Node();
                                node.ParentNodes = new List<Tuple<Node, byte, byte, byte>>();

                                node.Level = level;
                                node.HP = hps[h];
                                node.MP = mps[m];
                                node.ChildNodes = new List<Tuple<Node, byte, byte, byte>>();

                                parent.ChildNodes.Add(Tuple.Create<Node, byte, byte, byte>(node, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]));
                                node.ParentNodes.Add(Tuple.Create<Node, byte, byte, byte>(parent, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]));

                                LevelIndex[node.Level - 1].Add(node.HP * 1000 + node.MP, node);

                                AddLevelDepthFirst(node);
                            }
                        }
                    }
                }
            }

            if (level == Controller.MAX_LEVEL)
            {
                for (int n = 0; n < parent.ChildNodes.Count; n++)
                {
                    if (parent.ChildNodes[n] != null)
                    {
                        if (parent.ChildNodes[n].Item1.HP > Character.HPMax)
                        {
                            //HP has new MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Item1.MP > Character.MPMaxAbs)
                        {
                            //MP has new MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Item1.HP >= Character.HPMax && parent.ChildNodes[n].Item1.MP > Character.MPMax)
                        {
                            //MP has new combined MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Item1.HP >= Character.HPMax && parent.ChildNodes[n].Item1.MP >= Character.MPMax)
                        {
                            //Found the usual MAX
                            level = level;
                        }
                    }
                }
            }
            else
            {

            }
        }

        public void RemoveSubPars()
        {
            List<int> removals = new List<int>();

            foreach (KeyValuePair<int, Node> entry in LevelIndex[Controller.MAX_LEVEL - 1])
            {
                if (entry.Value != null)
                {
                    if (!(entry.Value.HP >= Character.HPMax && entry.Value.MP >= Character.MPMax))
                    {
                        removals.Add(entry.Key);
                    }
                }
                else
                {
                    removals.Add(entry.Key);
                }
            }

            for (int n = 0; n < removals.Count; n++)
            {
                Node node;
                if (LevelIndex[Controller.MAX_LEVEL - 1].TryGetValue(removals[n], out node))
                {
                    if (node != null)
                    {
                        node.Delete();
                        node = null;
                    }

                    LevelIndex[Controller.MAX_LEVEL - 1].Remove(removals[n]);
                }
            }

            PruneTree(RootNode.Level);
        }

        public void PruneTree(byte upToLevel = 1)
        {
            for (int l = LevelIndex.Length - 2; l > upToLevel - 1; --l)
            {
                List<int> removals = new List<int>();

                foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                {
                    if (entry.Value != null)
                    {
                        foreach (Tuple<Node, byte, byte, byte> c in entry.Value.ChildNodes.ToList())
                        {
                            if (c.Item1 == null)
                            {
                                //shouldn't be possble
                                entry.Value.ChildNodes.Remove(c);
                            }
                        }

                        if (entry.Value.ChildNodes.Count == 0)
                        {
                            removals.Add(entry.Key);
                        }
                    }
                    else
                    {
                        removals.Add(entry.Key);
                    }
                }

                for (int n = 0; n < removals.Count; n++)
                {
                    Node node;
                    if (LevelIndex[l].TryGetValue(removals[n], out node))
                    {
                        if (node != null)
                        {
                            node.Delete();
                            node = null;
                        }

                        LevelIndex[l].Remove(removals[n]);
                    }
                }
            }

            for (int l = Math.Max(Character.StartLevel, upToLevel); l < LevelIndex.Length; l++)
            {
                List<int> removals = new List<int>();

                foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                {
                    if (entry.Value != null)
                    {
                        foreach (Tuple<Node, byte, byte, byte> p in entry.Value.ParentNodes.ToList())
                        {
                            if (p.Item1 == null)
                            {
                                //shouldn't happen, but ok
                                entry.Value.ParentNodes.Remove(p);
                            }
                        }

                        if (entry.Value.ParentNodes.Count == 0)
                        {
                            removals.Add(entry.Key);
                        }
                    }
                    else
                    {
                        removals.Add(entry.Key);
                    }
                }

                for (int n = 0; n < removals.Count; n++)
                {
                    Node node;
                    if (LevelIndex[l].TryGetValue(removals[n], out node))
                    {
                        if (node != null)
                        {
                            node.Delete();
                            node = null;
                        }

                        LevelIndex[l].Remove(removals[n]);
                    }
                }
            }
        }

        public void FindMinMaxPath()
        {
            if (RootNode.MinPath == null)
            {
                foreach (KeyValuePair<int, Node> entry in LevelIndex[Controller.MAX_LEVEL - 1])
                {
                    entry.Value.MinPath = new Path();
                    entry.Value.MaxPath = new Path();

                    entry.Value.MinPath.PathList.AddFirst(entry.Value);
                    entry.Value.MaxPath.PathList.AddFirst(entry.Value);
                }

                for (int l = Controller.MAX_LEVEL - 1; l > Character.StartLevel - 1; --l)
                {
                    foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                    {
                        foreach (Tuple<Node, byte, byte, byte> parent in entry.Value.ParentNodes)
                        {
                            byte prob = parent.Item1.GetProbSafe();

                            if (parent.Item1.MinPath == null || parent.Item1.MinPath.Resets > entry.Value.MinPath.Resets + ((256.0 / prob) - 1))
                            {
                                Path p = new Path();

                                foreach (Node n in entry.Value.MinPath.PathList)
                                {
                                    p.PathList.AddLast(n);
                                }
                                p.PathList.AddLast(parent.Item1);

                                if (prob < 255)
                                {
                                    p.Resets = entry.Value.MinPath.Resets + ((256.0 / prob) - 1);
                                }
                                else
                                {
                                    p.Resets = entry.Value.MinPath.Resets;
                                }

                                parent.Item1.MinPath = p;
                            }

                            if (parent.Item1.MaxPath == null || parent.Item1.MaxPath.Resets < entry.Value.MaxPath.Resets + ((256.0 / prob) - 1))
                            {
                                Path p = new Path();

                                foreach (Node n in entry.Value.MaxPath.PathList)
                                {
                                    p.PathList.AddLast(n);
                                }
                                p.PathList.AddLast(parent.Item1);

                                if (prob < 255)
                                {
                                    p.Resets = entry.Value.MaxPath.Resets + ((256.0 / prob) - 1);
                                }
                                else
                                {
                                    p.Resets = entry.Value.MaxPath.Resets;
                                }

                                parent.Item1.MaxPath = p;
                            }
                        }
                    }
                }

                for (int l = SelectedNode.Level - 1; l < Controller.MAX_LEVEL; l++)
                {
                    foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                    {
                        foreach (Tuple<Node, byte, byte, byte> child in entry.Value.ChildNodes)
                        {
                            if (child.Item1.MinPath != null)
                            {
                                child.Item1.MinPath.Chances = child.Item4;
                                child.Item1.MaxPath.Chances = child.Item4;
                            }

                            if (child.Item1.MaxPath == null || child.Item1.MaxPath.Resets < entry.Value.MaxPath.Resets + (256.0 / child.Item4) - 1)
                            {
                                child.Item1.MinPath.Chances = child.Item4;
                                child.Item1.MaxPath.Chances = child.Item4;
                            }
                        }
                    }
                }

                if (SelectedNode.MinPath == null)
                {
                    Path p = new Path();

                    foreach (Node n in SelectedNode.ChildNodes[0].Item1.MinPath.PathList)
                    {
                        p.PathList.AddLast(n);
                    }
                    p.PathList.AddLast(SelectedNode);
                    p.Resets = SelectedNode.ChildNodes[0].Item1.MinPath.Resets;

                    SelectedNode.MinPath = p;
                }

                if (SelectedNode.MaxPath == null)
                {
                    Path p = new Path();

                    foreach (Node n in SelectedNode.ChildNodes[0].Item1.MaxPath.PathList)
                    {
                        p.PathList.AddLast(n);
                    }
                    p.PathList.AddLast(SelectedNode);
                    p.Resets = SelectedNode.ChildNodes[0].Item1.MaxPath.Resets;

                    SelectedNode.MaxPath = p;
                }

                SelectedNode.MinPath.Chances = 256;
                SelectedNode.MaxPath.Chances = 256;
            }

            /*string fileName = String.Format("{0}\\FF7\\{1}.txt", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Character.Name.ToLower().Replace(" ", string.Empty));
            if (!Directory.Exists(fileName.Substring(0, fileName.LastIndexOf(@"\"))))
            {
                Directory.CreateDirectory(fileName.Substring(0, fileName.LastIndexOf(@"\")));
            }
            else if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            Path path = LevelIndex[Character.StartLevel - 1].FirstOrDefault().Value.MinPath;

            var node = path.PathList.Last;

            string fileOut = String.Format("{0}- MAX HP:{1} MAX MP:{2} Minimum Probable Resets:{3}", Character.Name, Character.HPMax, Character.MPMax, path.Resets);

            double resets = path.Resets;

            while (node != null)
            {
                fileOut += String.Format("\nLevel {0}: ( {1} / {2} ) {3} resets", node.Value.Level, node.Value.HP, node.Value.MP, resets - node.Value.MinPath.Resets);
                resets = node.Value.MinPath.Resets; 
                node = node.Previous;
            }

            path = LevelIndex[Character.StartLevel - 1].FirstOrDefault().Value.MaxPath;

            node = path.PathList.Last;

            fileOut += String.Format("\n\nMaximum Probable Resets:{0}", path.Resets);

            resets = path.Resets;

            while (node != null)
            {
                fileOut += String.Format("\nLevel {0}: ( {1} / {2} ) {3} resets", node.Value.Level, node.Value.HP, node.Value.MP, resets - node.Value.MaxPath.Resets);
                resets = node.Value.MaxPath.Resets; 
                node = node.Previous;
            }

            File.WriteAllText(fileName, fileOut);*/
        }

        public void TrimUpToSelectedNode()
        {
            foreach (KeyValuePair<int, Node> entry in LevelIndex[SelectedNode.Level - 1])
            {
                if (entry.Value.HP != SelectedNode.HP || entry.Value.MP != SelectedNode.MP)
                {
                    entry.Value.Delete();
                }
            }

            PruneTree(SelectedNode.Level);
        }

        public void ExportTree(string fileName)
        {
            if (!Directory.Exists(fileName.Substring(0, fileName.LastIndexOf(@"\"))))
            {
                Directory.CreateDirectory(fileName.Substring(0, fileName.LastIndexOf(@"\")));
            }
            else if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            foreach (SortedDictionary<int, Node> level in LevelIndex)
            {
                using (var stream = new FileStream(fileName, FileMode.Append))
                {
                    List<byte> serializedOutput = new List<byte>();

                    foreach (KeyValuePair<int, Node> entry in level)
                    {
                        if (entry.Value != null)
                        {
                            serializedOutput.Add(entry.Value.Level);
                            serializedOutput.AddRange(BitConverter.GetBytes(entry.Value.HP));
                            serializedOutput.AddRange(BitConverter.GetBytes(entry.Value.MP));

                            if (entry.Value.ParentNodes != null && entry.Value.ParentNodes.Count > 0)
                            {
                                serializedOutput.Add(Controller.IO_LISTSTART);

                                foreach (Tuple<Node, byte, byte, byte> parent in entry.Value.ParentNodes)
                                {
                                    if (parent != null)
                                    {
                                        serializedOutput.Add(parent.Item2);
                                        serializedOutput.Add(parent.Item3);
                                        serializedOutput.Add(parent.Item4);
                                        serializedOutput.AddRange(BitConverter.GetBytes(parent.Item1.HP));
                                        serializedOutput.AddRange(BitConverter.GetBytes(parent.Item1.MP));
                                    }
                                }

                                serializedOutput.Add(Controller.IO_LISTEND);
                            }
                        }
                    }

                    stream.Write(serializedOutput.ToArray(), 0, serializedOutput.Count);
                }
            }
        }

        public void ImportTree(string fileName, int selectedNodeValue = 0)
        {
            byte[] serializedInput = File.ReadAllBytes(fileName);

            LevelIndex = new SortedDictionary<int, Node>[Controller.MAX_LEVEL];

            for (int l = 0; l < LevelIndex.Length; l++)
            {
                LevelIndex[l] = new SortedDictionary<int, Node>();
            }

            RootNode = null;

            bool skipAhead = selectedNodeValue > 0;

            for (int i = 0; i < serializedInput.Length; )
            {
                Node node = new Node();
                node.Level = serializedInput[i];
                node.HP = BitConverter.ToUInt16(serializedInput, i + 1);
                node.MP = BitConverter.ToUInt16(serializedInput, i + 3);

                if (skipAhead && selectedNodeValue / 10000000 < node.Level)
                {
                    skipAhead = false;
                }

                if (!skipAhead || selectedNodeValue == node.Level * 10000000 + node.HP * 1000 + node.MP)
                {
                    node.ChildNodes = new List<Tuple<Node, byte, byte, byte>>();

                    LevelIndex[node.Level - 1].Add(node.HP * 1000 + node.MP, node);

                    if (RootNode == null)
                    {
                        RootNode = node;
                    }
                }

                i += 5;

                if (i < serializedInput.Length)
                {
                    if (serializedInput[i] == Controller.IO_LISTSTART)
                    {
                        i++;
                        node.ParentNodes = new List<Tuple<Node, byte, byte, byte>>();

                        do
                        {
                            Node parentNode;

                            if (LevelIndex[node.Level - 2].TryGetValue(BitConverter.ToUInt16(serializedInput, i + 3) * 1000 + BitConverter.ToUInt16(serializedInput, i + 5), out parentNode))
                            {
                                Tuple<Node, byte, byte, byte> parent = Tuple.Create<Node, byte, byte, byte>(parentNode, serializedInput[i], serializedInput[i + 1], serializedInput[i + 2]);
                                node.ParentNodes.Add(parent);

                                bool found = false;

                                foreach (Tuple<Node, byte, byte, byte> child in parentNode.ChildNodes)
                                {
                                    if (child.Item1.HP == node.HP && child.Item1.MP == node.MP)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    parentNode.ChildNodes.Add(Tuple.Create<Node, byte, byte, byte>(node, serializedInput[i], serializedInput[i + 1], serializedInput[i + 2]));
                                }
                            }

                            i += 7;
                        }
                        while (serializedInput[i] != Controller.IO_LISTEND);

                        i++;
                    }
                }
            }
        }
    }
}
