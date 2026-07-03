using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class IndexBackupForm
    {
        private System.ComponentModel.IContainer components = null;
        internal ComboBox _indexFileComboBox;
        internal Button _refreshButton;
        internal Button _manualBackupButton;
        internal Button _restoreButton;
        internal Button _cleanupButton;
        internal Button _scanOrphansButton;
        internal Button _addToIndexButton;
        internal ListView _backupListView;
        internal Label _backupTipLabel;
        internal CheckBox _topCheckBox;
        internal CheckedListBox _orphanListBox;
        internal Label _orphanLabel;
        internal CheckBox _selectAllCheckBox;
        internal TextBox _searchTextBox;
        internal Label _warningLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IndexBackupForm));
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._indexFileComboBox = new System.Windows.Forms.ComboBox();
            this._refreshButton = new System.Windows.Forms.Button();
            this._manualBackupButton = new System.Windows.Forms.Button();
            this._restoreButton = new System.Windows.Forms.Button();
            this._cleanupButton = new System.Windows.Forms.Button();
            this._scanOrphansButton = new System.Windows.Forms.Button();
            this._addToIndexButton = new System.Windows.Forms.Button();
            this._orphanLabel = new System.Windows.Forms.Label();
            this._searchTextBox = new System.Windows.Forms.TextBox();
            this._selectAllCheckBox = new System.Windows.Forms.CheckBox();
            this._backupListView = new System.Windows.Forms.ListView();
            this._backupTipLabel = new System.Windows.Forms.Label();
            this._orphanListBox = new System.Windows.Forms.CheckedListBox();
            this._warningLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(6, 10);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 0;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _indexFileComboBox
            // 
            this._indexFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._indexFileComboBox.FormattingEnabled = true;
            this._indexFileComboBox.Location = new System.Drawing.Point(85, 10);
            this._indexFileComboBox.Name = "_indexFileComboBox";
            this._indexFileComboBox.Size = new System.Drawing.Size(200, 25);
            this._indexFileComboBox.TabIndex = 1;
            // 
            // _refreshButton
            // 
            this._refreshButton.Location = new System.Drawing.Point(290, 9);
            this._refreshButton.Name = "_refreshButton";
            this._refreshButton.Size = new System.Drawing.Size(60, 23);
            this._refreshButton.TabIndex = 2;
            this._refreshButton.Text = "刷新";
            this._refreshButton.UseVisualStyleBackColor = true;
            // 
            // _manualBackupButton
            // 
            this._manualBackupButton.Enabled = false;
            this._manualBackupButton.Location = new System.Drawing.Point(355, 9);
            this._manualBackupButton.Name = "_manualBackupButton";
            this._manualBackupButton.Size = new System.Drawing.Size(75, 23);
            this._manualBackupButton.TabIndex = 3;
            this._manualBackupButton.Text = "手动备份";
            this._manualBackupButton.UseVisualStyleBackColor = true;
            // 
            // _restoreButton
            // 
            this._restoreButton.Enabled = false;
            this._restoreButton.Location = new System.Drawing.Point(435, 9);
            this._restoreButton.Name = "_restoreButton";
            this._restoreButton.Size = new System.Drawing.Size(75, 23);
            this._restoreButton.TabIndex = 4;
            this._restoreButton.Text = "还原选中";
            this._restoreButton.UseVisualStyleBackColor = true;
            // 
            // _cleanupButton
            // 
            this._cleanupButton.Location = new System.Drawing.Point(515, 9);
            this._cleanupButton.Name = "_cleanupButton";
            this._cleanupButton.Size = new System.Drawing.Size(75, 23);
            this._cleanupButton.TabIndex = 5;
            this._cleanupButton.Text = "清理旧备份";
            this._cleanupButton.UseVisualStyleBackColor = true;
            // 
            // _scanOrphansButton
            // 
            this._scanOrphansButton.Enabled = false;
            this._scanOrphansButton.Location = new System.Drawing.Point(595, 9);
            this._scanOrphansButton.Name = "_scanOrphansButton";
            this._scanOrphansButton.Size = new System.Drawing.Size(70, 23);
            this._scanOrphansButton.TabIndex = 6;
            this._scanOrphansButton.Text = "扫描缺失";
            this._scanOrphansButton.UseVisualStyleBackColor = true;
            // 
            // _addToIndexButton
            // 
            this._addToIndexButton.Enabled = false;
            this._addToIndexButton.Location = new System.Drawing.Point(6, 38);
            this._addToIndexButton.Name = "_addToIndexButton";
            this._addToIndexButton.Size = new System.Drawing.Size(80, 23);
            this._addToIndexButton.TabIndex = 7;
            this._addToIndexButton.Text = "补全索引";
            this._addToIndexButton.UseVisualStyleBackColor = true;
            // 
            // _orphanLabel
            // 
            this._orphanLabel.AutoSize = true;
            this._orphanLabel.Location = new System.Drawing.Point(90, 42);
            this._orphanLabel.Name = "_orphanLabel";
            this._orphanLabel.Size = new System.Drawing.Size(0, 17);
            this._orphanLabel.TabIndex = 8;
            // 
            // _searchTextBox
            // 
            this._searchTextBox.Location = new System.Drawing.Point(6, 263);
            this._searchTextBox.Name = "_searchTextBox";
            this._searchTextBox.Size = new System.Drawing.Size(180, 23);
            this._searchTextBox.TabIndex = 9;
            this._searchTextBox.Visible = false;
            // 
            // _selectAllCheckBox
            // 
            this._selectAllCheckBox.AutoSize = true;
            this._selectAllCheckBox.Location = new System.Drawing.Point(195, 265);
            this._selectAllCheckBox.Name = "_selectAllCheckBox";
            this._selectAllCheckBox.Size = new System.Drawing.Size(51, 21);
            this._selectAllCheckBox.TabIndex = 10;
            this._selectAllCheckBox.Text = "全选";
            this._selectAllCheckBox.UseVisualStyleBackColor = true;
            this._selectAllCheckBox.Visible = false;
            // 
            // _backupListView
            // 
            this._backupListView.FullRowSelect = true;
            this._backupListView.HideSelection = false;
            this._backupListView.Location = new System.Drawing.Point(6, 67);
            this._backupListView.Name = "_backupListView";
            this._backupListView.Size = new System.Drawing.Size(658, 170);
            this._backupListView.TabIndex = 11;
            this._backupListView.UseCompatibleStateImageBehavior = false;
            this._backupListView.View = System.Windows.Forms.View.Details;
            // 
            // _backupTipLabel
            // 
            this._backupTipLabel.AutoSize = true;
            this._backupTipLabel.Location = new System.Drawing.Point(6, 240);
            this._backupTipLabel.Name = "_backupTipLabel";
            this._backupTipLabel.Size = new System.Drawing.Size(0, 17);
            this._backupTipLabel.TabIndex = 12;
            // 
            // _orphanListBox
            // 
            this._orphanListBox.CheckOnClick = true;
            this._orphanListBox.Location = new System.Drawing.Point(6, 288);
            this._orphanListBox.Name = "_orphanListBox";
            this._orphanListBox.Size = new System.Drawing.Size(658, 220);
            this._orphanListBox.TabIndex = 13;
            this._orphanListBox.Visible = false;
            // 
            // _warningLabel
            // 
            this._warningLabel.AutoSize = true;
            this._warningLabel.ForeColor = System.Drawing.Color.Red;
            this._warningLabel.Location = new System.Drawing.Point(6, 530);
            this._warningLabel.Name = "_warningLabel";
            this._warningLabel.Size = new System.Drawing.Size(515, 17);
            this._warningLabel.TabIndex = 14;
            this._warningLabel.Text = "提示：修改索引文件（补全/还原）前请先关闭 MuMu 模拟器，完成后再启动模拟器即可生效。";
            // 
            // IndexBackupForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(670, 585);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this._indexFileComboBox);
            this.Controls.Add(this._refreshButton);
            this.Controls.Add(this._manualBackupButton);
            this.Controls.Add(this._restoreButton);
            this.Controls.Add(this._cleanupButton);
            this.Controls.Add(this._scanOrphansButton);
            this.Controls.Add(this._addToIndexButton);
            this.Controls.Add(this._orphanLabel);
            this.Controls.Add(this._searchTextBox);
            this.Controls.Add(this._selectAllCheckBox);
            this.Controls.Add(this._backupListView);
            this.Controls.Add(this._backupTipLabel);
            this.Controls.Add(this._orphanListBox);
            this.Controls.Add(this._warningLabel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "IndexBackupForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "索引备份管理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
