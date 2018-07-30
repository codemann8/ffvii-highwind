using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FF7OptimalHP.Objects;

namespace FF7OptimalHP
{
    public partial class MainForm : Form
    {
        private Controller c;

        private Character chr;

        public MainForm()
        {
            InitializeComponent();
        }

        private void treePath_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                foreach (Tuple<Node, byte, byte, byte> child in ((Node)(e.Node.Tag)).ChildNodes)
                {
                    if (child.Item1 != null)
                    {
                        TreeNode node = new TreeNode(child.Item1.ToString());
                        node.Tag = child.Item1;
                        node.Nodes.Add(new TreeNode(""));

                        e.Node.Nodes.Add(node);
                    }
                }

                e.Node.Nodes.RemoveAt(0);
            }
        }

        private void RefreshTree()
        {
            treePath.Nodes.Clear();

            TreeNode node;

            if (c.SelectedNode != null)
            {
                node = new TreeNode(c.SelectedNode.ToString());
                node.Tag = c.SelectedNode;
            }
            else
            {
                node = new TreeNode(c.RootNode.ToString());
                node.Tag = c.RootNode;
            }

            node.Nodes.Add(new TreeNode(""));

            treePath.Nodes.Add(node);

            treePath.Nodes[0].Expand();
        }

        private void LoadDefaultTree()
        {
            stsStatus.BackColor = Color.Coral;

            string fileName = String.Format("{0}\\FFVIICache\\{1}.hpmp", AppDomain.CurrentDomain.BaseDirectory, c.Character.GetFilenamePrefix());

            if (File.Exists(fileName))
            {
                lblStatus.Text = "Loading...";
                Application.DoEvents();

                c.ImportTree(fileName);
            }
            else
            {
                lblStatus.Text = "Building...";
                Application.DoEvents();

                c.Run();

                lblStatus.Text = "Trimming...";
                Application.DoEvents();

                c.RemoveSubPars();

                lblStatus.Text = "Saving...";
                Application.DoEvents();

                c.ExportTree(fileName);
            }

            var property = new System.Configuration.SettingsProperty(Properties.Settings.Default.Properties["CharacterMemory"]);
            property.Name = c.Character.GetFilenamePrefix();
            try
            {
                Properties.Settings.Default.Properties.Add(property);
            }
            catch (ArgumentException ex) { }

            c.SelectedNode = c.RootNode;

            int value = (int)Properties.Settings.Default[c.Character.GetFilenamePrefix()];

            if (value > 0)
            {
                ushort mp = (ushort)(value % 1000);
                value /= 1000;
                ushort hp = (ushort)(value % 10000);
                value /= 10000;
                byte level = (byte)value;

                lblStatus.Text = "Setting...";
                Application.DoEvents();

                SetMemory(level, hp, mp);
            }
            else
            {
                RefreshTree();
            }

            lblStatus.Text = "Idle";
            stsStatus.BackColor = SystemColors.Control;
        }

        private void SetMemory(byte level, ushort hp, ushort mp, bool save = false)
        {
            if (!c.LevelIndex[level - 1].TryGetValue(hp * 1000 + mp, out c.SelectedNode))
            {
                if (save)
                {
                    MessageBox.Show("The values entered are NOT safe values");
                }
                Properties.Settings.Default[c.Character.GetFilenamePrefix()] = 0;
                Properties.Settings.Default.Save();
            }
            else
            {
                if (save)
                {
                    Properties.Settings.Default[c.Character.GetFilenamePrefix()] = level * 10000000 + hp * 1000 + mp;
                    Properties.Settings.Default.Save();
                }

                c.TrimUpToSelectedNode();
                c.FindMinMaxPath();

                cboLevel.ResetText();
                txtHP.Text = "";
                txtMP.Text = "";
            }

            RefreshTree();
        }

        private void grpCharacters_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            if (radio.Checked)
            {
                c = new Controller();

                switch (radio.Name)
                {
                    case "rdoCloud":
                        chr = new CloudCharacter();
                        break;
                    case "rdoBarret":
                        chr = new BarretCharacter();
                        break;
                    case "rdoTifa":
                        chr = new TifaCharacter();
                        break;
                    case "rdoAeris":
                        chr = new AerisCharacter();
                        break;
                    case "rdoRed":
                        chr = new RedCharacter();
                        break;
                    case "rdoYuffie":
                        chr = new YuffieCharacter();
                        break;
                    case "rdoCait":
                        chr = new CaitCharacter();
                        break;
                    case "rdoVincent":
                        chr = new VincentCharacter();
                        break;
                    case "rdoCid":
                        chr = new CidCharacter();
                        break;
                }

                c.InitCharacter(chr);

                cboLevel.Items.Clear();
                for (byte l = c.Character.StartLevel; l <= Controller.MAX_LEVEL; l++)
                {
                    cboLevel.Items.Add(l);
                }

                treePath.Enabled = false;

                Application.DoEvents();

                LoadDefaultTree();

                treePath.Enabled = true;
                btnClear.Enabled = true;
                btnSet.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            stsStatus.BackColor = Color.Coral;
            lblStatus.Text = "Resetting...";
            Application.DoEvents();

            SetMemory(1, 0, 0);

            c.SelectedNode = c.RootNode;

            LoadDefaultTree();

            lblStatus.Text = "Idle";
            stsStatus.BackColor = SystemColors.Control;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            stsStatus.BackColor = Color.Coral;
            lblStatus.Text = "Setting...";
            Application.DoEvents();

            byte level;
            ushort hp, mp;
            if (Byte.TryParse(cboLevel.Text, out level) && level > 0 && UInt16.TryParse(txtHP.Text, out hp) && hp > 0 && UInt16.TryParse(txtMP.Text, out mp) && mp > 0)
            {
                SetMemory(level, hp, mp, true);
            }
            else if (treePath.SelectedNode != null && treePath.SelectedNode.Tag != null)
            {
                c.SelectedNode = (Node)treePath.SelectedNode.Tag;
                c.TrimUpToSelectedNode();
                c.FindMinMaxPath();

                RefreshTree();
            }

            lblStatus.Text = "Idle";
            stsStatus.BackColor = SystemColors.Control;
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            int numberOfSimulations = 10000;
            int countTotalResets = 0, minResets = 9999, maxResets = 0;

            for (int i = 0; i < numberOfSimulations; i++)
            {
                int resets = SimulateMaxSafeOnly();
                //int resets = SimulateMaxBetterSafe();
                countTotalResets += resets;
                minResets = Math.Min(minResets, resets);
                maxResets = Math.Max(maxResets, resets);
            }

            MessageBox.Show(String.Format("{0} simulations, average {1:0.00} resets, range {2}-{3}", numberOfSimulations, countTotalResets / (double)numberOfSimulations, minResets, maxResets));
        }

        public int SimulateMaxBetterSafe()
        {
            int countResets = 0;

            Node current = c.RootNode;

            string output = String.Format("Level {0} ({1}/{2})", current.Level, current.HP, current.MP);

            bool shouldPrintLevel = false;

            while (current.Level < Controller.MAX_LEVEL)
            {
                bool valueIsSafe = false;

                int countResetsLevel = 0, rngIdx = 1;

                do
                {
                    if (current.Level == c.Character.SafetyLevel)
                    {
                        shouldPrintLevel = true;
                    }

                    //Calculating next level's possible values
                    HPMPGradientBase table = c.Character.GetTable((byte)(current.Level + 1));

                    short hpDiffBase = (short)((short)(100 * (table.HP_BASE + (current.Level) * table.HP_GRADIENT) / current.HP) - 100),
                        mpDiffBase = (short)((short)(100 * (table.MP_BASE + (short)((current.Level) * table.MP_GRADIENT / 10)) / current.MP) - 100);

                    ushort[] hps = new ushort[Controller.RNG_LIST.GetLength(0)],
                        mps = new ushort[Controller.RNG_LIST.GetLength(1)];

                    for (int h = 0; h < hps.Length; h++)
                    {
                        hps[h] = (ushort)Math.Min(current.HP + (ushort)(table.HP_GRADIENT * Controller.HP_GAIN[Math.Min(Math.Max((int)((h + 1) + hpDiffBase), 0), 11)] / 100), 9999);
                    }

                    for (int m = 0; m < mps.Length; m++)
                    {
                        mps[m] = (ushort)Math.Min(current.MP + (ushort)((((ushort)((current.Level + 1) * table.MP_GRADIENT / 10) - (ushort)(current.Level * table.MP_GRADIENT / 10)) * Controller.MP_GAIN[Math.Min(Math.Max((int)((m + 1) + mpDiffBase), 0), 11)]) / 100), 999);
                    }

                    //Randomly choosing the next level value
                    System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
                    byte[] rs = new byte[4];
                    provider.GetBytes(rs);
                    int r = rs[0];
                    rngIdx = 1;
                    while (r > 0)
                    {
                        r -= Controller.RNG_LIST[(rngIdx - 1) / 8, (rngIdx - 1) % 8];

                        if (r >= 0)
                        {
                            rngIdx++;
                        }
                    }
                    rngIdx--;

                    while (Controller.RNG_LIST[rngIdx / 8, rngIdx % 8] == 0)
                    {
                        rngIdx++;
                    }

                    //Check if chosen value is safe
                    Node tempCurrent = current;

                    foreach (Tuple<Node, byte, byte, byte> child in current.ChildNodes)
                    {
                        if (child.Item1.HP == hps[rngIdx / 8] && child.Item1.MP == mps[rngIdx % 8])
                        {
                            valueIsSafe = true;
                            tempCurrent = child.Item1;
                            break;
                        }
                    }

                    //If it is safe, would it be advantageous to roll the dice for another value?
                    if (valueIsSafe)
                    {
                        //MAKE MODS BELOW
                        //Calculate a heuristic value for the node in question
                        //double value = (current.MaxPath.Resets - tempCurrent.MaxPath.Resets) + (current.MinPath.Resets - tempCurrent.MinPath.Resets);
                        double value = tempCurrent.MinPath.Resets;

                        //Item4 is the number of times out of 256 that this will hit, (256 / x) - 1 represents the probable number of resets needed to hit this
                        //value -= ((256.0 / tempCurrent.FindParent(current).Item4) - 1);

                        //Loop thru all possible safe values and compare heuristic values, tally up the probability of a better value hitting
                        int prob = 0, countBetter = 0;
                        double magicCalc = 1, amountGain = 0; //magicCalc is an attempt to combine the probability and amount of gained benefit from switching
                        foreach (Tuple<Node, byte, byte, byte> child in current.ChildNodes)
                        {
                            double diffMax = current.MaxPath.Resets - child.Item1.MaxPath.Resets, diffMin = current.MinPath.Resets - child.Item1.MinPath.Resets;
                            if (child.Item1.MinPath.Resets < value)
                            {
                                countBetter++;
                                prob += child.Item4;
                                amountGain += (value - child.Item1.MinPath.Resets);
                                //magicCalc *= ((value - child.Item1.MinPath.Resets) * (child.Item4 / 256.0));
                                break;
                            }
                        }

                        magicCalc = (amountGain / countBetter) * (prob / 256.0);

                        //this is the condition that will force the routine to try for a new value at the same level
                        if (magicCalc > 3.0)
                        //MAKE MODS ABOVE
                        {
                            valueIsSafe = false;
                        }
                    }

                    if (!valueIsSafe)
                    {
                        countResetsLevel++;
                    }
                    else
                    {
                        current = tempCurrent;
                    }
                }
                while (!valueIsSafe);

                if (shouldPrintLevel)
                {
                    output += String.Format("\nLevel {0} ({1}/{2}) ({3}&{4}) - {5} resets", current.Level, current.HP, current.MP, rngIdx / 8 + 1, rngIdx % 8 + 1, countResetsLevel);
                }

                countResets += countResetsLevel;
            }

            output += String.Format("\nMax HP/MP achieved with {0} resets", countResets);

            //MessageBox.Show(output);

            return countResets;
        }

        public int SimulateMaxSafeOnly()
        {
            int countResets = 0;

            Node current = c.RootNode;

            string output = String.Format("Level {0} ({1}/{2})", current.Level, current.HP, current.MP);

            bool shouldPrintLevel = false;

            while (current.Level < Controller.MAX_LEVEL)
            {
                bool valueIsSafe = false;

                int countResetsLevel = 0, rngIdx = 1;

                do
                {
                    HPMPGradientBase table = c.Character.GetTable((byte)(current.Level + 1));

                    short hpDiffBase = (short)((short)(100 * (table.HP_BASE + (current.Level) * table.HP_GRADIENT) / current.HP) - 100),
                        mpDiffBase = (short)((short)(100 * (table.MP_BASE + (short)((current.Level) * table.MP_GRADIENT / 10)) / current.MP) - 100);

                    ushort[] hps = new ushort[Controller.RNG_LIST.GetLength(0)],
                        mps = new ushort[Controller.RNG_LIST.GetLength(1)];

                    for (int h = 0; h < hps.Length; h++)
                    {
                        hps[h] = (ushort)Math.Min(current.HP + (ushort)(table.HP_GRADIENT * Controller.HP_GAIN[Math.Min(Math.Max((int)((h + 1) + hpDiffBase), 0), 11)] / 100), 9999);
                    }

                    for (int m = 0; m < mps.Length; m++)
                    {
                        mps[m] = (ushort)Math.Min(current.MP + (ushort)((((ushort)((current.Level + 1) * table.MP_GRADIENT / 10) - (ushort)(current.Level * table.MP_GRADIENT / 10)) * Controller.MP_GAIN[Math.Min(Math.Max((int)((m + 1) + mpDiffBase), 0), 11)]) / 100), 999);
                    }

                    System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
                    byte[] rs = new byte[4];
                    provider.GetBytes(rs);
                    int r = rs[0];
                    rngIdx = 1;
                    while (r > 0)
                    {
                        r -= Controller.RNG_LIST[(rngIdx - 1) / 8, (rngIdx - 1) % 8];

                        if (r >= 0)
                        {
                            rngIdx++;
                        }
                    }
                    rngIdx--;

                    while (Controller.RNG_LIST[rngIdx / 8, rngIdx % 8] == 0)
                    {
                        rngIdx++;
                    }

                    if (current.Level == c.Character.SafetyLevel)
                    {
                        shouldPrintLevel = true;
                    }

                    foreach (Tuple<Node, byte, byte, byte> child in current.ChildNodes)
                    {
                        if (child.Item1.HP == hps[rngIdx / 8] && child.Item1.MP == mps[rngIdx % 8])
                        {
                            valueIsSafe = true;
                            current = child.Item1;
                            break;
                        }
                    }

                    if (!valueIsSafe)
                    {
                        countResetsLevel++;
                    }
                }
                while (!valueIsSafe);

                if (shouldPrintLevel)
                {
                    output += String.Format("\nLevel {0} ({1}/{2}) ({3}&{4}) - {5} resets", current.Level, current.HP, current.MP, rngIdx / 8 + 1, rngIdx % 8 + 1, countResetsLevel);
                }

                countResets += countResetsLevel;
            }

            output += String.Format("\nMax HP/MP achieved with {0} resets", countResets);

            //MessageBox.Show(output);

            return countResets;
        }
    }
}
