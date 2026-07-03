using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class WideScreenForm
    {
        private System.ComponentModel.IContainer components = null;
        internal ComboBox _packageNamecomboBox;
        internal SearchableComboBox _fileNameSearchCombo;
        internal ComboBox _resolutionTypecomboBox;
        internal ComboBox _resolutioncomboBox;
        internal CheckBox _topCheckBox;
        internal Button _ktckReadbutton;
        internal Button _ktckOPWritebutton;
        internal Button _ktckAPWritebutton;
        internal CheckedListBox _ktckPListcheckedListBox;
        internal TextBox _ktckKXtextBox;
        internal TextBox _ktckKYtextBox;
        internal TextBox _ktckCKXtextBox;
        internal TextBox _ktckCKYtextBox;
        internal Label _lblSourceRes;
        internal Label _lblWideScreenTip;
        internal Label _lblSelectedCoord;
        internal Label _lblWideScreenCoord;
        internal Label _lblColon2;
        internal Label _lblColon4;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WideScreenForm));
            this._lblWideScreenTip = new System.Windows.Forms.Label();
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._packageNamecomboBox = new System.Windows.Forms.ComboBox();
            this._fileNameSearchCombo = new MuMu坐标计算.SearchableComboBox();
            this._lblSourceRes = new System.Windows.Forms.Label();
            this._resolutionTypecomboBox = new System.Windows.Forms.ComboBox();
            this._resolutioncomboBox = new System.Windows.Forms.ComboBox();
            this._ktckReadbutton = new System.Windows.Forms.Button();
            this._ktckPListcheckedListBox = new System.Windows.Forms.CheckedListBox();
            this._lblSelectedCoord = new System.Windows.Forms.Label();
            this._ktckKXtextBox = new System.Windows.Forms.TextBox();
            this._lblColon2 = new System.Windows.Forms.Label();
            this._ktckKYtextBox = new System.Windows.Forms.TextBox();
            this._lblWideScreenCoord = new System.Windows.Forms.Label();
            this._ktckCKXtextBox = new System.Windows.Forms.TextBox();
            this._lblColon4 = new System.Windows.Forms.Label();
            this._ktckCKYtextBox = new System.Windows.Forms.TextBox();
            this._ktckOPWritebutton = new System.Windows.Forms.Button();
            this._ktckAPWritebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lblWideScreenTip
            // 
            this._lblWideScreenTip.AutoSize = true;
            this._lblWideScreenTip.ForeColor = System.Drawing.Color.Red;
            this._lblWideScreenTip.Location = new System.Drawing.Point(260, 190);
            this._lblWideScreenTip.Name = "_lblWideScreenTip";
            this._lblWideScreenTip.Size = new System.Drawing.Size(212, 17);
            this._lblWideScreenTip.TabIndex = 18;
            this._lblWideScreenTip.Text = "提示：测试向功能，出现问题请转人工";
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(3, 14);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 0;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _packageNamecomboBox
            // 
            this._packageNamecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._packageNamecomboBox.FormattingEnabled = true;
            this._packageNamecomboBox.Location = new System.Drawing.Point(86, 14);
            this._packageNamecomboBox.Name = "_packageNamecomboBox";
            this._packageNamecomboBox.Size = new System.Drawing.Size(68, 25);
            this._packageNamecomboBox.TabIndex = 1;
            // 
            // _fileNameSearchCombo
            // 
            this._fileNameSearchCombo.DataSource = null;
            this._fileNameSearchCombo.DisplayMember = "";
            this._fileNameSearchCombo.DropDownHeight = 200;
            this._fileNameSearchCombo.DroppedDown = false;
            this._fileNameSearchCombo.Location = new System.Drawing.Point(160, 14);
            this._fileNameSearchCombo.Name = "_fileNameSearchCombo";
            this._fileNameSearchCombo.SelectedIndex = -1;
            this._fileNameSearchCombo.SelectedItem = null;
            this._fileNameSearchCombo.Size = new System.Drawing.Size(248, 25);
            this._fileNameSearchCombo.TabIndex = 2;
            this._fileNameSearchCombo.ValueMember = "";
            // 
            // _lblSourceRes
            // 
            this._lblSourceRes.AutoSize = true;
            this._lblSourceRes.Location = new System.Drawing.Point(7, 49);
            this._lblSourceRes.Name = "_lblSourceRes";
            this._lblSourceRes.Size = new System.Drawing.Size(116, 17);
            this._lblSourceRes.TabIndex = 3;
            this._lblSourceRes.Text = "待读取文件分辨率：";
            // 
            // _resolutionTypecomboBox
            // 
            this._resolutionTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._resolutionTypecomboBox.FormattingEnabled = true;
            this._resolutionTypecomboBox.Location = new System.Drawing.Point(129, 44);
            this._resolutionTypecomboBox.Name = "_resolutionTypecomboBox";
            this._resolutionTypecomboBox.Size = new System.Drawing.Size(68, 25);
            this._resolutionTypecomboBox.TabIndex = 4;
            // 
            // _resolutioncomboBox
            // 
            this._resolutioncomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._resolutioncomboBox.FormattingEnabled = true;
            this._resolutioncomboBox.Location = new System.Drawing.Point(201, 44);
            this._resolutioncomboBox.Name = "_resolutioncomboBox";
            this._resolutioncomboBox.Size = new System.Drawing.Size(155, 25);
            this._resolutioncomboBox.TabIndex = 5;
            // 
            // _ktckReadbutton
            // 
            this._ktckReadbutton.Location = new System.Drawing.Point(415, 12);
            this._ktckReadbutton.Name = "_ktckReadbutton";
            this._ktckReadbutton.Size = new System.Drawing.Size(75, 23);
            this._ktckReadbutton.TabIndex = 6;
            this._ktckReadbutton.Text = "读取";
            this._ktckReadbutton.UseVisualStyleBackColor = true;
            // 
            // _ktckPListcheckedListBox
            // 
            this._ktckPListcheckedListBox.FormattingEnabled = true;
            this._ktckPListcheckedListBox.Items.AddRange(new object[] {
            "全选"});
            this._ktckPListcheckedListBox.Location = new System.Drawing.Point(9, 70);
            this._ktckPListcheckedListBox.Name = "_ktckPListcheckedListBox";
            this._ktckPListcheckedListBox.Size = new System.Drawing.Size(120, 94);
            this._ktckPListcheckedListBox.TabIndex = 7;
            // 
            // _lblSelectedCoord
            // 
            this._lblSelectedCoord.AutoSize = true;
            this._lblSelectedCoord.Location = new System.Drawing.Point(139, 80);
            this._lblSelectedCoord.Name = "_lblSelectedCoord";
            this._lblSelectedCoord.Size = new System.Drawing.Size(92, 17);
            this._lblSelectedCoord.TabIndex = 8;
            this._lblSelectedCoord.Text = "当前选择坐标：";
            // 
            // _ktckKXtextBox
            // 
            this._ktckKXtextBox.Location = new System.Drawing.Point(232, 76);
            this._ktckKXtextBox.Name = "_ktckKXtextBox";
            this._ktckKXtextBox.ReadOnly = true;
            this._ktckKXtextBox.Size = new System.Drawing.Size(42, 23);
            this._ktckKXtextBox.TabIndex = 9;
            this._ktckKXtextBox.Text = "0";
            // 
            // _lblColon2
            // 
            this._lblColon2.AutoSize = true;
            this._lblColon2.Location = new System.Drawing.Point(278, 80);
            this._lblColon2.Name = "_lblColon2";
            this._lblColon2.Size = new System.Drawing.Size(20, 17);
            this._lblColon2.TabIndex = 10;
            this._lblColon2.Text = "：";
            // 
            // _ktckKYtextBox
            // 
            this._ktckKYtextBox.Location = new System.Drawing.Point(299, 76);
            this._ktckKYtextBox.Name = "_ktckKYtextBox";
            this._ktckKYtextBox.ReadOnly = true;
            this._ktckKYtextBox.Size = new System.Drawing.Size(42, 23);
            this._ktckKYtextBox.TabIndex = 11;
            this._ktckKYtextBox.Text = "0";
            // 
            // _lblWideScreenCoord
            // 
            this._lblWideScreenCoord.AutoSize = true;
            this._lblWideScreenCoord.Location = new System.Drawing.Point(139, 111);
            this._lblWideScreenCoord.Name = "_lblWideScreenCoord";
            this._lblWideScreenCoord.Size = new System.Drawing.Size(92, 17);
            this._lblWideScreenCoord.TabIndex = 12;
            this._lblWideScreenCoord.Text = "对应宽屏坐标：";
            // 
            // _ktckCKXtextBox
            // 
            this._ktckCKXtextBox.Location = new System.Drawing.Point(232, 109);
            this._ktckCKXtextBox.Name = "_ktckCKXtextBox";
            this._ktckCKXtextBox.ReadOnly = true;
            this._ktckCKXtextBox.Size = new System.Drawing.Size(42, 23);
            this._ktckCKXtextBox.TabIndex = 13;
            this._ktckCKXtextBox.Text = "0";
            // 
            // _lblColon4
            // 
            this._lblColon4.AutoSize = true;
            this._lblColon4.Location = new System.Drawing.Point(278, 112);
            this._lblColon4.Name = "_lblColon4";
            this._lblColon4.Size = new System.Drawing.Size(20, 17);
            this._lblColon4.TabIndex = 14;
            this._lblColon4.Text = "：";
            // 
            // _ktckCKYtextBox
            // 
            this._ktckCKYtextBox.Location = new System.Drawing.Point(299, 108);
            this._ktckCKYtextBox.Name = "_ktckCKYtextBox";
            this._ktckCKYtextBox.ReadOnly = true;
            this._ktckCKYtextBox.Size = new System.Drawing.Size(42, 23);
            this._ktckCKYtextBox.TabIndex = 15;
            this._ktckCKYtextBox.Text = "0";
            // 
            // _ktckOPWritebutton
            // 
            this._ktckOPWritebutton.Location = new System.Drawing.Point(350, 98);
            this._ktckOPWritebutton.Name = "_ktckOPWritebutton";
            this._ktckOPWritebutton.Size = new System.Drawing.Size(75, 23);
            this._ktckOPWritebutton.TabIndex = 16;
            this._ktckOPWritebutton.Text = "单点写入";
            this._ktckOPWritebutton.UseVisualStyleBackColor = true;
            // 
            // _ktckAPWritebutton
            // 
            this._ktckAPWritebutton.Location = new System.Drawing.Point(438, 98);
            this._ktckAPWritebutton.Name = "_ktckAPWritebutton";
            this._ktckAPWritebutton.Size = new System.Drawing.Size(75, 23);
            this._ktckAPWritebutton.TabIndex = 17;
            this._ktckAPWritebutton.Text = "批量写入";
            this._ktckAPWritebutton.UseVisualStyleBackColor = true;
            // 
            // WideScreenForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(540, 240);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this._packageNamecomboBox);
            this.Controls.Add(this._fileNameSearchCombo);
            this.Controls.Add(this._lblSourceRes);
            this.Controls.Add(this._resolutionTypecomboBox);
            this.Controls.Add(this._resolutioncomboBox);
            this.Controls.Add(this._ktckReadbutton);
            this.Controls.Add(this._ktckPListcheckedListBox);
            this.Controls.Add(this._lblSelectedCoord);
            this.Controls.Add(this._ktckKXtextBox);
            this.Controls.Add(this._lblColon2);
            this.Controls.Add(this._ktckKYtextBox);
            this.Controls.Add(this._lblWideScreenCoord);
            this.Controls.Add(this._ktckCKXtextBox);
            this.Controls.Add(this._lblColon4);
            this.Controls.Add(this._ktckCKYtextBox);
            this.Controls.Add(this._ktckOPWritebutton);
            this.Controls.Add(this._ktckAPWritebutton);
            this.Controls.Add(this._lblWideScreenTip);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WideScreenForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "超宽屏坐标转换";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
