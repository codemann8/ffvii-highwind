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

        public MainForm()
        {
            InitializeComponent();

            c = new Controller();
        }

        private void grpCharacters_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            if (radio.Checked)
            {
                switch (radio.Name)
                {
                    case "rdoCloud":
                        c.InitCharacter(new CloudCharacter());
                        break;
                    case "rdoBarret":
                        c.InitCharacter(new BarretCharacter());
                        break;
                    case "rdoTifa":
                        c.InitCharacter(new TifaCharacter());
                        break;
                    case "rdoAeris":
                        c.InitCharacter(new AerisCharacter());
                        break;
                    case "rdoRed":
                        c.InitCharacter(new RedCharacter());
                        break;
                    case "rdoYuffie":
                        c.InitCharacter(new YuffieCharacter());
                        break;
                    case "rdoCait":
                        c.InitCharacter(new CaitCharacter());
                        break;
                    case "rdoVincent":
                        c.InitCharacter(new VincentCharacter());
                        break;
                    case "rdoCid":
                        c.InitCharacter(new CidCharacter());
                        break;
                }

                cboLevel.Items.Clear();
                for (byte l = c.ActiveTree.Character.StartLevel; l <= Controller.MAX_LEVEL; l++)
                {
                    cboLevel.Items.Add(l);
                }

                treePath.Enabled = false;
                Application.DoEvents();

                LoadTree();

                treePath.Enabled = true;
                btnClear.Enabled = true;
                btnSet.Enabled = true;
            }
        }

        private void LoadTree()
        {
            if (c.ActiveTree.LevelIndex[Controller.MAX_LEVEL - 1].Count == 0)
            {
                stsStatus.BackColor = Color.Coral;

                string fileName = String.Format(@"{0}\FFVIICache\{1}.hpmp", AppDomain.CurrentDomain.BaseDirectory, c.ActiveTree.Character.GetFilenamePrefix());

                if (File.Exists(fileName))
                {
                    lblStatus.Text = "Loading...";
                    Application.DoEvents();

                    int memory = c.GetMemory(c.ActiveTree.Character);

                    c.ActiveTree.ImportTree(fileName, memory);

                    c.ActiveTree.SelectedNode = c.ActiveTree.RootNode;

                    c.ActiveTree.TrimUpToSelectedNode();
                }
                else
                {
                    lblStatus.Text = "Building...";
                    Application.DoEvents();

                    c.BuildTree();

                    lblStatus.Text = "Trimming...";
                    Application.DoEvents();

                    c.ActiveTree.RemoveSubPars();

                    lblStatus.Text = "Saving...";
                    Application.DoEvents();

                    c.ActiveTree.ExportTree(fileName);

                    c.ActiveTree.SelectedNode = c.ActiveTree.RootNode;
                }

                lblStatus.Text = "Analyzing...";
                Application.DoEvents();

                c.ActiveTree.FindMinMaxPath();

                lblStatus.Text = "Idle";
                stsStatus.BackColor = SystemColors.Control;
            }

            RefreshTree();
        }

        private void RefreshTree()
        {
            treePath.Nodes.Clear();

            TreeNode node;

            if (c.ActiveTree.SelectedNode != null)
            {
                node = new TreeNode(c.ActiveTree.SelectedNode.ToString());
                node.Tag = c.ActiveTree.SelectedNode;
            }
            else
            {
                node = new TreeNode(c.ActiveTree.RootNode.ToString());
                node.Tag = c.ActiveTree.RootNode;
            }

            node.Nodes.Add(new TreeNode(""));

            treePath.Nodes.Add(node);

            treePath.Nodes[0].Expand();
        }

        private void treePath_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                foreach (NodeLink child in ((Node)(e.Node.Tag)).ChildNodes)
                {
                    if (child.Child != null)
                    {
                        TreeNode node = new TreeNode(child.Child.ToString());
                        node.Tag = child.Child;
                        node.Nodes.Add(new TreeNode(""));

                        e.Node.Nodes.Add(node);
                    }
                }

                e.Node.Nodes.RemoveAt(0);
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            stsStatus.BackColor = Color.Coral;
            lblStatus.Text = "Setting...";
            Application.DoEvents();

            byte level;
            ushort hp, mp;

            bool success = false;

            if (Byte.TryParse(cboLevel.Text, out level) && level > 0 && UInt16.TryParse(txtHP.Text, out hp) && hp > 0 && UInt16.TryParse(txtMP.Text, out mp) && mp > 0)
            {
                if (c.SetMemory(c.ActiveTree.Character, level * 10000000 + hp * 1000 + mp))
                {
                    success = true;

                    cboLevel.ResetText();
                    txtHP.Text = "";
                    txtMP.Text = "";
                }
                else
                {
                    MessageBox.Show("The values entered are NOT safe values");
                }
            }
            else if (treePath.SelectedNode != null && treePath.SelectedNode.Tag != null)
            {
                success = true;

                c.ActiveTree.SelectedNode = (Node)treePath.SelectedNode.Tag;

                c.SetMemory(c.ActiveTree.Character, c.ActiveTree.SelectedNode.Level * 10000000 + c.ActiveTree.SelectedNode.HP * 1000 + c.ActiveTree.SelectedNode.MP);
            }

            if (success)
            {
                c.ActiveTree.TrimUpToSelectedNode();

                lblStatus.Text = "Analyzing...";
                Application.DoEvents();

                c.ActiveTree.FindMinMaxPath();

                RefreshTree();
            }

            lblStatus.Text = "Idle";
            stsStatus.BackColor = SystemColors.Control;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            stsStatus.BackColor = Color.Coral;
            lblStatus.Text = "Resetting...";
            Application.DoEvents();

            c.SetMemory(c.ActiveTree.Character, 0);

            c.ActiveTree = new CharacterTree(c.ActiveTree.Character);

            LoadTree();

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

            Node current = c.ActiveTree.RootNode;

            string output = String.Format("Level {0} ({1}/{2})", current.Level, current.HP, current.MP);

            bool shouldPrintLevel = false;

            while (current.Level < Controller.MAX_LEVEL)
            {
                bool valueIsSafe = false;

                int countResetsLevel = 0, rngIdx = 1;

                do
                {
                    if (current.Level == c.ActiveTree.Character.SafetyLevel)
                    {
                        shouldPrintLevel = true;
                    }

                    //Calculating next level's possible values
                    HPMPGradientBase table = c.ActiveTree.Character.GetTable((byte)(current.Level + 1));

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
                    NodeLink chosenPath = null;

                    foreach (NodeLink child in current.ChildNodes)
                    {
                        if (child.Child.HP == hps[rngIdx / 8] && child.Child.MP == mps[rngIdx % 8])
                        {
                            valueIsSafe = true;
                            chosenPath = child;
                            break;
                        }
                    }

                    //If it is safe, would it be advantageous to roll the dice for another value?
                    if (valueIsSafe)
                    {
                        //MAKE MODS BELOW
                        //Calculate a heuristic value for the node in question
                        //double value = (current.MaxPath.Resets - tempCurrent.MaxPath.Resets) + (current.MinPath.Resets - tempCurrent.MinPath.Resets);
                        double value = chosenPath.Child.MinPath.Resets;

                        //Prob is the number of times out of 256 that this will hit, (256 / x) - 1 represents the probable number of resets needed to hit this
                        //value -= ((256.0 / tempCurrent.FindParent(current).Prob) - 1);

                        //Loop thru all possible safe values and compare heuristic values, tally up the probability of a better value hitting
                        int prob = 0, countBetter = 0;
                        double magicCalc = 1, amountGain = 0; //magicCalc is an attempt to combine the probability and amount of gained benefit from switching
                        foreach (NodeLink child in current.ChildNodes)
                        {
                            double diffMax = current.MaxPath.Resets - child.Child.MaxPath.Resets, diffMin = current.MinPath.Resets - child.Child.MinPath.Resets;
                            if (child.Child.MinPath.Resets < value)
                            {
                                countBetter++;
                                prob += child.Prob;
                                amountGain += (value - child.Child.MinPath.Resets);
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
                        current = chosenPath.Child;
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

            Node current = c.ActiveTree.RootNode;

            string output = String.Format("Level {0} ({1}/{2})", current.Level, current.HP, current.MP);

            bool shouldPrintLevel = false;

            while (current.Level < Controller.MAX_LEVEL)
            {
                bool valueIsSafe = false;

                int countResetsLevel = 0, rngIdx = 1;

                do
                {
                    HPMPGradientBase table = c.ActiveTree.Character.GetTable((byte)(current.Level + 1));

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

                    if (current.Level == c.ActiveTree.Character.SafetyLevel)
                    {
                        shouldPrintLevel = true;
                    }

                    foreach (NodeLink child in current.ChildNodes)
                    {
                        if (child.Child.HP == hps[rngIdx / 8] && child.Child.MP == mps[rngIdx % 8])
                        {
                            valueIsSafe = true;
                            current = child.Child;
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
