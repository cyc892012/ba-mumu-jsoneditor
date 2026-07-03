namespace MuMu坐标计算
{
    partial class KeyPresetForm
    {
        private System.ComponentModel.IContainer components = null;

        private MuMu坐标计算.SearchableComboBox searchKeysCombo;
        private System.Windows.Forms.Button WriteKeysButton;
        private System.Windows.Forms.Button WriteKeyButton;
        private System.Windows.Forms.Button DeleteRepeatKeysButton;
        private System.Windows.Forms.Button DeleteRangeRDkeysButton;
        private System.Windows.Forms.Button importKeymapbutton;
        private System.Windows.Forms.Button openPresetJsonFolderbutton;
        private System.Windows.Forms.Button deleteDataJsonbutton;
        private System.Windows.Forms.TextBox Button2textBox;
        private System.Windows.Forms.Button ReadPP2Button;
        private System.Windows.Forms.Label lblBaseKeys;
        private System.Windows.Forms.Label lblPresetRead;
        private System.Windows.Forms.CheckBox _topCheckBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.searchKeysCombo = new MuMu坐标计算.SearchableComboBox();
            this.WriteKeysButton = new System.Windows.Forms.Button();
            this.WriteKeyButton = new System.Windows.Forms.Button();
            this.DeleteRepeatKeysButton = new System.Windows.Forms.Button();
            this.DeleteRangeRDkeysButton = new System.Windows.Forms.Button();
            this.importKeymapbutton = new System.Windows.Forms.Button();
            this.openPresetJsonFolderbutton = new System.Windows.Forms.Button();
            this.deleteDataJsonbutton = new System.Windows.Forms.Button();
            this.Button2textBox = new System.Windows.Forms.TextBox();
            this.ReadPP2Button = new System.Windows.Forms.Button();
            this.lblBaseKeys = new System.Windows.Forms.Label();
            this.lblPresetRead = new System.Windows.Forms.Label();
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // searchKeysCombo
            // 
            this.searchKeysCombo.DataSource = null;
            this.searchKeysCombo.DisplayMember = "";
            this.searchKeysCombo.DropDownHeight = 100;
            this.searchKeysCombo.DroppedDown = false;
            this.searchKeysCombo.Location = new System.Drawing.Point(77, 39);
            this.searchKeysCombo.Name = "searchKeysCombo";
            this.searchKeysCombo.SelectedIndex = -1;
            this.searchKeysCombo.SelectedItem = null;
            this.searchKeysCombo.Size = new System.Drawing.Size(163, 25);
            this.searchKeysCombo.TabIndex = 2;
            this.searchKeysCombo.ValueMember = "";
            // 
            // WriteKeysButton
            // 
            this.WriteKeysButton.Location = new System.Drawing.Point(12, 70);
            this.WriteKeysButton.Name = "WriteKeysButton";
            this.WriteKeysButton.Size = new System.Drawing.Size(72, 28);
            this.WriteKeysButton.TabIndex = 3;
            this.WriteKeysButton.Text = "写入所有";
            this.WriteKeysButton.UseVisualStyleBackColor = true;
            // 
            // WriteKeyButton
            // 
            this.WriteKeyButton.Location = new System.Drawing.Point(90, 70);
            this.WriteKeyButton.Name = "WriteKeyButton";
            this.WriteKeyButton.Size = new System.Drawing.Size(72, 28);
            this.WriteKeyButton.TabIndex = 4;
            this.WriteKeyButton.Text = "写入单个";
            this.WriteKeyButton.UseVisualStyleBackColor = true;
            // 
            // DeleteRepeatKeysButton
            // 
            this.DeleteRepeatKeysButton.Location = new System.Drawing.Point(168, 70);
            this.DeleteRepeatKeysButton.Name = "DeleteRepeatKeysButton";
            this.DeleteRepeatKeysButton.Size = new System.Drawing.Size(72, 28);
            this.DeleteRepeatKeysButton.TabIndex = 5;
            this.DeleteRepeatKeysButton.Text = "键位去重";
            this.DeleteRepeatKeysButton.UseVisualStyleBackColor = true;
            // 
            // DeleteRangeRDkeysButton
            // 
            this.DeleteRangeRDkeysButton.Location = new System.Drawing.Point(246, 70);
            this.DeleteRangeRDkeysButton.Name = "DeleteRangeRDkeysButton";
            this.DeleteRangeRDkeysButton.Size = new System.Drawing.Size(72, 28);
            this.DeleteRangeRDkeysButton.TabIndex = 6;
            this.DeleteRangeRDkeysButton.Text = "右下清空";
            this.DeleteRangeRDkeysButton.UseVisualStyleBackColor = true;
            // 
            // importKeymapbutton
            // 
            this.importKeymapbutton.Location = new System.Drawing.Point(324, 70);
            this.importKeymapbutton.Name = "importKeymapbutton";
            this.importKeymapbutton.Size = new System.Drawing.Size(72, 28);
            this.importKeymapbutton.TabIndex = 7;
            this.importKeymapbutton.Text = "导入";
            this.importKeymapbutton.UseVisualStyleBackColor = true;
            // 
            // openPresetJsonFolderbutton
            // 
            this.openPresetJsonFolderbutton.Location = new System.Drawing.Point(402, 70);
            this.openPresetJsonFolderbutton.Name = "openPresetJsonFolderbutton";
            this.openPresetJsonFolderbutton.Size = new System.Drawing.Size(72, 28);
            this.openPresetJsonFolderbutton.TabIndex = 8;
            this.openPresetJsonFolderbutton.Text = "打开";
            this.openPresetJsonFolderbutton.UseVisualStyleBackColor = true;
            // 
            // deleteDataJsonbutton
            // 
            this.deleteDataJsonbutton.Location = new System.Drawing.Point(480, 70);
            this.deleteDataJsonbutton.Name = "deleteDataJsonbutton";
            this.deleteDataJsonbutton.Size = new System.Drawing.Size(72, 28);
            this.deleteDataJsonbutton.TabIndex = 9;
            this.deleteDataJsonbutton.Text = "删除";
            this.deleteDataJsonbutton.UseVisualStyleBackColor = true;
            // 
            // Button2textBox
            // 
            this.Button2textBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Button2textBox.Location = new System.Drawing.Point(125, 109);
            this.Button2textBox.Name = "Button2textBox";
            this.Button2textBox.Size = new System.Drawing.Size(36, 21);
            this.Button2textBox.TabIndex = 11;
            // 
            // ReadPP2Button
            // 
            this.ReadPP2Button.Location = new System.Drawing.Point(167, 108);
            this.ReadPP2Button.Name = "ReadPP2Button";
            this.ReadPP2Button.Size = new System.Drawing.Size(55, 23);
            this.ReadPP2Button.TabIndex = 12;
            this.ReadPP2Button.Text = "读取";
            this.ReadPP2Button.UseVisualStyleBackColor = true;
            // 
            // lblBaseKeys
            // 
            this.lblBaseKeys.AutoSize = true;
            this.lblBaseKeys.Location = new System.Drawing.Point(12, 42);
            this.lblBaseKeys.Name = "lblBaseKeys";
            this.lblBaseKeys.Size = new System.Drawing.Size(59, 12);
            this.lblBaseKeys.TabIndex = 1;
            this.lblBaseKeys.Text = "基础键位:";
            // 
            // lblPresetRead
            // 
            this.lblPresetRead.AutoSize = true;
            this.lblPresetRead.Location = new System.Drawing.Point(12, 112);
            this.lblPresetRead.Name = "lblPresetRead";
            this.lblPresetRead.Size = new System.Drawing.Size(107, 12);
            this.lblPresetRead.TabIndex = 10;
            this.lblPresetRead.Text = "从预设读取: 按键:";
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(12, 12);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(72, 16);
            this._topCheckBox.TabIndex = 0;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // KeyPresetForm
            // 
            this.ClientSize = new System.Drawing.Size(564, 144);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this.lblBaseKeys);
            this.Controls.Add(this.searchKeysCombo);
            this.Controls.Add(this.WriteKeysButton);
            this.Controls.Add(this.WriteKeyButton);
            this.Controls.Add(this.DeleteRepeatKeysButton);
            this.Controls.Add(this.DeleteRangeRDkeysButton);
            this.Controls.Add(this.importKeymapbutton);
            this.Controls.Add(this.openPresetJsonFolderbutton);
            this.Controls.Add(this.deleteDataJsonbutton);
            this.Controls.Add(this.lblPresetRead);
            this.Controls.Add(this.Button2textBox);
            this.Controls.Add(this.ReadPP2Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::MuMu坐标计算.Properties.Resources.AppIcon;
            this.MaximizeBox = false;
            this.Name = "KeyPresetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "基础键位预设";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
