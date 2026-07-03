using System.Drawing;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    partial class QuickTouchForm
    {
        private System.ComponentModel.IContainer components = null;
        internal TextBox _sXtextBox;
        internal TextBox _sYtextBox;
        internal TextBox _mXtextBox;
        internal TextBox _mYtextBox;
        internal TextBox _nXtextBox;
        internal TextBox _nYtextBox;
        internal CheckBox _createKeyscheckBox;
        internal CheckBox _createKeyOncecheckBox;
        internal CheckBox _topCheckBox;
        internal Button _btnGetScreenResolution;
        internal Label _lblDesktopRes;
        internal Label _lblScreenResX;
        internal Label _lblColon1;
        internal Label _lblColon3;
        internal Label _lblCurrentMouseCoord;
        internal Label _lblInternalCoord;
        internal Label _lblFullScreenTip;
        internal Label _lblAdminTip;
        internal Label _tip1label;
        internal Label _tip2label;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickTouchForm));
            this._lblFullScreenTip = new System.Windows.Forms.Label();
            this._lblAdminTip = new System.Windows.Forms.Label();
            this._lblDesktopRes = new System.Windows.Forms.Label();
            this._sXtextBox = new System.Windows.Forms.TextBox();
            this._lblScreenResX = new System.Windows.Forms.Label();
            this._sYtextBox = new System.Windows.Forms.TextBox();
            this._btnGetScreenResolution = new System.Windows.Forms.Button();
            this._topCheckBox = new System.Windows.Forms.CheckBox();
            this._createKeyOncecheckBox = new System.Windows.Forms.CheckBox();
            this._createKeyscheckBox = new System.Windows.Forms.CheckBox();
            this._lblCurrentMouseCoord = new System.Windows.Forms.Label();
            this._mXtextBox = new System.Windows.Forms.TextBox();
            this._lblColon1 = new System.Windows.Forms.Label();
            this._mYtextBox = new System.Windows.Forms.TextBox();
            this._lblInternalCoord = new System.Windows.Forms.Label();
            this._nXtextBox = new System.Windows.Forms.TextBox();
            this._lblColon3 = new System.Windows.Forms.Label();
            this._nYtextBox = new System.Windows.Forms.TextBox();
            this._tip1label = new System.Windows.Forms.Label();
            this._tip2label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _lblFullScreenTip
            // 
            this._lblFullScreenTip.AutoSize = true;
            this._lblFullScreenTip.ForeColor = System.Drawing.Color.Red;
            this._lblFullScreenTip.Location = new System.Drawing.Point(3, 9);
            this._lblFullScreenTip.Name = "_lblFullScreenTip";
            this._lblFullScreenTip.Size = new System.Drawing.Size(378, 17);
            this._lblFullScreenTip.TabIndex = 0;
            this._lblFullScreenTip.Text = "提示：按F11将MuMu模拟器全屏化后使用，建议将小助手窗口置顶。";
            // 
            // _lblAdminTip
            // 
            this._lblAdminTip.AutoSize = true;
            this._lblAdminTip.ForeColor = System.Drawing.Color.Red;
            this._lblAdminTip.Location = new System.Drawing.Point(3, 24);
            this._lblAdminTip.Name = "_lblAdminTip";
            this._lblAdminTip.Size = new System.Drawing.Size(291, 17);
            this._lblAdminTip.TabIndex = 1;
            this._lblAdminTip.Text = "提示2：如无法生成按键请以管理员模式启动小助手。";
            // 
            // _lblDesktopRes
            // 
            this._lblDesktopRes.AutoSize = true;
            this._lblDesktopRes.Location = new System.Drawing.Point(3, 46);
            this._lblDesktopRes.Name = "_lblDesktopRes";
            this._lblDesktopRes.Size = new System.Drawing.Size(80, 17);
            this._lblDesktopRes.TabIndex = 2;
            this._lblDesktopRes.Text = "桌面分辨率：";
            // 
            // _sXtextBox
            // 
            this._sXtextBox.Location = new System.Drawing.Point(83, 43);
            this._sXtextBox.Name = "_sXtextBox";
            this._sXtextBox.Size = new System.Drawing.Size(42, 23);
            this._sXtextBox.TabIndex = 3;
            this._sXtextBox.Text = "0";
            // 
            // _lblScreenResX
            // 
            this._lblScreenResX.AutoSize = true;
            this._lblScreenResX.Location = new System.Drawing.Point(129, 47);
            this._lblScreenResX.Name = "_lblScreenResX";
            this._lblScreenResX.Size = new System.Drawing.Size(16, 17);
            this._lblScreenResX.TabIndex = 4;
            this._lblScreenResX.Text = "X";
            // 
            // _sYtextBox
            // 
            this._sYtextBox.Location = new System.Drawing.Point(149, 43);
            this._sYtextBox.Name = "_sYtextBox";
            this._sYtextBox.Size = new System.Drawing.Size(42, 23);
            this._sYtextBox.TabIndex = 5;
            this._sYtextBox.Text = "0";
            // 
            // _btnGetScreenResolution
            // 
            this._btnGetScreenResolution.Location = new System.Drawing.Point(213, 43);
            this._btnGetScreenResolution.Name = "_btnGetScreenResolution";
            this._btnGetScreenResolution.Size = new System.Drawing.Size(66, 24);
            this._btnGetScreenResolution.TabIndex = 6;
            this._btnGetScreenResolution.Text = "自动获取";
            this._btnGetScreenResolution.UseVisualStyleBackColor = true;
            // 
            // _topCheckBox
            // 
            this._topCheckBox.AutoSize = true;
            this._topCheckBox.Location = new System.Drawing.Point(285, 46);
            this._topCheckBox.Name = "_topCheckBox";
            this._topCheckBox.Size = new System.Drawing.Size(75, 21);
            this._topCheckBox.TabIndex = 7;
            this._topCheckBox.Text = "窗口置顶";
            this._topCheckBox.UseVisualStyleBackColor = true;
            // 
            // _createKeyOncecheckBox
            // 
            this._createKeyOncecheckBox.AutoSize = true;
            this._createKeyOncecheckBox.Location = new System.Drawing.Point(5, 71);
            this._createKeyOncecheckBox.Name = "_createKeyOncecheckBox";
            this._createKeyOncecheckBox.Size = new System.Drawing.Size(75, 21);
            this._createKeyOncecheckBox.TabIndex = 8;
            this._createKeyOncecheckBox.Text = "生成一次";
            this._createKeyOncecheckBox.UseVisualStyleBackColor = true;
            // 
            // _createKeyscheckBox
            // 
            this._createKeyscheckBox.AutoSize = true;
            this._createKeyscheckBox.Location = new System.Drawing.Point(90, 70);
            this._createKeyscheckBox.Name = "_createKeyscheckBox";
            this._createKeyscheckBox.Size = new System.Drawing.Size(75, 21);
            this._createKeyscheckBox.TabIndex = 9;
            this._createKeyscheckBox.Text = "生成多次";
            this._createKeyscheckBox.UseVisualStyleBackColor = true;
            // 
            // _lblCurrentMouseCoord
            // 
            this._lblCurrentMouseCoord.AutoSize = true;
            this._lblCurrentMouseCoord.Location = new System.Drawing.Point(3, 100);
            this._lblCurrentMouseCoord.Name = "_lblCurrentMouseCoord";
            this._lblCurrentMouseCoord.Size = new System.Drawing.Size(92, 17);
            this._lblCurrentMouseCoord.TabIndex = 10;
            this._lblCurrentMouseCoord.Text = "当前鼠标坐标：";
            // 
            // _mXtextBox
            // 
            this._mXtextBox.Location = new System.Drawing.Point(95, 97);
            this._mXtextBox.Name = "_mXtextBox";
            this._mXtextBox.Size = new System.Drawing.Size(42, 23);
            this._mXtextBox.TabIndex = 11;
            this._mXtextBox.Text = "0";
            // 
            // _lblColon1
            // 
            this._lblColon1.AutoSize = true;
            this._lblColon1.Location = new System.Drawing.Point(141, 101);
            this._lblColon1.Name = "_lblColon1";
            this._lblColon1.Size = new System.Drawing.Size(20, 17);
            this._lblColon1.TabIndex = 12;
            this._lblColon1.Text = "：";
            // 
            // _mYtextBox
            // 
            this._mYtextBox.Location = new System.Drawing.Point(164, 97);
            this._mYtextBox.Name = "_mYtextBox";
            this._mYtextBox.Size = new System.Drawing.Size(42, 23);
            this._mYtextBox.TabIndex = 13;
            this._mYtextBox.Text = "0";
            // 
            // _lblInternalCoord
            // 
            this._lblInternalCoord.AutoSize = true;
            this._lblInternalCoord.Location = new System.Drawing.Point(3, 132);
            this._lblInternalCoord.Name = "_lblInternalCoord";
            this._lblInternalCoord.Size = new System.Drawing.Size(92, 17);
            this._lblInternalCoord.TabIndex = 14;
            this._lblInternalCoord.Text = "对应内部坐标：";
            // 
            // _nXtextBox
            // 
            this._nXtextBox.Location = new System.Drawing.Point(96, 129);
            this._nXtextBox.Name = "_nXtextBox";
            this._nXtextBox.Size = new System.Drawing.Size(42, 23);
            this._nXtextBox.TabIndex = 15;
            this._nXtextBox.Text = "0";
            // 
            // _lblColon3
            // 
            this._lblColon3.AutoSize = true;
            this._lblColon3.Location = new System.Drawing.Point(142, 133);
            this._lblColon3.Name = "_lblColon3";
            this._lblColon3.Size = new System.Drawing.Size(20, 17);
            this._lblColon3.TabIndex = 16;
            this._lblColon3.Text = "：";
            // 
            // _nYtextBox
            // 
            this._nYtextBox.Location = new System.Drawing.Point(165, 129);
            this._nYtextBox.Name = "_nYtextBox";
            this._nYtextBox.Size = new System.Drawing.Size(42, 23);
            this._nYtextBox.TabIndex = 17;
            this._nYtextBox.Text = "0";
            // 
            // _tip1label
            // 
            this._tip1label.AutoSize = true;
            this._tip1label.ForeColor = System.Drawing.Color.Red;
            this._tip1label.Location = new System.Drawing.Point(218, 75);
            this._tip1label.Name = "_tip1label";
            this._tip1label.Size = new System.Drawing.Size(128, 17);
            this._tip1label.TabIndex = 18;
            this._tip1label.Text = "提示：已关闭键盘监听";
            // 
            // _tip2label
            // 
            this._tip2label.AutoSize = true;
            this._tip2label.ForeColor = System.Drawing.Color.Red;
            this._tip2label.Location = new System.Drawing.Point(218, 93);
            this._tip2label.Name = "_tip2label";
            this._tip2label.Size = new System.Drawing.Size(116, 17);
            this._tip2label.TabIndex = 19;
            this._tip2label.Text = "                           ";
            // 
            // QuickTouchForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(460, 200);
            this.Controls.Add(this._lblFullScreenTip);
            this.Controls.Add(this._lblAdminTip);
            this.Controls.Add(this._lblDesktopRes);
            this.Controls.Add(this._sXtextBox);
            this.Controls.Add(this._lblScreenResX);
            this.Controls.Add(this._sYtextBox);
            this.Controls.Add(this._btnGetScreenResolution);
            this.Controls.Add(this._topCheckBox);
            this.Controls.Add(this._createKeyOncecheckBox);
            this.Controls.Add(this._createKeyscheckBox);
            this.Controls.Add(this._lblCurrentMouseCoord);
            this.Controls.Add(this._mXtextBox);
            this.Controls.Add(this._lblColon1);
            this.Controls.Add(this._mYtextBox);
            this.Controls.Add(this._lblInternalCoord);
            this.Controls.Add(this._nXtextBox);
            this.Controls.Add(this._lblColon3);
            this.Controls.Add(this._nYtextBox);
            this.Controls.Add(this._tip1label);
            this.Controls.Add(this._tip2label);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QuickTouchForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "全屏快速摸点";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
