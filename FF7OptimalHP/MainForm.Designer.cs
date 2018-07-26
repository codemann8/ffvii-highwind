namespace FF7OptimalHP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treePath = new System.Windows.Forms.TreeView();
            this.grpCharacters = new System.Windows.Forms.GroupBox();
            this.rdoCid = new System.Windows.Forms.RadioButton();
            this.rdoVincent = new System.Windows.Forms.RadioButton();
            this.rdoCait = new System.Windows.Forms.RadioButton();
            this.rdoYuffie = new System.Windows.Forms.RadioButton();
            this.rdoRed = new System.Windows.Forms.RadioButton();
            this.rdoAeris = new System.Windows.Forms.RadioButton();
            this.rdoTifa = new System.Windows.Forms.RadioButton();
            this.rdoBarret = new System.Windows.Forms.RadioButton();
            this.rdoCloud = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.cboLevel = new System.Windows.Forms.ComboBox();
            this.txtHP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMP = new System.Windows.Forms.TextBox();
            this.stsStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpCharacters.SuspendLayout();
            this.stsStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // treePath
            // 
            this.treePath.Location = new System.Drawing.Point(82, 41);
            this.treePath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treePath.Name = "treePath";
            this.treePath.Size = new System.Drawing.Size(436, 348);
            this.treePath.TabIndex = 9;
            this.treePath.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treePath_BeforeExpand);
            // 
            // grpCharacters
            // 
            this.grpCharacters.Controls.Add(this.rdoCid);
            this.grpCharacters.Controls.Add(this.rdoVincent);
            this.grpCharacters.Controls.Add(this.rdoCait);
            this.grpCharacters.Controls.Add(this.rdoYuffie);
            this.grpCharacters.Controls.Add(this.rdoRed);
            this.grpCharacters.Controls.Add(this.rdoAeris);
            this.grpCharacters.Controls.Add(this.rdoTifa);
            this.grpCharacters.Controls.Add(this.rdoBarret);
            this.grpCharacters.Controls.Add(this.rdoCloud);
            this.grpCharacters.Location = new System.Drawing.Point(6, 7);
            this.grpCharacters.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCharacters.Name = "grpCharacters";
            this.grpCharacters.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCharacters.Size = new System.Drawing.Size(74, 183);
            this.grpCharacters.TabIndex = 0;
            this.grpCharacters.TabStop = false;
            this.grpCharacters.Text = "Characters";
            // 
            // rdoCid
            // 
            this.rdoCid.AutoSize = true;
            this.rdoCid.Location = new System.Drawing.Point(4, 162);
            this.rdoCid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoCid.Name = "rdoCid";
            this.rdoCid.Size = new System.Drawing.Size(40, 17);
            this.rdoCid.TabIndex = 8;
            this.rdoCid.Text = "Cid";
            this.rdoCid.UseVisualStyleBackColor = true;
            this.rdoCid.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoVincent
            // 
            this.rdoVincent.AutoSize = true;
            this.rdoVincent.Location = new System.Drawing.Point(3, 144);
            this.rdoVincent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoVincent.Name = "rdoVincent";
            this.rdoVincent.Size = new System.Drawing.Size(61, 17);
            this.rdoVincent.TabIndex = 7;
            this.rdoVincent.Text = "Vincent";
            this.rdoVincent.UseVisualStyleBackColor = true;
            this.rdoVincent.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoCait
            // 
            this.rdoCait.AutoSize = true;
            this.rdoCait.Location = new System.Drawing.Point(3, 125);
            this.rdoCait.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoCait.Name = "rdoCait";
            this.rdoCait.Size = new System.Drawing.Size(64, 17);
            this.rdoCait.TabIndex = 6;
            this.rdoCait.Text = "Cait Sith";
            this.rdoCait.UseVisualStyleBackColor = true;
            this.rdoCait.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoYuffie
            // 
            this.rdoYuffie.AutoSize = true;
            this.rdoYuffie.Location = new System.Drawing.Point(3, 107);
            this.rdoYuffie.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoYuffie.Name = "rdoYuffie";
            this.rdoYuffie.Size = new System.Drawing.Size(52, 17);
            this.rdoYuffie.TabIndex = 5;
            this.rdoYuffie.Text = "Yuffie";
            this.rdoYuffie.UseVisualStyleBackColor = true;
            this.rdoYuffie.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoRed
            // 
            this.rdoRed.AutoSize = true;
            this.rdoRed.Location = new System.Drawing.Point(4, 89);
            this.rdoRed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoRed.Name = "rdoRed";
            this.rdoRed.Size = new System.Drawing.Size(64, 17);
            this.rdoRed.TabIndex = 4;
            this.rdoRed.Text = "Red XIII";
            this.rdoRed.UseVisualStyleBackColor = true;
            this.rdoRed.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoAeris
            // 
            this.rdoAeris.AutoSize = true;
            this.rdoAeris.Location = new System.Drawing.Point(3, 71);
            this.rdoAeris.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoAeris.Name = "rdoAeris";
            this.rdoAeris.Size = new System.Drawing.Size(48, 17);
            this.rdoAeris.TabIndex = 3;
            this.rdoAeris.Text = "Aeris";
            this.rdoAeris.UseVisualStyleBackColor = true;
            this.rdoAeris.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoTifa
            // 
            this.rdoTifa.AutoSize = true;
            this.rdoTifa.Location = new System.Drawing.Point(4, 53);
            this.rdoTifa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoTifa.Name = "rdoTifa";
            this.rdoTifa.Size = new System.Drawing.Size(43, 17);
            this.rdoTifa.TabIndex = 2;
            this.rdoTifa.Text = "Tifa";
            this.rdoTifa.UseVisualStyleBackColor = true;
            this.rdoTifa.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoBarret
            // 
            this.rdoBarret.AutoSize = true;
            this.rdoBarret.Location = new System.Drawing.Point(4, 34);
            this.rdoBarret.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoBarret.Name = "rdoBarret";
            this.rdoBarret.Size = new System.Drawing.Size(53, 17);
            this.rdoBarret.TabIndex = 1;
            this.rdoBarret.Text = "Barret";
            this.rdoBarret.UseVisualStyleBackColor = true;
            this.rdoBarret.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // rdoCloud
            // 
            this.rdoCloud.AutoSize = true;
            this.rdoCloud.Location = new System.Drawing.Point(4, 16);
            this.rdoCloud.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoCloud.Name = "rdoCloud";
            this.rdoCloud.Size = new System.Drawing.Size(52, 17);
            this.rdoCloud.TabIndex = 0;
            this.rdoCloud.Text = "Cloud";
            this.rdoCloud.UseVisualStyleBackColor = true;
            this.rdoCloud.CheckedChanged += new System.EventHandler(this.grpCharacters_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(82, 6);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(74, 32);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(443, 6);
            this.btnSet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(74, 31);
            this.btnSet.TabIndex = 8;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // cboLevel
            // 
            this.cboLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLevel.FormattingEnabled = true;
            this.cboLevel.Location = new System.Drawing.Point(202, 6);
            this.cboLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Size = new System.Drawing.Size(46, 28);
            this.cboLevel.TabIndex = 3;
            // 
            // txtHP
            // 
            this.txtHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHP.Location = new System.Drawing.Point(284, 6);
            this.txtHP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHP.Name = "txtHP";
            this.txtHP.Size = new System.Drawing.Size(52, 26);
            this.txtHP.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(164, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "HP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "MP:";
            // 
            // txtMP
            // 
            this.txtMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMP.Location = new System.Drawing.Point(372, 6);
            this.txtMP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMP.Name = "txtMP";
            this.txtMP.Size = new System.Drawing.Size(52, 26);
            this.txtMP.TabIndex = 7;
            // 
            // stsStatus
            // 
            this.stsStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.stsStatus.Location = new System.Drawing.Point(0, 398);
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.Size = new System.Drawing.Size(522, 22);
            this.stsStatus.TabIndex = 10;
            this.stsStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeftAutoMirrorImage = true;
            this.lblStatus.Size = new System.Drawing.Size(476, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 420);
            this.Controls.Add(this.stsStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHP);
            this.Controls.Add(this.cboLevel);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grpCharacters);
            this.Controls.Add(this.treePath);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "FFVII Optimal Max HP/MP Helper";
            this.grpCharacters.ResumeLayout(false);
            this.grpCharacters.PerformLayout();
            this.stsStatus.ResumeLayout(false);
            this.stsStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treePath;
        private System.Windows.Forms.GroupBox grpCharacters;
        private System.Windows.Forms.RadioButton rdoCid;
        private System.Windows.Forms.RadioButton rdoVincent;
        private System.Windows.Forms.RadioButton rdoCait;
        private System.Windows.Forms.RadioButton rdoYuffie;
        private System.Windows.Forms.RadioButton rdoRed;
        private System.Windows.Forms.RadioButton rdoAeris;
        private System.Windows.Forms.RadioButton rdoTifa;
        private System.Windows.Forms.RadioButton rdoBarret;
        private System.Windows.Forms.RadioButton rdoCloud;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.ComboBox cboLevel;
        private System.Windows.Forms.TextBox txtHP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMP;
        private System.Windows.Forms.StatusStrip stsStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}

