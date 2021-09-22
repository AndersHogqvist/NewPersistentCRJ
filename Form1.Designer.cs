
namespace NewPersistentCRJ
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.saveBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.loadGroup = new System.Windows.Forms.GroupBox();
            this.loadBtn = new System.Windows.Forms.Button();
            this.saveGroup = new System.Windows.Forms.GroupBox();
            this.autoSaveBtn = new System.Windows.Forms.Button();
            this.connectBtn = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numOfCtrlLabel = new System.Windows.Forms.Label();
            this.randomBtn = new System.Windows.Forms.Button();
            this.randomLvarsScroll = new System.Windows.Forms.TrackBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.loadGroup.SuspendLayout();
            this.saveGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomLvarsScroll)).BeginInit();
            this.SuspendLayout();
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 22);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(95, 32);
            this.saveBtn.TabIndex = 0;
            this.saveBtn.Text = "Save to file";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(24, 176);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(95, 32);
            this.exitBtn.TabIndex = 3;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // loadGroup
            // 
            this.loadGroup.Controls.Add(this.loadBtn);
            this.loadGroup.Location = new System.Drawing.Point(155, 12);
            this.loadGroup.Name = "loadGroup";
            this.loadGroup.Size = new System.Drawing.Size(121, 65);
            this.loadGroup.TabIndex = 7;
            this.loadGroup.TabStop = false;
            this.loadGroup.Text = "Load state";
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(13, 22);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(95, 32);
            this.loadBtn.TabIndex = 9;
            this.loadBtn.Text = "Load from file";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // saveGroup
            // 
            this.saveGroup.Controls.Add(this.autoSaveBtn);
            this.saveGroup.Controls.Add(this.saveBtn);
            this.saveGroup.Location = new System.Drawing.Point(12, 12);
            this.saveGroup.Name = "saveGroup";
            this.saveGroup.Size = new System.Drawing.Size(121, 108);
            this.saveGroup.TabIndex = 8;
            this.saveGroup.TabStop = false;
            this.saveGroup.Text = "Save";
            // 
            // autoSaveBtn
            // 
            this.autoSaveBtn.Location = new System.Drawing.Point(12, 66);
            this.autoSaveBtn.Name = "autoSaveBtn";
            this.autoSaveBtn.Size = new System.Drawing.Size(95, 32);
            this.autoSaveBtn.TabIndex = 1;
            this.autoSaveBtn.Text = "Auto save";
            this.autoSaveBtn.UseVisualStyleBackColor = true;
            this.autoSaveBtn.Click += new System.EventHandler(this.autoSaveBtn_Click);
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(24, 133);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(95, 32);
            this.connectBtn.TabIndex = 9;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numOfCtrlLabel);
            this.groupBox1.Controls.Add(this.randomBtn);
            this.groupBox1.Controls.Add(this.randomLvarsScroll);
            this.groupBox1.Location = new System.Drawing.Point(155, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 124);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Randomizer";
            // 
            // numOfCtrlLabel
            // 
            this.numOfCtrlLabel.AutoSize = true;
            this.numOfCtrlLabel.Location = new System.Drawing.Point(13, 53);
            this.numOfCtrlLabel.Name = "numOfCtrlLabel";
            this.numOfCtrlLabel.Size = new System.Drawing.Size(59, 15);
            this.numOfCtrlLabel.TabIndex = 14;
            this.numOfCtrlLabel.Text = "0 controls";
            // 
            // randomBtn
            // 
            this.randomBtn.Location = new System.Drawing.Point(13, 80);
            this.randomBtn.Name = "randomBtn";
            this.randomBtn.Size = new System.Drawing.Size(95, 32);
            this.randomBtn.TabIndex = 12;
            this.randomBtn.Text = "Randomize";
            this.randomBtn.UseVisualStyleBackColor = true;
            this.randomBtn.Click += new System.EventHandler(this.randomBtn_Click);
            // 
            // randomLvarsScroll
            // 
            this.randomLvarsScroll.Location = new System.Drawing.Point(13, 22);
            this.randomLvarsScroll.Name = "randomLvarsScroll";
            this.randomLvarsScroll.Size = new System.Drawing.Size(95, 45);
            this.randomLvarsScroll.TabIndex = 13;
            this.randomLvarsScroll.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.randomLvarsScroll.Scroll += new System.EventHandler(this.randomLvarsScroll_Scroll);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 221);
            this.statusLabel.MinimumSize = new System.Drawing.Size(260, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(260, 15);
            this.statusLabel.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 245);
            this.ControlBox = false;
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.loadGroup);
            this.Controls.Add(this.saveGroup);
            this.Controls.Add(this.exitBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.loadGroup.ResumeLayout(false);
            this.saveGroup.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.randomLvarsScroll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.GroupBox loadGroup;
        private System.Windows.Forms.GroupBox saveGroup;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Button autoSaveBtn;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label numOfCtrlLabel;
        private System.Windows.Forms.Button randomBtn;
        private System.Windows.Forms.TrackBar randomLvarsScroll;
        private System.Windows.Forms.Label statusLabel;
    }
}

