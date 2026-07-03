using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class MouseTrackForm
    {
        private System.ComponentModel.IContainer components = null;
        internal TextBox _ncXtextBox;
        internal TextBox _ncYtextBox;
        internal TextBox _scXtextBox;
        internal TextBox _scYtextBox;
        internal TextBox _findKeytextBox;
        internal TextBox _resetKeytextBox;
        internal CheckBox _topCheckBox;
        internal CheckBox _cCheckBox;
        internal CheckBox _eCheckBox;
        internal Button _saveKeybutton;
        internal Button _loadKeybutton;
        internal Label _lblMouseCurrentX;
        internal Label _lblMouseCurrentY;
        internal Label _lblMouseSavedX;
        internal Label _lblMouseSavedY;
        internal Label _lblCtrlPrefix1;
        internal Label _lblCtrlPrefix2;
        internal Label _lblSaveMouseCoord;
        internal Label _lblMoveMouseToSaved;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MouseTrackForm));
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._cCheckBox = new System.Windows.Forms.CheckBox();
            this._lblMouseCurrentX = new System.Windows.Forms.Label();
            this._ncXtextBox = new System.Windows.Forms.TextBox();
            this._lblMouseCurrentY = new System.Windows.Forms.Label();
            this._ncYtextBox = new System.Windows.Forms.TextBox();
            this._lblMouseSavedX = new System.Windows.Forms.Label();
            this._scXtextBox = new System.Windows.Forms.TextBox();
            this._lblMouseSavedY = new System.Windows.Forms.Label();
            this._scYtextBox = new System.Windows.Forms.TextBox();
            this._eCheckBox = new System.Windows.Forms.CheckBox();
            this._lblCtrlPrefix1 = new System.Windows.Forms.Label();
            this._findKeytextBox = new System.Windows.Forms.TextBox();
            this._lblCtrlPrefix2 = new System.Windows.Forms.Label();
            this._resetKeytextBox = new System.Windows.Forms.TextBox();
            this._saveKeybutton = new System.Windows.Forms.Button();
            this._loadKeybutton = new System.Windows.Forms.Button();
            this._lblSaveMouseCoord = new System.Windows.Forms.Label();
            this._lblMoveMouseToSaved = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(3, 12);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 0;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _cCheckBox
            // 
            this._cCheckBox.AutoSize = true;
            this._cCheckBox.Location = new System.Drawing.Point(85, 12);
            this._cCheckBox.Name = "_cCheckBox";
            this._cCheckBox.Size = new System.Drawing.Size(123, 21);
            this._cCheckBox.TabIndex = 1;
            this._cCheckBox.Text = "启用鼠标坐标捕捉";
            this._cCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lblMouseCurrentX
            // 
            this._lblMouseCurrentX.AutoSize = true;
            this._lblMouseCurrentX.Location = new System.Drawing.Point(7, 42);
            this._lblMouseCurrentX.Name = "_lblMouseCurrentX";
            this._lblMouseCurrentX.Size = new System.Drawing.Size(100, 17);
            this._lblMouseCurrentX.TabIndex = 2;
            this._lblMouseCurrentX.Text = "当前鼠标坐标X：";
            // 
            // _ncXtextBox
            // 
            this._ncXtextBox.Location = new System.Drawing.Point(108, 39);
            this._ncXtextBox.Name = "_ncXtextBox";
            this._ncXtextBox.Size = new System.Drawing.Size(42, 23);
            this._ncXtextBox.TabIndex = 3;
            this._ncXtextBox.Text = "0";
            // 
            // _lblMouseCurrentY
            // 
            this._lblMouseCurrentY.AutoSize = true;
            this._lblMouseCurrentY.Location = new System.Drawing.Point(7, 73);
            this._lblMouseCurrentY.Name = "_lblMouseCurrentY";
            this._lblMouseCurrentY.Size = new System.Drawing.Size(99, 17);
            this._lblMouseCurrentY.TabIndex = 4;
            this._lblMouseCurrentY.Text = "当前鼠标坐标Y：";
            // 
            // _ncYtextBox
            // 
            this._ncYtextBox.Location = new System.Drawing.Point(108, 70);
            this._ncYtextBox.Name = "_ncYtextBox";
            this._ncYtextBox.Size = new System.Drawing.Size(42, 23);
            this._ncYtextBox.TabIndex = 5;
            this._ncYtextBox.Text = "0";
            // 
            // _lblMouseSavedX
            // 
            this._lblMouseSavedX.AutoSize = true;
            this._lblMouseSavedX.Location = new System.Drawing.Point(166, 42);
            this._lblMouseSavedX.Name = "_lblMouseSavedX";
            this._lblMouseSavedX.Size = new System.Drawing.Size(76, 17);
            this._lblMouseSavedX.TabIndex = 6;
            this._lblMouseSavedX.Text = "保存坐标X：";
            // 
            // _scXtextBox
            // 
            this._scXtextBox.Location = new System.Drawing.Point(243, 39);
            this._scXtextBox.Name = "_scXtextBox";
            this._scXtextBox.Size = new System.Drawing.Size(42, 23);
            this._scXtextBox.TabIndex = 7;
            this._scXtextBox.Text = "0";
            // 
            // _lblMouseSavedY
            // 
            this._lblMouseSavedY.AutoSize = true;
            this._lblMouseSavedY.Location = new System.Drawing.Point(166, 73);
            this._lblMouseSavedY.Name = "_lblMouseSavedY";
            this._lblMouseSavedY.Size = new System.Drawing.Size(75, 17);
            this._lblMouseSavedY.TabIndex = 8;
            this._lblMouseSavedY.Text = "保存坐标Y：";
            // 
            // _scYtextBox
            // 
            this._scYtextBox.Location = new System.Drawing.Point(242, 70);
            this._scYtextBox.Name = "_scYtextBox";
            this._scYtextBox.Size = new System.Drawing.Size(42, 23);
            this._scYtextBox.TabIndex = 9;
            this._scYtextBox.Text = "0";
            // 
            // _eCheckBox
            // 
            this._eCheckBox.AutoSize = true;
            this._eCheckBox.Location = new System.Drawing.Point(305, 12);
            this._eCheckBox.Name = "_eCheckBox";
            this._eCheckBox.Size = new System.Drawing.Size(87, 21);
            this._eCheckBox.TabIndex = 10;
            this._eCheckBox.Text = "编辑快捷键";
            this._eCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lblCtrlPrefix1
            // 
            this._lblCtrlPrefix1.AutoSize = true;
            this._lblCtrlPrefix1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this._lblCtrlPrefix1.Location = new System.Drawing.Point(309, 40);
            this._lblCtrlPrefix1.Name = "_lblCtrlPrefix1";
            this._lblCtrlPrefix1.Size = new System.Drawing.Size(45, 19);
            this._lblCtrlPrefix1.TabIndex = 11;
            this._lblCtrlPrefix1.Text = "Ctrl+";
            // 
            // _findKeytextBox
            // 
            this._findKeytextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this._findKeytextBox.Location = new System.Drawing.Point(362, 39);
            this._findKeytextBox.Name = "_findKeytextBox";
            this._findKeytextBox.ReadOnly = true;
            this._findKeytextBox.Size = new System.Drawing.Size(41, 23);
            this._findKeytextBox.TabIndex = 12;
            this._findKeytextBox.Text = "D";
            // 
            // _lblCtrlPrefix2
            // 
            this._lblCtrlPrefix2.AutoSize = true;
            this._lblCtrlPrefix2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this._lblCtrlPrefix2.Location = new System.Drawing.Point(309, 73);
            this._lblCtrlPrefix2.Name = "_lblCtrlPrefix2";
            this._lblCtrlPrefix2.Size = new System.Drawing.Size(45, 19);
            this._lblCtrlPrefix2.TabIndex = 13;
            this._lblCtrlPrefix2.Text = "Ctrl+";
            // 
            // _resetKeytextBox
            // 
            this._resetKeytextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this._resetKeytextBox.Location = new System.Drawing.Point(362, 70);
            this._resetKeytextBox.Name = "_resetKeytextBox";
            this._resetKeytextBox.ReadOnly = true;
            this._resetKeytextBox.Size = new System.Drawing.Size(41, 23);
            this._resetKeytextBox.TabIndex = 14;
            this._resetKeytextBox.Text = "F";
            // 
            // _saveKeybutton
            // 
            this._saveKeybutton.Location = new System.Drawing.Point(394, 8);
            this._saveKeybutton.Name = "_saveKeybutton";
            this._saveKeybutton.Size = new System.Drawing.Size(70, 25);
            this._saveKeybutton.TabIndex = 15;
            this._saveKeybutton.Text = "保存设置";
            this._saveKeybutton.UseVisualStyleBackColor = true;
            // 
            // _loadKeybutton
            // 
            this._loadKeybutton.Location = new System.Drawing.Point(467, 8);
            this._loadKeybutton.Name = "_loadKeybutton";
            this._loadKeybutton.Size = new System.Drawing.Size(70, 25);
            this._loadKeybutton.TabIndex = 16;
            this._loadKeybutton.Text = "读取设置";
            this._loadKeybutton.UseVisualStyleBackColor = true;
            // 
            // _lblSaveMouseCoord
            // 
            this._lblSaveMouseCoord.AutoSize = true;
            this._lblSaveMouseCoord.Location = new System.Drawing.Point(409, 42);
            this._lblSaveMouseCoord.Name = "_lblSaveMouseCoord";
            this._lblSaveMouseCoord.Size = new System.Drawing.Size(104, 17);
            this._lblSaveMouseCoord.TabIndex = 17;
            this._lblSaveMouseCoord.Text = "保存当前鼠标坐标";
            // 
            // _lblMoveMouseToSaved
            // 
            this._lblMoveMouseToSaved.AutoSize = true;
            this._lblMoveMouseToSaved.Location = new System.Drawing.Point(409, 75);
            this._lblMoveMouseToSaved.Name = "_lblMoveMouseToSaved";
            this._lblMoveMouseToSaved.Size = new System.Drawing.Size(116, 17);
            this._lblMoveMouseToSaved.TabIndex = 18;
            this._lblMoveMouseToSaved.Text = "移动鼠标至保存位置";
            // 
            // MouseTrackForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(540, 170);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this._cCheckBox);
            this.Controls.Add(this._lblMouseCurrentX);
            this.Controls.Add(this._ncXtextBox);
            this.Controls.Add(this._lblMouseCurrentY);
            this.Controls.Add(this._ncYtextBox);
            this.Controls.Add(this._lblMouseSavedX);
            this.Controls.Add(this._scXtextBox);
            this.Controls.Add(this._lblMouseSavedY);
            this.Controls.Add(this._scYtextBox);
            this.Controls.Add(this._eCheckBox);
            this.Controls.Add(this._lblCtrlPrefix1);
            this.Controls.Add(this._findKeytextBox);
            this.Controls.Add(this._lblCtrlPrefix2);
            this.Controls.Add(this._resetKeytextBox);
            this.Controls.Add(this._saveKeybutton);
            this.Controls.Add(this._loadKeybutton);
            this.Controls.Add(this._lblSaveMouseCoord);
            this.Controls.Add(this._lblMoveMouseToSaved);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MouseTrackForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "鼠标回溯";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
