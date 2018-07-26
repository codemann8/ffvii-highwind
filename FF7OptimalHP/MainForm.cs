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
                foreach (Node n in ((Node)(e.Node.Tag)).ChildNodes)
                {
                    if (n != null)
                    {
                        TreeNode node = new TreeNode(n.ToString());
                        node.Tag = n;
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

            c.FindMinMaxPath();

            RefreshTree();

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
    }
}
