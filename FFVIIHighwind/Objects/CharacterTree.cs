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

        public Node RootNode;

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
            RootNode.ChildNodes = new List<NodeLink>();

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
                            //since no paths can have a 255/256 chance, this special case of 255 will later be converted to 256
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
                        foreach (NodeLink child in parent.ChildNodes)
                        {
                            if (child.Child != null)
                            {
                                if (child.Child.HP == hps[h] && child.Child.MP == mps[m])
                                {
                                    //TODO: Possibly in the future add this HPRNG AND MPRNG (h + 1 & m + 1) to the NodeLink
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
                                    NodeLink link = new NodeLink(parent, n, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]);
                                    parent.ChildNodes.Add(link);
                                    n.ParentNodes.Add(link);

                                    found = true;
                                }
                            }

                            if (!found)
                            {
                                Node node = new Node();
                                node.Level = level;
                                node.HP = hps[h];
                                node.MP = mps[m];

                                node.ParentNodes = new List<NodeLink>();
                                node.ChildNodes = new List<NodeLink>();

                                NodeLink link = new NodeLink(parent, node, (byte)(h + 1), (byte)(m + 1), overallProb[h, m]);
                                parent.ChildNodes.Add(link);
                                node.ParentNodes.Add(link);

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
                        if (parent.ChildNodes[n].Child.HP > Character.HPMax)
                        {
                            //HP has new MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Child.MP > Character.MPMaxAbs)
                        {
                            //MP has new MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Child.HP >= Character.HPMax && parent.ChildNodes[n].Child.MP > Character.MPMax)
                        {
                            //MP has new combined MAX!!!
                            level = level;
                        }
                        else if (parent.ChildNodes[n].Child.HP >= Character.HPMax && parent.ChildNodes[n].Child.MP >= Character.MPMax)
                        {
                            //Found the usual MAX
                            level = level;
                        }
                    }
                }
            }
        }

        public void RemoveSubPars()
        {
            foreach (KeyValuePair<int, Node> entry in LevelIndex[Controller.MAX_LEVEL - 1].ToList())
            {
                if (entry.Value != null)
                {
                    if (!(entry.Value.HP >= Character.HPMax && entry.Value.MP >= Character.MPMax))
                    {
                        entry.Value.Delete();
                        LevelIndex[Controller.MAX_LEVEL - 1].Remove(entry.Key);
                    }
                }
            }

            PruneTree(RootNode.Level);
        }

        public void PruneTree(byte upToLevel = 1)
        {
            for (int l = LevelIndex.Length - 2; l > upToLevel - 1; --l)
            {
                foreach (KeyValuePair<int, Node> entry in LevelIndex[l].ToList())
                {
                    if (entry.Value != null)
                    {
                        foreach (NodeLink c in entry.Value.ChildNodes.ToList())
                        {
                            if (c.Child == null)
                            {
                                //shouldn't happen, but here just in case
                                entry.Value.ChildNodes.Remove(c);
                            }
                        }

                        if (entry.Value.ChildNodes.Count == 0)
                        {
                            LevelIndex[l].Remove(entry.Key);
                            entry.Value.Delete();
                        }
                    }
                    else
                    {
                        LevelIndex[l].Remove(entry.Key);
                    }
                }
            }

            for (int l = Math.Max(Character.StartLevel, upToLevel); l < LevelIndex.Length; l++)
            {
                foreach (KeyValuePair<int, Node> entry in LevelIndex[l].ToList())
                {
                    if (entry.Value != null)
                    {
                        foreach (NodeLink p in entry.Value.ParentNodes.ToList())
                        {
                            if (p.Parent == null)
                            {
                                //shouldn't happen, but here just in case
                                entry.Value.ParentNodes.Remove(p);
                            }
                        }

                        if (entry.Value.ParentNodes.Count == 0)
                        {
                            LevelIndex[l].Remove(entry.Key);
                            entry.Value.Delete();
                        }
                    }
                    else
                    {
                        LevelIndex[l].Remove(entry.Key);
                    }
                }
            }
        }

        public void TrimUpToSelectedNode()
        {
            if (RootNode != null)
            {
                foreach (KeyValuePair<int, Node> entry in LevelIndex[RootNode.Level - 1].ToList())
                {
                    if (entry.Value.HP != RootNode.HP || entry.Value.MP != RootNode.MP)
                    {
                        LevelIndex[RootNode.Level - 1].Remove(entry.Key);
                        entry.Value.Delete();
                    }
                }
            }

            PruneTree(RootNode.Level);
        }

        public Node FindNode(byte level, ushort hp, ushort mp)
        {
            Node ret = null;

            LevelIndex[level - 1].TryGetValue(hp * 1000 + mp, out ret);

            return ret;
        }

        public void FindMinMaxPath()
        {
            if (RootNode.MinPath == null)
            {
                foreach (KeyValuePair<int, Node> entry in LevelIndex[Controller.MAX_LEVEL - 1])
                {
                    entry.Value.MinPath = new Path();
                    entry.Value.MaxPath = new Path();
                }

                for (int l = Controller.MAX_LEVEL - 1; l > RootNode.Level - 1; --l)
                {
                    foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                    {
                        foreach (NodeLink parentLink in entry.Value.ParentNodes)
                        {
                            byte prob = parentLink.Parent.GetProbSafe();

                            if (parentLink.Parent.MinPath == null || parentLink.Parent.MinPath.Resets > entry.Value.MinPath.Resets + ((256.0 / prob) - 1))
                            {
                                Path p = new Path();

                                foreach (NodeLink link in entry.Value.MinPath.PathList)
                                {
                                    p.PathList.AddLast(link);
                                }
                                p.PathList.AddLast(parentLink);

                                if (prob < 255)
                                {
                                    p.Resets = entry.Value.MinPath.Resets + ((256.0 / prob) - 1);
                                }
                                else
                                {
                                    p.Resets = entry.Value.MinPath.Resets;
                                }

                                parentLink.Parent.MinPath = p;
                            }

                            if (parentLink.Parent.MaxPath == null || parentLink.Parent.MaxPath.Resets < entry.Value.MaxPath.Resets + ((256.0 / prob) - 1))
                            {
                                Path p = new Path();

                                foreach (NodeLink link in entry.Value.MaxPath.PathList)
                                {
                                    p.PathList.AddLast(link);
                                }
                                p.PathList.AddLast(parentLink);

                                if (prob < 255)
                                {
                                    p.Resets = entry.Value.MaxPath.Resets + ((256.0 / prob) - 1);
                                }
                                else
                                {
                                    p.Resets = entry.Value.MaxPath.Resets;
                                }

                                parentLink.Parent.MaxPath = p;
                            }
                        }
                    }
                }

                for (int l = RootNode.Level - 1; l < Controller.MAX_LEVEL; l++)
                {
                    foreach (KeyValuePair<int, Node> entry in LevelIndex[l])
                    {
                        foreach (NodeLink child in entry.Value.ChildNodes)
                        {
                            if (child.Child.MinPath != null)
                            {
                                child.Child.MinPath.Chances = child.Prob;
                                child.Child.MaxPath.Chances = child.Prob;
                            }

                            if (child.Child.MaxPath == null || child.Child.MaxPath.Resets < entry.Value.MaxPath.Resets + (256.0 / child.Prob) - 1)
                            {
                                child.Child.MinPath.Chances = child.Prob;
                                child.Child.MaxPath.Chances = child.Prob;
                            }
                        }
                    }
                }

                if (RootNode.MinPath == null)
                {
                    Path p = new Path();

                    foreach (NodeLink link in RootNode.ChildNodes[0].Child.MinPath.PathList)
                    {
                        p.PathList.AddLast(link);
                    }
                    p.PathList.AddLast(RootNode.ChildNodes[0]);
                    p.Resets = RootNode.ChildNodes[0].Child.MinPath.Resets;

                    RootNode.MinPath = p;
                }

                if (RootNode.MaxPath == null)
                {
                    Path p = new Path();

                    foreach (NodeLink link in RootNode.ChildNodes[0].Child.MaxPath.PathList)
                    {
                        p.PathList.AddLast(link);
                    }
                    p.PathList.AddLast(RootNode.ChildNodes[0]);
                    p.Resets = RootNode.ChildNodes[0].Child.MaxPath.Resets;

                    RootNode.MaxPath = p;
                }

                RootNode.MinPath.Chances = 256;
                RootNode.MaxPath.Chances = 256;
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

                                foreach (NodeLink parent in entry.Value.ParentNodes)
                                {
                                    if (parent != null)
                                    {
                                        serializedOutput.Add(parent.HPRNG);
                                        serializedOutput.Add(parent.MPRNG);
                                        serializedOutput.Add(parent.Prob);
                                        serializedOutput.AddRange(BitConverter.GetBytes(parent.Parent.HP));
                                        serializedOutput.AddRange(BitConverter.GetBytes(parent.Parent.MP));
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

        public void ImportTree(string fileName, int memory)
        {
            byte[] serializedInput = File.ReadAllBytes(fileName);

            LevelIndex = new SortedDictionary<int, Node>[Controller.MAX_LEVEL];

            for (int l = 0; l < LevelIndex.Length; l++)
            {
                LevelIndex[l] = new SortedDictionary<int, Node>();
            }

            RootNode = null;

            bool skipAhead = memory > 0;

            for (int i = 0; i < serializedInput.Length; )
            {
                Node node = new Node();
                node.Level = serializedInput[i];
                node.HP = BitConverter.ToUInt16(serializedInput, i + 1);
                node.MP = BitConverter.ToUInt16(serializedInput, i + 3);

                if (skipAhead && memory / 10000000 < node.Level)
                {
                    skipAhead = false;
                }

                if (!skipAhead || memory == node.Level * 10000000 + node.HP * 1000 + node.MP)
                {
                    node.ChildNodes = new List<NodeLink>();

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
                        node.ParentNodes = new List<NodeLink>();

                        do
                        {
                            Node parentNode;

                            if (LevelIndex[node.Level - 2].TryGetValue(BitConverter.ToUInt16(serializedInput, i + 3) * 1000 + BitConverter.ToUInt16(serializedInput, i + 5), out parentNode))
                            {
                                NodeLink link = new NodeLink(parentNode, node, serializedInput[i], serializedInput[i + 1], serializedInput[i + 2]);
                                node.ParentNodes.Add(link);

                                bool found = false;

                                foreach (NodeLink child in parentNode.ChildNodes)
                                {
                                    if (child.Child.HP == node.HP && child.Child.MP == node.MP)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    parentNode.ChildNodes.Add(link);
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
