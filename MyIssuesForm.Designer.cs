using System.Drawing;
using System.Windows.Forms;

namespace ExampleCsPlugin
{
    partial class MyIssuesForm
    {
        public int window_width = 1000;
        public int window_height = 400;
        public int page_button_size_width = 80;
        public int page_button_size_height = 25;
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.comboBoxSearchStatus = new System.Windows.Forms.ComboBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.comboBoxPlanning = new System.Windows.Forms.ComboBox();
            this.checkboxCheck = new System.Windows.Forms.CheckBox();
            this.configButton = new System.Windows.Forms.Button();
            this.checkBranchView = new UserControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listViewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            /*            // 
            */
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(window_width - 120, window_height );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 40);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += (sender, e) => this.Close();
            // 
            // comboBoxSearchStatus
            // 
            this.comboBoxSearchStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxSearchStatus.FormattingEnabled = true;
            this.comboBoxSearchStatus.Items.AddRange(new object[] {
            "全部",
            "开始",
            "开发中",
            "策划验收中",
            "测试中",
            "缺陷修复中",
            "缺陷测试中",
            });
            this.comboBoxSearchStatus.Location = new System.Drawing.Point(120, 5);
            this.comboBoxSearchStatus.Name = "comboBoxSearchStatus";
            this.comboBoxSearchStatus.Size = new System.Drawing.Size(90, 25);
            this.comboBoxSearchStatus.TabIndex = 3;
            this.comboBoxSearchStatus.SelectedIndex = 0;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(216, 5);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(239, 21);
            this.textBoxSearch.TabIndex = 5;
            this.textBoxSearch.KeyDown += this.textBoxSearchKeyDown;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(461, 5);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 6;
            this.searchButton.Text = "搜索";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // configButton
            // 
            this.configButton.Location = new System.Drawing.Point(0, 35 + page_button_size_height);
            this.configButton.Name = "configButton";
            this.configButton.Size = new System.Drawing.Size(page_button_size_width, 25);
            this.configButton.TabIndex = 6;
            this.configButton.Text = "分支检查";
            this.configButton.UseVisualStyleBackColor = true;
            this.configButton.Click += new System.EventHandler(this.configButtonClicked);            // 
            // listViewButton
            // 
            // 
            this.listViewButton.Location = new System.Drawing.Point(0, 35);
            this.listViewButton.Name = "listViewButton";
            this.listViewButton.Size = new System.Drawing.Size(page_button_size_width, 25);
            this.listViewButton.TabIndex = 6;
            this.listViewButton.Text = "提交信息";
            this.listViewButton.UseVisualStyleBackColor = true;
            this.listViewButton.Click += new System.EventHandler(this.listViewButtonClicked);
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(page_button_size_width, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(window_width - 100, window_height - 50);
            this.panel1.TabIndex = 10;
            this.panel1.Controls.Add(this.checkBranchView);
            this.panel1.BackColor = Color.White;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(page_button_size_width, 34);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(window_width-100,window_height-50);
            this.listView1.SmallImageList = this.imgList;
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += this.listViewItemDoubelClick;
            // 
            // imgList
            // 
            this.imgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgList.ImageSize = new System.Drawing.Size(1, 20);
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // comboBoxPlanning
            // 
            this.comboBoxPlanning.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlanning.DropDownWidth = 90;
            this.comboBoxPlanning.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxPlanning.FormattingEnabled = true;
            this.comboBoxPlanning.ItemHeight = 17;
            this.comboBoxPlanning.Location = new System.Drawing.Point(12, 5);
            this.comboBoxPlanning.Name = "comboBoxPlanning";
            this.comboBoxPlanning.Size = new System.Drawing.Size(102, 25);
            this.comboBoxPlanning.TabIndex = 8;
            // 
            // checkBranchView
            // 
            // 
            this.checkBranchView.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.checkBranchView.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.checkBranchView.ClientSize = new System.Drawing.Size(284, 244);
            this.checkBranchView.Name = "checkBranchForm";
            this.checkBranchView.Text = "提交分支检查";
            this.checkBranchView.ResumeLayout(false);
            this.checkBranchView.PerformLayout();
            this.checkBranchView.Controls.Add(checkboxCheck);
            // 
            // checkboxCheck
            // 
            this.checkboxCheck.AutoSize = true;
            this.checkboxCheck.Location = new System.Drawing.Point(44, 52);
            this.checkboxCheck.Name = "checkbox1";
            this.checkboxCheck.Size = new System.Drawing.Size(120, 16);
            this.checkboxCheck.TabIndex = 12;
            this.checkboxCheck.Text = "启用提交分支检查";
            this.checkboxCheck.UseVisualStyleBackColor = true;
            this.checkboxCheck.CheckedChanged += this.optionsChecked;

            // MyIssuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(window_width,window_height );
            this.Controls.Add(this.comboBoxPlanning);
            this.Controls.Add(this.listView1);
            this.Controls.Add(configButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.comboBoxSearchStatus);
            this.Controls.Add(this.listViewButton);
            this.Controls.Add(this.cancelButton);
            /*this.cancelButton.Visible = false;*/
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyIssuesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提交信息选取";
            this.Load += new System.EventHandler(this.MyIssuesForm_Load);
            this.Click += new System.EventHandler(this.okButton_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox comboBoxSearchStatus;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button configButton;
        private System.Windows.Forms.UserControl checkBranchView;
        public System.Windows.Forms.CheckBox checkboxCheck;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button listViewButton;
        private ImageList imgList;
        private ComboBox comboBoxPlanning;
    }
}