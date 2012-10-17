namespace Mahjong
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTileFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.modeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.panelView = new Mahjong.PanelView();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.gridComboBox,
            this.modeComboBox,
            this.saveTileFieldToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(1167, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(97, 21);
            this.gameToolStripMenuItem.Text = "Start New Game";
            // 
            // saveTileFieldToolStripMenuItem
            // 
            this.saveTileFieldToolStripMenuItem.Name = "saveTileFieldToolStripMenuItem";
            this.saveTileFieldToolStripMenuItem.Size = new System.Drawing.Size(84, 21);
            this.saveTileFieldToolStripMenuItem.Text = "Save TileField";
            this.saveTileFieldToolStripMenuItem.Click += new System.EventHandler(this.saveTileFieldToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gridComboBox
            // 
            this.gridComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridComboBox.Items.AddRange(new object[] {
            "Show Grid",
            "Do Not Show Grid"});
            this.gridComboBox.Name = "gridComboBox";
            this.gridComboBox.Size = new System.Drawing.Size(121, 21);
            this.gridComboBox.SelectedIndexChanged += new System.EventHandler(this.gridComboBox_SelectedIndexChanged);
            // 
            // modeComboBox
            // 
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.Items.AddRange(new object[] {
            "Play",
            "Edit"});
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(121, 21);
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // panelView
            // 
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.DrawGrid = false;
            this.panelView.Field = null;
            this.panelView.Location = new System.Drawing.Point(0, 25);
            this.panelView.Mode = Mahjong.PanelMode.Play;
            this.panelView.Name = "panelView";
            this.panelView.Selected = null;
            this.panelView.Size = new System.Drawing.Size(1167, 495);
            this.panelView.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 520);
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Mahjong";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private PanelView panelView;
        private System.Windows.Forms.ToolStripMenuItem saveTileFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox gridComboBox;
        private System.Windows.Forms.ToolStripComboBox modeComboBox;
    }
}

