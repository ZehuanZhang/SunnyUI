namespace SunnyUI.Demo
{
    partial class FMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("控件");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("窗体");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("主题");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.uiLogo1 = new SunnyUI.UILogo();
            this.uiAvatar = new SunnyUI.UIAvatar();
            this.StyleManager = new SunnyUI.UIStyleManager(this.components);
            this.Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // Aside
            // 
            this.Aside.ItemHeight = 36;
            this.Aside.LineColor = System.Drawing.Color.Black;
            this.Aside.Location = new System.Drawing.Point(1, 145);
            this.Aside.Size = new System.Drawing.Size(250, 574);
            // 
            // Header
            // 
            this.Header.Controls.Add(this.uiAvatar);
            this.Header.Controls.Add(this.uiLogo1);
            this.Header.Location = new System.Drawing.Point(1, 35);
            treeNode1.Name = "节点0";
            treeNode1.Text = "控件";
            treeNode2.Name = "节点1";
            treeNode2.Text = "窗体";
            treeNode3.Name = "节点2";
            treeNode3.Text = "主题";
            this.Header.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.Header.SelectedIndex = 0;
            this.Header.Size = new System.Drawing.Size(1022, 110);
            this.Header.MenuItemClick += new SunnyUI.UINavBar.OnMenuItemClick(this.Header_MenuItemClick);
            // 
            // Main
            // 
            this.Main.Location = new System.Drawing.Point(251, 145);
            this.Main.Size = new System.Drawing.Size(772, 574);
            // 
            // uiLogo1
            // 
            this.uiLogo1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLogo1.Location = new System.Drawing.Point(20, 15);
            this.uiLogo1.MaximumSize = new System.Drawing.Size(300, 80);
            this.uiLogo1.MinimumSize = new System.Drawing.Size(300, 80);
            this.uiLogo1.Name = "uiLogo1";
            this.uiLogo1.Size = new System.Drawing.Size(300, 80);
            this.uiLogo1.Style = SunnyUI.UIStyle.Custom;
            this.uiLogo1.TabIndex = 3;
            this.uiLogo1.Text = "uiLogo1";
            // 
            // uiAvatar
            // 
            this.uiAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiAvatar.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiAvatar.Location = new System.Drawing.Point(941, 25);
            this.uiAvatar.Name = "uiAvatar";
            this.uiAvatar.Size = new System.Drawing.Size(65, 61);
            this.uiAvatar.TabIndex = 4;
            this.uiAvatar.Text = "uiAvatar1";
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 720);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.Padding = new System.Windows.Forms.Padding(1, 35, 1, 1);
            this.Text = "SunnyUI.Net";
            this.Header.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UILogo uiLogo1;
        private UIAvatar uiAvatar;
        private UIStyleManager StyleManager;
    }
}